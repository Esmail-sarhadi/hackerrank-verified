# Super Functional Strings

Sum, over every distinct substring of S, of `len^d` where `d` is the number of
distinct characters in that substring, modulo 1e9+7.

- **Problem:** https://www.hackerrank.com/challenges/super-functional-strings
- **Input:** a single lowercase string S
- **Constraints:** |S| up to 1e5
- **Status:** Verified: matches the sample and an independent brute-force answer.

## Sample

```
tzglwviwqtvfzblgtwt
```

Expected output:

```
844081342
```

## Notes

Suffix array plus Kasai LCP: each suffix contributes the lengths from
`lcp[i]+1` to `n - sa[i]`, and the distinct-character count stays constant
between consecutive "next occurrence" offsets, so those ranges are summed with
precomputed prefix sums of `L^d`.

Verified against a brute force that enumerates every distinct substring and
computes `len^distinct` directly, on 32 random strings of length 1..8.

One bug worth recording: the counting-sort buffer was sized `n + 2`, but a
letter code (`z` = 25) can exceed `n + 1` on short strings, which corrupted the
suffix array. Sizing it `max(n + 2, 27)` fixes it.

---

The statement above is a summary for orientation, not a copy of the problem
text, which belongs to HackerRank.
