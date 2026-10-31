using System;
using System.Collections.Generic;

// Letter Islands
// https://www.hackerrank.com/challenges/letter-islands
//
// A suffix automaton groups the distinct substrings that share the same set of
// occurrence positions. For one such group with P occurrences, the islands a
// substring of length L produces is
//
//     1 + (number of consecutive gaps between occurrences that are > L)
//
// so "exactly K islands" is a range of lengths, and the group contributes the
// size of that range. Gaps are kept in an order-statistic tree so the K-th gap
// is available while merging occurrence sets up the suffix links.
//
// Status: passes the sample. Not cross-checked by brute force, and not measured
// against the judge's time limit.

class Result
{
    private class Node
    {
        public int Key, Count = 1, Size = 1;
        public uint Priority;
        public Node Left, Right;
        public Node(int key, uint priority) { Key = key; Priority = priority; }
    }

    private class Bag
    {
        public Node Positions, Gaps;
    }

    private class Solver
    {
        private uint random = 2463534242u;

        private uint NextRandom()
        {
            random ^= random << 13;
            random ^= random >> 17;
            random ^= random << 5;
            return random;
        }

        private static int Size(Node t) { return t == null ? 0 : t.Size; }

        private static void Update(Node t) { t.Size = t.Count + Size(t.Left) + Size(t.Right); }

        private static Node RotateRight(Node t)
        {
            Node a = t.Left;
            t.Left = a.Right; a.Right = t;
            Update(t); Update(a); return a;
        }

        private static Node RotateLeft(Node t)
        {
            Node a = t.Right;
            t.Right = a.Left; a.Left = t;
            Update(t); Update(a); return a;
        }

        private Node Insert(Node t, int key)
        {
            if (t == null) return new Node(key, NextRandom());
            if (key == t.Key) t.Count++;
            else if (key < t.Key)
            {
                t.Left = Insert(t.Left, key);
                if (t.Left.Priority > t.Priority) t = RotateRight(t);
            }
            else
            {
                t.Right = Insert(t.Right, key);
                if (t.Right.Priority > t.Priority) t = RotateLeft(t);
            }
            Update(t);
            return t;
        }

        private static Node Join(Node a, Node b)
        {
            if (a == null) return b;
            if (b == null) return a;
            if (a.Priority > b.Priority)
            {
                a.Right = Join(a.Right, b); Update(a); return a;
            }
            b.Left = Join(a, b.Left); Update(b); return b;
        }

        private static Node Remove(Node t, int key)
        {
            if (key < t.Key) t.Left = Remove(t.Left, key);
            else if (key > t.Key) t.Right = Remove(t.Right, key);
            else
            {
                if (t.Count == 1) return Join(t.Left, t.Right);
                t.Count--;
            }
            Update(t);
            return t;
        }

        private static int Kth(Node t, int rank)
        {
            while (true)
            {
                int left = Size(t.Left);
                if (rank <= left) t = t.Left;
                else if (rank <= left + t.Count) return t.Key;
                else { rank -= left + t.Count; t = t.Right; }
            }
        }

        private void AddPosition(Bag bag, int position)
        {
            int previous = -1, next = -1;
            Node t = bag.Positions;
            while (t != null)
            {
                if (position < t.Key) { next = t.Key; t = t.Left; }
                else if (position > t.Key) { previous = t.Key; t = t.Right; }
                else return;
            }
            if (previous != -1 && next != -1) bag.Gaps = Remove(bag.Gaps, next - previous);
            if (previous != -1) bag.Gaps = Insert(bag.Gaps, position - previous);
            if (next != -1) bag.Gaps = Insert(bag.Gaps, next - position);
            bag.Positions = Insert(bag.Positions, position);
        }

        private Bag Merge(Bag a, Bag b)
        {
            if (a == null) return b;
            if (b == null) return a;
            if (Size(a.Positions) < Size(b.Positions)) { Bag temp = a; a = b; b = temp; }

            var stack = new Stack<Node>();
            if (b.Positions != null) stack.Push(b.Positions);
            while (stack.Count > 0)
            {
                Node t = stack.Pop();
                AddPosition(a, t.Key);
                if (t.Left != null) stack.Push(t.Left);
                if (t.Right != null) stack.Push(t.Right);
            }
            return a;
        }

        public long Run(string s, int k)
        {
            int n = s.Length;
            if (n == 0 || k < 1 || k > n) return 0;

            int capacity = 2 * n;
            int[,] transition = new int[capacity, 26];
            int[] length = new int[capacity];
            int[] link = new int[capacity];
            Bag[] bags = new Bag[capacity];
            link[0] = -1;

            int states = 1, last = 0;
            for (int i = 0; i < n; i++)
            {
                int c = s[i] - 'a';
                int current = states++;
                length[current] = length[last] + 1;
                bags[current] = new Bag();
                AddPosition(bags[current], i);

                int p = last;
                while (p != -1 && transition[p, c] == 0) { transition[p, c] = current; p = link[p]; }

                if (p == -1) link[current] = 0;
                else
                {
                    int q = transition[p, c];
                    if (length[p] + 1 == length[q]) link[current] = q;
                    else
                    {
                        int clone = states++;
                        length[clone] = length[p] + 1;
                        link[clone] = link[q];
                        for (int d = 0; d < 26; d++) transition[clone, d] = transition[q, d];
                        while (p != -1 && transition[p, c] == q) { transition[p, c] = clone; p = link[p]; }
                        link[q] = link[current] = clone;
                    }
                }
                last = current;
            }

            int[] counts = new int[n + 1];
            int[] order = new int[states];
            for (int v = 0; v < states; v++) counts[length[v]]++;
            for (int i = 1; i <= n; i++) counts[i] += counts[i - 1];
            for (int v = states - 1; v >= 0; v--) order[--counts[length[v]]] = v;

            long answer = 0;
            for (int index = states - 1; index > 0; index--)
            {
                int v = order[index];
                Bag bag = bags[v];
                int occurrences = Size(bag.Positions);

                if (occurrences >= k)
                {
                    int low = length[link[v]] + 1;
                    int high = length[v];
                    if (k < occurrences) low = Math.Max(low, Kth(bag.Gaps, occurrences - k));
                    if (k > 1) high = Math.Min(high, Kth(bag.Gaps, occurrences - k + 1) - 1);
                    if (low <= high) answer += (long)high - low + 1;
                }

                int parent = link[v];
                bags[parent] = Merge(bags[parent], bag);
                bags[v] = null;
            }
            return answer;
        }
    }

    public static long LetterIslands(string s, int k)
    {
        return new Solver().Run(s, k);
    }
}

class Solution
{
    public static void Main()
    {
        string s = Console.ReadLine();
        string kLine = Console.ReadLine();
        if (s == null || kLine == null) return;
        Console.Out.WriteLine(Result.LetterIslands(s.Trim(), int.Parse(kLine.Trim())));
        Console.Out.Flush();
    }
}
