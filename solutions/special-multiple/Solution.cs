using System;
using System.Collections.Generic;
using System.Text;

// Special Multiple
// https://www.hackerrank.com/challenges/special-multiple
//
// The number can be far longer than a long, so it is never built as an integer.
// Instead, breadth-first search over the remainder modulo n: a node is a
// remainder, and appending a digit moves r to (10r + d) mod n. The first time
// remainder 0 is reached, the path is the shortest number that works.
//
// Trying the digit 0 before 9 makes that number the smallest among those of the
// same length, because breadth-first order already guarantees shortest length.

class Result
{
    private static readonly Dictionary<int, string> Cache = new Dictionary<int, string>();

    public static string Solve(int n)
    {
        string cached;
        if (Cache.TryGetValue(n, out cached)) return cached;

        var parent = new int[n];
        var digit = new char[n];
        for (int i = 0; i < n; i++) parent[i] = -2; // -2 means unvisited

        var queue = new int[n];
        int head = 0, tail = 0;

        int start = 9 % n;
        parent[start] = -1; // -1 marks the first digit
        digit[start] = '9';
        queue[tail++] = start;

        while (parent[0] == -2 && head < tail)
        {
            int remainder = queue[head++];
            for (int d = 0; d <= 9; d += 9) // 0 first, then 9
            {
                int next = (remainder * 10 + d) % n;
                if (parent[next] != -2) continue;
                parent[next] = remainder;
                digit[next] = (char)('0' + d);
                queue[tail++] = next;
            }
        }

        var answer = new StringBuilder();
        for (int r = 0; r != -1; r = parent[r]) answer.Append(digit[r]);

        var chars = answer.ToString().ToCharArray();
        Array.Reverse(chars);
        string result = new string(chars);
        Cache[n] = result;
        return result;
    }
}

class Solution
{
    public static void Main()
    {
        string first = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(first)) return;
        int tests = int.Parse(first.Trim());

        var output = new StringBuilder();
        for (int i = 0; i < tests; i++)
        {
            string line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) { i--; continue; }
            output.Append(Result.Solve(int.Parse(line.Trim()))).Append('\n');
        }
        Console.Out.Write(output.ToString());
        Console.Out.Flush();
    }
}
