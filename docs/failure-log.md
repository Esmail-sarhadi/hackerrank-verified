# Failure log

Three of the eight generated solutions are not in this repository. This file
records what each one did, because the failure modes are more instructive than
the successes.

## Gridland Provinces

**Wrong Answer** on the first submission. The reply was an attempt to add
"every perimeter rotation in both directions" and switch to two prime-modulus
hashes, followed by:

> I haven't run the supplied test case, and its truncated input prevents
> verifying those 15 expected outputs here.

A second revision is described but never shown to pass. Nothing here is
verified, so nothing here is published.

**Lesson:** a hashing-based count of distinct strings is a probabilistic answer
to an exact question. If the problem asks "how many distinct strings", a
collision is a wrong answer, not a rare event to be ignored.

## Letter Islands - resolved

```
Solution.cs(234,22): error CS0266: Cannot implicitly convert type 'long' to 'int'.
An explicit conversion exists (are you missing a cast?)
```

The function had been widened to return `long` - correctly, since the answer can
exceed `int` - but the `Main` that HackerRank generates still declared `int`.
The fix is one line in the caller.

The failure is worth keeping in the log even though the solution is now in
`solutions/`: the bug is at the seam between the function and the platform's
template, which is exactly the place a chat answer does not look, because it is
not in the code block being edited.

## Pseudo-Isomorphic Substrings

This one failed four times, in three different ways:

| Attempt | Result |
|---|---|
| 1 | `CS0029: Cannot implicitly convert type 'List<long>' to 'List<int>'` |
| 2 | Time Limit Exceeded |
| 3 | `CS0101: The namespace already contains a definition for 'Solution'` |
| 4 | Time Limit Exceeded on `aaaa...`, then on `koko...`, then on a repeated 24-character pattern |

The revisions added a fast path for a single repeated letter, then for a
repeated pattern, then replaced hashing with a suffix array. Each one moved the
timeout to the next input shape instead of removing it. Three special cases were
added before the general algorithm was fixed - which is the classic way to turn
one bug into four.

**Lesson:** if a fix is "add a special case for the input that failed", the
algorithm is still wrong. The special case only hides it until the next input.

## How Many Substrings

Time Limit Exceeded at `n = q = 100000`, with the author's own note attached to
the code:

> worst case O(n^2 + q log q), so it may time out on large tests with varied
> start points. I do not present this as a guaranteed full solution.

The next revision claims a suffix automaton with a link-cut tree and
`O(n log^2 n + q log n)`, and ends with "I cannot confirm it passes without
running HackerRank's tests". It was never run.

**Lesson:** a stated complexity is not a measured one. This repository only
publishes what the timer agreed with.

## Huarongdao - resolved, but slow

Time Limit Exceeded at `200 x 200` with 250 queries. The approach - states of
(box position, empty cell side) with Dijkstra - is the standard one, but the
precomputation runs a bounded BFS for every cell and every pair of sides, which
is roughly 64 million operations before the first query is answered.

The current version is verified correct against an independent Dijkstra on small
boards, and still takes 5.2 s at the maximum input. It is in `solutions/` as a
correct reference, with that number written down rather than hidden.

Also worth noting: the problem's function signature returns a single `int` while
the input has `Q` queries. The template is wrong, and every attempt had to
change the signature - a detail that costs a submission if you paste the
generated code into the template without reading it.

## The pattern

| Failure | Count | Now |
|---|---|---|
| Compile error (type mismatch at the caller) | 2 | 1 fixed and published |
| Time Limit Exceeded | 4 | 1 fixed but still slow, 3 unresolved |
| Wrong Answer | 1 | unresolved |

Every single one is invisible in a chat transcript and immediately visible to a
compiler, a sample test, or a stopwatch. That is the whole argument for the
harness in `tools/`.
