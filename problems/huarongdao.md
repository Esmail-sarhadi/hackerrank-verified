# Huarongdao

A N x M board of movable and fixed blocks, one empty cell. Moving a block into the empty cell costs 1 second; cheating costs K seconds. For each query, find the minimum time to move the Cao Cao block to the exit.

- **Problem:** https://www.hackerrank.com/challenges/huarongdao
- **Input:** N M K Q, then the board, then Q lines of `EX EY SX SY TX TY`.
- **Constraints:** N, M <= 200, Q <= 250, 10 <= K <= 15
- **Status:** Not verified - Time Limit Exceeded on the judge.

## Sample

```
5 5 12 1
1 1 1 1 1
1 1 1 1 1
0 1 1 1 1
1 1 1 1 1
0 1 0 1 1
1 5 4 3 4 1
```

Expected output:

```
20
```

## Notes

Timed out at 200 x 200 with 250 queries. The approach is the standard one; the precomputation cost was not. Not published.

---

The statement above is a summary for orientation, not a copy of the problem
text, which belongs to HackerRank. Open the link for the authoritative version.
