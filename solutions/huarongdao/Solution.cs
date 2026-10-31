using System;
using System.Collections.Generic;
using System.Text;

// Huarongdao
// https://www.hackerrank.com/challenges/huarongdao
//
// A state is (Cao Cao position, which side of him the empty cell is on), so
// there are 4 * N * M of them. Moving the empty cell from one side of the block
// to another is a small BFS that may not pass through the block itself, and it
// can always be replaced by cheating, which costs K. Then Dijkstra over those
// states gives the cheapest way to walk the block to the exit.
//
// The BFS is bounded by K - 1 steps because beyond that cheating is cheaper,
// which is what keeps the precomputation finite.
//
// Status: passes the sample. Not measured against the judge's time limit.

class Result
{
    private class Heap
    {
        private readonly List<int> states = new List<int>();
        private readonly List<int> costs = new List<int>();

        public int Count { get { return states.Count; } }

        public void Push(int state, int cost)
        {
            int i = states.Count;
            states.Add(state); costs.Add(cost);
            while (i > 0)
            {
                int p = (i - 1) / 2;
                if (costs[p] <= cost) break;
                states[i] = states[p]; costs[i] = costs[p]; i = p;
            }
            states[i] = state; costs[i] = cost;
        }

        public void Pop(out int state, out int cost)
        {
            state = states[0]; cost = costs[0];
            int last = states.Count - 1;
            int s = states[last], c = costs[last];
            states.RemoveAt(last); costs.RemoveAt(last);
            if (last == 0) return;

            int i = 0;
            while (2 * i + 1 < last)
            {
                int child = 2 * i + 1;
                if (child + 1 < last && costs[child + 1] < costs[child]) child++;
                if (costs[child] >= c) break;
                states[i] = states[child]; costs[i] = costs[child]; i = child;
            }
            states[i] = s; costs[i] = c;
        }
    }

    private class Solver
    {
        private readonly int rows, columns, size, cheat;
        private readonly int[] neighbor, seen, depth, queue;
        private readonly bool[] open;
        private readonly byte[] turn;
        private readonly int[] component;
        private int stamp;

        public Solver(int cheatCost, List<List<int>> board)
        {
            rows = board.Count;
            columns = board[0].Count;
            size = rows * columns;
            cheat = cheatCost;

            open = new bool[size];
            neighbor = new int[size * 4];
            seen = new int[size];
            depth = new int[size];
            queue = new int[size];
            turn = new byte[size * 16];
            component = new int[size];

            for (int i = 0; i < neighbor.Length; i++) neighbor[i] = -1;
            for (int i = 0; i < size; i++) open[i] = board[i / columns][i % columns] == 1;

            int[] dr = { -1, 0, 1, 0 };
            int[] dc = { 0, 1, 0, -1 };
            for (int u = 0; u < size; u++)
            {
                if (!open[u]) continue;
                for (int d = 0; d < 4; d++)
                {
                    int r = u / columns + dr[d];
                    int c = u % columns + dc[d];
                    if (r >= 0 && r < rows && c >= 0 && c < columns && open[r * columns + c])
                        neighbor[u * 4 + d] = r * columns + c;
                }
            }

            int id = 0;
            for (int u = 0; u < size; u++)
            {
                if (!open[u] || component[u] != 0) continue;
                int head = 0, tail = 0;
                queue[tail++] = u;
                component[u] = ++id;
                while (head < tail)
                {
                    int v = queue[head++];
                    for (int d = 0; d < 4; d++)
                    {
                        int w = neighbor[v * 4 + d];
                        if (w >= 0 && component[w] == 0) { component[w] = id; queue[tail++] = w; }
                    }
                }
            }

            for (int u = 0; u < size; u++)
            {
                if (!open[u]) continue;
                for (int a = 0; a < 4; a++)
                {
                    int start = neighbor[u * 4 + a];
                    if (start < 0) continue;
                    Bfs(start, u);
                    for (int b = 0; b < 4; b++)
                    {
                        int target = neighbor[u * 4 + b];
                        if (target >= 0)
                            turn[u * 16 + a * 4 + b] = (byte)(seen[target] == stamp ? depth[target] : cheat);
                    }
                }
            }
        }

        private void Bfs(int start, int blocked)
        {
            stamp++;
            int head = 0, tail = 0;
            queue[tail++] = start;
            seen[start] = stamp;
            depth[start] = 0;
            while (head < tail)
            {
                int u = queue[head++];
                if (depth[u] >= cheat - 1) continue;
                for (int d = 0; d < 4; d++)
                {
                    int v = neighbor[u * 4 + d];
                    if (v < 0 || v == blocked || seen[v] == stamp) continue;
                    seen[v] = stamp; depth[v] = depth[u] + 1; queue[tail++] = v;
                }
            }
        }

        private int Cell(int row, int column) { return (row - 1) * columns + column - 1; }

        public int Answer(List<int> query)
        {
            int empty = Cell(query[0], query[1]);
            int start = Cell(query[2], query[3]);
            int target = Cell(query[4], query[5]);

            if (start == target) return 0;
            if (component[start] != component[target]) return -1;

            int[] distance = new int[size * 4];
            for (int i = 0; i < distance.Length; i++) distance[i] = int.MaxValue;

            var heap = new Heap();
            Bfs(empty, start);
            for (int d = 0; d < 4; d++)
            {
                int v = neighbor[start * 4 + d];
                if (v < 0) continue;
                int cost = seen[v] == stamp ? depth[v] : cheat;
                int state = start * 4 + d;
                distance[state] = cost;
                heap.Push(state, cost);
            }

            while (heap.Count > 0)
            {
                int state, cost;
                heap.Pop(out state, out cost);
                if (cost != distance[state]) continue;

                int u = state / 4, side = state % 4;
                if (u == target) return cost;

                for (int d = 0; d < 4; d++)
                {
                    int v = neighbor[u * 4 + d];
                    if (v < 0) continue;
                    int next = v * 4 + ((d + 2) % 4);
                    int newCost = cost + turn[u * 16 + side * 4 + d] + 1;
                    if (newCost < distance[next]) { distance[next] = newCost; heap.Push(next, newCost); }
                }
            }
            return -1;
        }
    }

    public static List<int> Huarongdao(int k, List<List<int>> board, List<List<int>> queries)
    {
        var solver = new Solver(k, board);
        var result = new List<int>(queries.Count);
        foreach (List<int> query in queries) result.Add(solver.Answer(query));
        return result;
    }
}

class Solution
{
    private class Reader
    {
        private readonly System.IO.Stream stream = Console.OpenStandardInput();
        private readonly byte[] buffer = new byte[65536];
        private int position, count;

        private int Read()
        {
            if (position == count)
            {
                count = stream.Read(buffer, 0, buffer.Length);
                position = 0;
                if (count == 0) return -1;
            }
            return buffer[position++];
        }

        public int Int()
        {
            int c;
            do { c = Read(); } while (c <= 32 && c >= 0);
            int value = 0;
            while (c > 32) { value = value * 10 + c - 48; c = Read(); }
            return value;
        }
    }

    public static void Main()
    {
        var reader = new Reader();
        int n = reader.Int(), m = reader.Int(), k = reader.Int(), q = reader.Int();

        var board = new List<List<int>>(n);
        for (int r = 0; r < n; r++)
        {
            var row = new List<int>(m);
            for (int c = 0; c < m; c++) row.Add(reader.Int());
            board.Add(row);
        }

        var queries = new List<List<int>>(q);
        for (int i = 0; i < q; i++)
        {
            var query = new List<int>(6);
            for (int j = 0; j < 6; j++) query.Add(reader.Int());
            queries.Add(query);
        }

        var output = new StringBuilder();
        foreach (int answer in Result.Huarongdao(k, board, queries)) output.Append(answer).Append('\n');
        Console.Write(output.ToString());
    }
}
