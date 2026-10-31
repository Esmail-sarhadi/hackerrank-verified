# Huarongdao

A N x M board of movable and fixed blocks, one empty cell. Moving a block into the empty cell costs 1 second; cheating costs K seconds. For each query, find the minimum time to move the Cao Cao block to the exit.

- **Problem:** https://www.hackerrank.com/challenges/huarongdao
- **Input:** N M K Q, then the board, then Q lines of `EX EY SX SY TX TY`.
- **Constraints:** N, M <= 200, Q <= 250, 10 <= K <= 15
- **Status:** Verified for correctness; too slow at the maximum input (5.2 s at 200 x 200).

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

States are (block position, which side of it the empty cell is on). Moving the
hole from one side to another is a bounded BFS that may not pass through the
block, and cheating is always the alternative, so the precomputation is finite.
Dijkstra over those states gives the cheapest route.

Verified against a from-scratch Dijkstra over (block, empty) pairs - no shared
logic - on four boards, two cheat costs and four random queries each.

It is still slow: 5.2 s for 200 x 200 with 250 queries, which is at or past the
usual limit. Published as a correct reference, not as an accepted submission.

---

The statement above is a summary for orientation, not a copy of the problem
text, which belongs to HackerRank. Open the link for the authoritative version.
