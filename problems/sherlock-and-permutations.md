# Sherlock and Permutations

Count the distinct permutations of a string of N zeros and M ones that begin with a 1, modulo 1e9+7.

- **Problem:** https://www.hackerrank.com/challenges/sherlock-and-permutations
- **Input:** First line T. Each test case is `N M`.
- **Constraints:** 1 <= T <= 200, 1 <= N, M <= 1000
- **Status:** Verified: compiles, matches the sample, and matches an independent brute-force answer.

## Sample

```
2
1 1
2 3
```

Expected output:

```
1
6
```

## Notes

Fix the first character as 1; the rest is a multiset of N zeros and M-1 ones, so the answer is C(N+M-1, N).

---

The statement above is a summary for orientation, not a copy of the problem
text, which belongs to HackerRank. Open the link for the authoritative version.
