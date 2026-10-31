# Letter Islands

A substring marks every position it covers. Count how many distinct substrings of S mark exactly K groups of contiguous positions.

- **Problem:** https://www.hackerrank.com/challenges/letter-islands
- **Input:** The string S, then K.
- **Constraints:** |S| up to 1e5, lowercase letters
- **Status:** Verified: compiles, matches the sample, and matches an independent brute-force answer.

## Sample

```
abaab
2
```

Expected output:

```
3
```

## Notes

A suffix automaton groups the distinct substrings that share the same occurrence
positions. For a group with P occurrences, a substring of length L produces
`1 + #{consecutive gaps > L}` islands, so "exactly K islands" is a range of
lengths and the group contributes the size of that range.

Verified against a brute force that enumerates every distinct substring, marks
the positions it covers and counts the groups - for all strings of length 1..7
over `{a,b}` and every k. Runs in 0.93 s at n = 100000.

---

The statement above is a summary for orientation, not a copy of the problem
text, which belongs to HackerRank. Open the link for the authoritative version.
