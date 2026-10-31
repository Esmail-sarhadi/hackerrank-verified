#!/usr/bin/env python3
"""Compile every solution and run it against its sample.

A solution folder is any directory under solutions/ that contains a
solution.csproj, a Solution.cs and a pair of sample.in / sample.out files.
Nothing is taken on trust: if the program does not build, or its output differs
from the expected output by even one character after trailing whitespace is
normalised, this script fails.

    python tools/verify.py            # everything
    python tools/verify.py special    # only folders matching a substring
"""

from __future__ import annotations

import argparse
import pathlib
import shutil
import subprocess
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
SOLUTIONS = ROOT / "solutions"
BUILD_TIMEOUT = 240
RUN_TIMEOUT = 30


def normalise(text: str) -> list[str]:
    """Compare line by line, ignoring trailing spaces and trailing blank lines."""
    lines = [line.rstrip() for line in text.replace("\r\n", "\n").split("\n")]
    while lines and lines[-1] == "":
        lines.pop()
    return lines


def build(folder: pathlib.Path) -> tuple[bool, str]:
    result = subprocess.run(
        ["dotnet", "build", "-c", "Release", "--nologo", "-v", "quiet"],
        cwd=folder,
        capture_output=True,
        text=True,
        timeout=BUILD_TIMEOUT,
    )
    if result.returncode != 0:
        return False, (result.stdout + result.stderr).strip()
    return True, ""


def run(folder: pathlib.Path, stdin_text: str) -> tuple[bool, str, str]:
    binary = folder / "bin" / "Release" / "net10.0" / "solution.dll"
    if not binary.exists():
        return False, "", "expected %s to exist after a successful build" % binary
    try:
        result = subprocess.run(
            ["dotnet", str(binary)],
            input=stdin_text,
            capture_output=True,
            text=True,
            timeout=RUN_TIMEOUT,
        )
    except subprocess.TimeoutExpired:
        return False, "", "timed out after %d s" % RUN_TIMEOUT
    if result.returncode != 0:
        return False, result.stdout, "exit code %d\n%s" % (result.returncode, result.stderr.strip())
    return True, result.stdout, ""


def check(folder: pathlib.Path) -> tuple[str, str]:
    """Returns (status, detail) where status is PASS, FAIL or SKIP."""
    required = ["Solution.cs", "solution.csproj", "sample.in", "sample.out"]
    missing = [name for name in required if not (folder / name).exists()]
    if missing:
        return "SKIP", "missing " + ", ".join(missing)

    ok, error = build(folder)
    if not ok:
        return "FAIL", "build failed:\n" + error

    stdin_text = (folder / "sample.in").read_text(encoding="utf-8")
    expected = normalise((folder / "sample.out").read_text(encoding="utf-8"))

    ok, stdout, error = run(folder, stdin_text)
    if not ok:
        return "FAIL", error

    actual = normalise(stdout)
    if actual == expected:
        return "PASS", "%d line(s)" % len(expected)

    detail = ["expected %d line(s), got %d" % (len(expected), len(actual))]
    for index in range(max(len(expected), len(actual))):
        want = expected[index] if index < len(expected) else "<missing>"
        got = actual[index] if index < len(actual) else "<missing>"
        if want != got:
            detail.append("  line %d: expected %r, got %r" % (index + 1, want, got))
    return "FAIL", "\n".join(detail)


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("filter", nargs="?", default="", help="only folders matching this substring")
    args = parser.parse_args()

    if shutil.which("dotnet") is None:
        print("dotnet is not on PATH; install the .NET SDK to run this", file=sys.stderr)
        return 2

    folders = sorted(p for p in SOLUTIONS.iterdir() if p.is_dir())
    if args.filter:
        folders = [p for p in folders if args.filter in p.name]

    if not folders:
        print("no solution folders found", file=sys.stderr)
        return 2

    failures = 0
    for folder in folders:
        status, detail = check(folder)
        print("%-6s %-32s %s" % (status, folder.name, detail.replace("\n", "\n       ")))
        if status == "FAIL":
            failures += 1

    total = len(folders)
    print("\n%d solution(s), %d passed, %d failed" % (total, total - failures, failures))
    return 1 if failures else 0


if __name__ == "__main__":
    raise SystemExit(main())
