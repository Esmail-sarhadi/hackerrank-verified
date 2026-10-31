using System;
using System.Text;

// Even Odd Query
// https://www.hackerrank.com/challenges/even-odd-query
//
// find(x, y) returns A[x] raised to find(x+1, y), and 1 when x > y.
//
// The result is odd in exactly two cases:
//   1. A[x] is odd, because an odd base stays odd for any exponent.
//   2. A[x] is even and the exponent is 0, because a^0 = 1.
//
// The exponent is 0 only when A[x+1] is 0, which the constraints allow at most
// once in a row - so the second case is "x < y and A[x+1] == 0". No power is
// ever computed, and each query is O(1).

class Result
{
    public static string Query(int[] values, int x, int y)
    {
        bool odd = values[x] % 2 != 0 || (x < y && values[x + 1] == 0);
        return odd ? "Odd" : "Even";
    }
}

class Solution
{
    public static void Main()
    {
        string first = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(first)) return;
        int n = int.Parse(first.Trim());

        string arrayLine = Console.ReadLine();
        var parts = arrayLine.Trim().Split(' ');
        var values = new int[n + 2];
        for (int i = 1; i <= n; i++) values[i] = int.Parse(parts[i - 1]);

        int queries = int.Parse(Console.ReadLine().Trim());
        var output = new StringBuilder();
        for (int i = 0; i < queries; i++)
        {
            string[] pair = Console.ReadLine().Trim().Split(' ');
            int x = int.Parse(pair[0]);
            int y = int.Parse(pair[1]);
            output.Append(Result.Query(values, x, y)).Append('\n');
        }
        Console.Out.Write(output.ToString());
        Console.Out.Flush();
    }
}
