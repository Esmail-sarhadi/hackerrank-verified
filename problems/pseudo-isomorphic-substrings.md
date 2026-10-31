# Pseudo-Isomorphic Substrings

Two strings are pseudo-isomorphic when their equality pattern is the same (abb and xyy). For every prefix of S, count the largest set of substrings of that prefix with no two pseudo-isomorphic.

- **Problem:** https://www.hackerrank.com/challenges/pseudo-isomorphic-substrings
- **Input:** A single string S.
- **Constraints:** |S| <= 1e5, lowercase letters
- **Status:** Not verified - Time Limit Exceeded on the judge.

## Sample

```
abbabab
```

Expected output:

```
1
2
4
6
9
12
15
```

## Notes

Four attempts: one type error, one duplicate-class error, and three Time Limit Exceeded on three different input shapes. Not published.

---

The statement above is a summary for orientation, not a copy of the problem
text, which belongs to HackerRank. Open the link for the authoritative version.
