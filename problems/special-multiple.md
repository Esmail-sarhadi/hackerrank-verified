# Special Multiple

For each N, find the least positive integer made only of the digits 9 and 0 that is divisible by N.

- **Problem:** https://www.hackerrank.com/challenges/special-multiple
- **Input:** First line T, then T lines each with N.
- **Constraints:** 1 <= T <= 1e4, 1 <= N <= 500
- **Status:** Verified: compiles, matches the sample, and matches an independent brute-force answer.

## Sample

```
3
5
7
1
```

Expected output:

```
90
9009
9
```

## Notes

BFS over remainders modulo N; appending a digit maps r to (10r + d) mod N. The answer is built as a string because it can exceed any integer type.

---

The statement above is a summary for orientation, not a copy of the problem
text, which belongs to HackerRank. Open the link for the authoritative version.
