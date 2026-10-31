#!/usr/bin/env python3
"""Cross-check the solutions against independent brute force.

A sample test only proves the program agrees with one hand-written case. This
script recomputes the answers a second way - by enumerating, in Python, without
reusing any of the C# logic - and compares.

    python tools/crosscheck.py

The bounds are deliberately small so the reference answer stays cheap. That is
the point: an independent answer that is obviously correct beats a fast one that
might share the same bug.
"""

from __future__ import annotations

import itertools
import pathlib
import subprocess
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
SOLUTIONS = ROOT / "solutions"
MOD = 1000000007


def run(folder: str, stdin_text: str) -> list[str]:
    binary = SOLUTIONS / folder / "bin" / "Release" / "net10.0" / "solution.dll"
    if not binary.exists():
        raise SystemExit("%s is not built; run tools/verify.py first" % folder)
    result = subprocess.run(
        ["dotnet", str(binary)], input=stdin_text, capture_output=True, text=True, timeout=120
    )
    if result.returncode != 0:
        raise SystemExit("%s failed: %s" % (folder, result.stderr.strip()))
    return [line for line in result.stdout.replace("\r\n", "\n").split("\n") if line != ""]


# ---------------------------------------------------------------------------
# Sherlock and Permutations
# ---------------------------------------------------------------------------
def check_sherlock() -> list[str]:
    cases = [(n, m) for n in range(1, 6) for m in range(1, 6)]
    stdin = "%d\n%s\n" % (len(cases), "\n".join("%d %d" % c for c in cases))
    actual = run("sherlock-and-permutations", stdin)

    problems = []
    for (n, m), got in zip(cases, actual):
        letters = "0" * n + "1" * m
        unique = {"".join(p) for p in itertools.permutations(letters)}
        expected = sum(1 for word in unique if word[0] == "1") % MOD
        if int(got) != expected:
            problems.append("n=%d m=%d: expected %d, got %s" % (n, m, expected, got))
    return problems


# ---------------------------------------------------------------------------
# Even Odd Query
# ---------------------------------------------------------------------------
def check_even_odd() -> list[str]:
    """Exact integer evaluation, only on arrays small enough to evaluate."""
    arrays = []
    for values in itertools.product(range(3), repeat=4):
        if any(values[i] == 0 and values[i + 1] == 0 for i in range(3)):
            continue  # forbidden by the constraints
        arrays.append(values)

    problems = []
    for values in arrays:
        queries = [(x, y) for x in range(1, 5) for y in range(x, 5)]
        stdin = "%d\n%s\n%d\n%s\n" % (
            len(values),
            " ".join(str(v) for v in values),
            len(queries),
            "\n".join("%d %d" % q for q in queries),
        )
        actual = run("even-odd-query", stdin)

        for (x, y), got in zip(queries, actual):
            expected = _parity(values, x, y)
            if got != expected:
                problems.append("array=%s query=(%d,%d): expected %s, got %s"
                                % (values, x, y, expected, got))
    return problems


def _parity(values, x, y) -> str:
    """Evaluate find(x, y) exactly; the bounds keep the number small."""
    def find(i, j):
        if i > j:
            return 1
        exponent = find(i + 1, j)
        if exponent > 64:  # would be astronomically large, and unreachable here
            raise OverflowError
        return values[i - 1] ** exponent

    return "Odd" if find(x, y) % 2 else "Even"


# ---------------------------------------------------------------------------
# Special Multiple
# ---------------------------------------------------------------------------
def check_special_multiple() -> list[str]:
    ns = list(range(1, 201))
    stdin = "%d\n%s\n" % (len(ns), "\n".join(str(n) for n in ns))
    actual = run("special-multiple", stdin)

    problems = []
    for n, answer in zip(ns, actual):
        if set(answer) - {"9", "0"}:
            problems.append("n=%d: %r contains digits other than 9 and 0" % (n, answer))
            continue
        if answer[0] != "9":
            problems.append("n=%d: %r starts with 0" % (n, answer))
            continue
        if int(answer) % n != 0:
            problems.append("n=%d: %r is not a multiple" % (n, answer))
            continue
        if len(answer) <= 12:
            for length in range(1, len(answer)):
                if any(int("9" + rest) % n == 0
                       for rest in map("".join, itertools.product("90", repeat=length - 1))):
                    problems.append("n=%d: %r is not the smallest" % (n, answer))
                    break
    return problems


CHECKS = [
    ("sherlock-and-permutations", check_sherlock),
    ("even-odd-query", check_even_odd),
    ("special-multiple", check_special_multiple),
]


def main() -> int:
    failures = 0
    for name, function in CHECKS:
        problems = function()
        if problems:
            failures += len(problems)
            print("FAIL %s" % name)
            for problem in problems[:10]:
                print("       " + problem)
        else:
            print("PASS %s" % name)
    print("\n%d problem(s)" % failures)
    return 1 if failures else 0


if __name__ == "__main__":
    raise SystemExit(main())
