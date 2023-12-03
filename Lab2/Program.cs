namespace Lab2;


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
            var (rabbitResult, hamsterResult) = Play(input);
            File.WriteAllLines("OUTPUT.TXT", [$"{rabbitResult} {hamsterResult}"]);
        }
        catch (Exception exception)
        {
            File.WriteAllLines("OUTPUT.TXT", [exception.Message]);
        }
    }


    private static int[,] Read(IEnumerator<string> lines)
    {
        if (!lines.MoveNext())
        {
            throw new InvalidOperationException(
                "The input does not contain a line of size."
            );
        }
        if (!(
            lines.Current.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            is [var nText, var mText]
            && int.TryParse(nText, out var n)
            && n >= 1
            && int.TryParse(mText, out var m)
            && m >= 1
        ))
        {
            throw new InvalidOperationException(
                "Invalid 'N' or 'M'."
            );
        }

        var matrix = new int[n, m];
        for (int i = 0; i < n; ++i)
        {
            if (!lines.MoveNext())
            {
                throw new InvalidOperationException(
                    $"Row {i + 1}: the input does not contain line."
                );
            }
            var rowTexts = lines.Current.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (rowTexts.Length != m)
            {
                throw new InvalidOperationException(
                    $"Row {i + 1}: expected {m} values, but received {rowTexts.Length}."
                );
            }

            for (int j = 0; j < m; ++j)
            {
                if (!(
                    int.TryParse(rowTexts[j], out var value)
                    && value >= 0
                ))
                {
                    throw new InvalidOperationException(
                        $"Row {i + 1}, column {j + 1}: invalid value."
                    );
                }
                matrix[i, j] = value;
            }
        }

        return matrix;
    }


    private static (int RabbitResult, int HamsterResult) Play(int[,] matrix)
    {
        var n = matrix.GetLength(0);
        var m = matrix.GetLength(1);

        var rabbitResult = 0;
        var hamsterResult = 0;
        var needContinue = true;
        while (needContinue)
        {
            var rabbitCurrent = GoRabbit();
            var hamsterCurrent = GoHamster();
            rabbitResult += rabbitCurrent;
            hamsterResult += hamsterCurrent;
            needContinue = hamsterCurrent > 0;
        }
        return (rabbitResult, hamsterResult);


        int GoRabbit()
        {
            var result = 0;
            var previousJ = 0;
            for (var i = 0; i < n; ++i)
            {
                var maxJ = previousJ;
                for (var j = 0; j < m; ++j)
                {
                    if (
                        matrix[i, j] >= matrix[i, maxJ]
                        && (i == 0 || Math.Abs(previousJ - j) <= 1)
                    )
                    {
                        maxJ = j;
                    }
                }

                result += matrix[i, maxJ];
                matrix[i, maxJ] = 0;
                previousJ = maxJ;
            }
            return result;
        }


        int GoHamster()
        {
            var map = new int[n, m];
            for (var j = 0; j < m; ++j)
            {
                map[n - 1, j] = matrix[n - 1, j];
            }

            for (var i = n - 2; i >= 0; --i)
            {
                for (var j = 0; j < m; ++j)
                {
                    map[i, j] = map[i + 1, j];
                    map[i, j] = Math.Max(
                        map[i, j],
                        j != 0 ? map[i + 1, j - 1] : 0
                    );
                    map[i, j] = Math.Max(
                        map[i, j],
                        j < m - 1 ? map[i + 1, j + 1] : 0
                    );
                    map[i, j] += matrix[i, j];
                }
            }

            var maxJ = 0;
            for (var j = 0; j < m; ++j)
            {
                if (map[0, maxJ] <= map[0, j])
                {
                    maxJ = j;
                }
            }

            var result = map[0, maxJ];
            for (var i = 1; i < n; ++i)
            {
                var currentJ = -1;
                for (int j = Math.Max(0, maxJ - 1); j < m && j < maxJ + 2; ++j)
                {
                    if (map[i - 1, maxJ] == map[i, j] + matrix[i - 1, maxJ])
                    {
                        currentJ = j;
                    }
                }

                matrix[i - 1, maxJ] = 0;
                maxJ = currentJ;
            }
            matrix[n - 1, maxJ] = 0;
            return result;
        }
    }
}