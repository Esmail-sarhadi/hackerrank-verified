using System;
using System.IO;

// Sherlock and Permutations
// https://www.hackerrank.com/challenges/sherlock-and-permutations
//
// Fix the first character as 1. What is left is a string of n zeros and m-1
// ones, and every arrangement of those is a distinct permutation, so the answer
// is the binomial coefficient C(n + m - 1, n).
//
// n and m are at most 1000, so n + m - 1 is at most 1999 and the factorial
// table is tiny. The modulus is prime, so the inverse factorial comes from
// Fermat's little theorem instead of a division.

class Result
{
    private const long Mod = 1000000007L;
    private const int MaxN = 2000;

    private static long[] factorial;
    private static long[] inverseFactorial;

    private static long Power(long value, long exponent)
    {
        long result = 1;
        value %= Mod;
        while (exponent > 0)
        {
            if ((exponent & 1) == 1) result = result * value % Mod;
            value = value * value % Mod;
            exponent >>= 1;
        }
        return result;
    }

    private static void Prepare()
    {
        if (factorial != null) return;
        factorial = new long[MaxN + 1];
        inverseFactorial = new long[MaxN + 1];
        factorial[0] = 1;
        for (int i = 1; i <= MaxN; i++) factorial[i] = factorial[i - 1] * i % Mod;
        inverseFactorial[MaxN] = Power(factorial[MaxN], Mod - 2);
        for (int i = MaxN; i > 0; i--) inverseFactorial[i - 1] = inverseFactorial[i] * i % Mod;
    }

    private static long Binomial(int n, int k)
    {
        if (k < 0 || k > n) return 0;
        return factorial[n] * inverseFactorial[k] % Mod * inverseFactorial[n - k] % Mod;
    }

    public static int Solve(int n, int m)
    {
        Prepare();
        return (int)Binomial(n + m - 1, n);
    }
}

class Solution
{
    public static void Main()
    {
        PrepareOutput();
        string first = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(first)) return;
        int tests = int.Parse(first.Trim());

        var output = new System.Text.StringBuilder();
        for (int i = 0; i < tests; i++)
        {
            string line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) { i--; continue; }
            string[] parts = line.Trim().Split(' ');
            int n = int.Parse(parts[0]);
            int m = int.Parse(parts[1]);
            output.Append(Result.Solve(n, m)).Append('\n');
        }
        Console.Out.Write(output.ToString());
        Console.Out.Flush();
    }

    private static void PrepareOutput()
    {
        var stream = new StreamWriter(Console.OpenStandardOutput());
        stream.AutoFlush = false;
        Console.SetOut(stream);
    }
}
