# Even Odd Query

Given an array and Q queries (x, y), decide whether `find(x, y)` is Odd or Even, where `find(x,y) = A[x] ^ find(x+1,y)` and `find(x,y) = 1` when x > y.

- **Problem:** https://www.hackerrank.com/challenges/even-odd-query
- **Input:** N, then N values, then Q, then Q lines of `x y`.
- **Constraints:** 2 <= N, Q <= 1e5, values 0..9, no two consecutive zeros, 1-indexed, x <= y
- **Status:** Verified: compiles, matches the sample, and matches an independent brute-force answer.

## Sample

```
3
3 2 7
2
1 2
2 3
```

Expected output:

```
Odd
Even
```

## Notes

The power is never computed: the result is odd iff A[x] is odd, or A[x] is even and the exponent is 0, which happens exactly when A[x+1] == 0.

---

The statement above is a summary for orientation, not a copy of the problem
text, which belongs to HackerRank. Open the link for the authoritative version.
