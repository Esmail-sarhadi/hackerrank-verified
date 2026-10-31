# How Many Substrings?

For each query (l, r), count the distinct substrings of S inside that inclusive range.

- **Problem:** https://www.hackerrank.com/challenges/how-many-substrings
- **Input:** N and Q, then S, then Q lines of `l r`.
- **Constraints:** N, Q up to 1e5
- **Status:** Not verified - Time Limit Exceeded on the judge.

## Sample

```
5 5
aabaa
1 1
1 4
1 1
1 4
0 2
```

Expected output:

```
1
8
1
8
5
```

## Notes

The first versions were quadratic by the author's own admission and timed out at n = q = 100000. The final revision was never run. Not published.

---

The statement above is a summary for orientation, not a copy of the problem
text, which belongs to HackerRank. Open the link for the authoritative version.
