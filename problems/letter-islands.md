# Letter Islands

A substring marks every position it covers. Count how many distinct substrings of S mark exactly K groups of contiguous positions.

- **Problem:** https://www.hackerrank.com/challenges/letter-islands
- **Input:** The string S, then K.
- **Constraints:** |S| up to 1e5, lowercase letters
- **Status:** Not verified - never run against the judge after the compile error was fixed.

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

A suffix-automaton solution was produced, but it first failed to compile (CS0266, int vs long at the caller) and was never run against the judge afterwards.

---

The statement above is a summary for orientation, not a copy of the problem
text, which belongs to HackerRank. Open the link for the authoritative version.
