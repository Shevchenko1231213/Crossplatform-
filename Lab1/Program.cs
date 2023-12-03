namespace Lab1;


internal static class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var lines = File.ReadAllLines("INPUT.TXT")
                .AsEnumerable()
                .GetEnumerator();
            var input = Read(lines);
            var numberPermutations = CountPermutations(input.N, input.K);
            File.WriteAllLines("OUTPUT.TXT", [numberPermutations.ToString()]);
        }
        catch (Exception exception)
        {
            File.WriteAllLines("OUTPUT.TXT", [exception.Message]);
        }
    }


    private static UserInput Read(IEnumerator<string> lines)
    {
        if (!lines.MoveNext())
        {
            throw new InvalidOperationException(
                "Input does not contain lines with 'n' and 'k'."
            );
        }
        if (!(
            lines.Current.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            is [var nText, var kText]
            && int.TryParse(nText, out var n)
            && n >= 1 && n <= 9
            && int.TryParse(kText, out var k)
            && k >= 0 && k <= n
        ))
        {
            throw new InvalidOperationException(
                "Invalid 'n' or 'k'."
            );
        }

        return new UserInput
        {
            N = n,
            K = k
        };
    }


    private static long CalculateFactorial(int n)
    {
        var factorial = 1L;
        for (var multiplier = 2; multiplier <= n; ++multiplier)
            factorial *= multiplier;
        return factorial;
    }


    private static long CountPermutations(int n, int k)
    {
        var sum = 0L;
        var multiplier = CalculateFactorial(n) / CalculateFactorial(k);
        var term = 1;
        for (var i = 0; i <= n - k; ++i)
        {
            sum += term * multiplier / CalculateFactorial(i);
            term *= -1;
        }
        return sum;
    }
}