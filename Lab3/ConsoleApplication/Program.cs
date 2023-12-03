using Algorithms;

namespace ConsoleApplication;


internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var lines = File.ReadAllLines("INPUT.TXT")
                .AsEnumerable()
                .GetEnumerator();
            var input = Read(lines);

            using var file = new StreamWriter("OUTPUT.TXT");
            foreach(var data in input)
            {
                var isValid = Calculator.CheckIsValid(data);
                file.WriteLine(isValid ? "YES" : "NO");
            }
        }
        catch (Exception exception)
        {
            File.WriteAllLines("OUTPUT.TXT", [exception.Message]);
        }
    }


    private static int[][][] Read(IEnumerator<string> lines)
    {
        if (!lines.MoveNext())
        {
            throw new InvalidOperationException(
                "The input does not contain a line of number blocks."
            );
        }
        if (!(
            int.TryParse(lines.Current, out var t)
            && t >= 0
        ))
        {
            throw new InvalidOperationException(
                "Invalid 't'."
            );
        }

        var data = new int[t][][];
        for (var k = 0; k < t; ++k)
        {
            data[k] = ReadBlock(lines, k);
        }
        return data;
    }


    private static int[][] ReadBlock(IEnumerator<string> lines, int index)
    {
        if (!lines.MoveNext())
        {
            throw new InvalidOperationException(
                $"Block {index + 1}. The input does not contain a line of size."
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
                "Invalid 'n' or 'm'."
            );
        }


        var data = new int[n][];
        for (int i = 0; i < n; ++i)
        {
            if (!lines.MoveNext())
            {
                throw new InvalidOperationException(
                    $"Block {index + 1}, row {i + 1}: the input does not contain line."
                );
            }
            var rowTexts = lines.Current.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (!(
                int.TryParse(rowTexts[0], out var size)
                && size >= 0 && size == rowTexts.Length - 1
            ))
            {
                throw new InvalidOperationException(
                    $"Block {index + 1}, row {i + 1}: invalid size."
                );
            }

            data[i] = new int[size];
            for (int j = 0; j < size; ++j)
            {
                if (!int.TryParse(rowTexts[j + 1], out var value))
                {
                    throw new InvalidOperationException(
                        $"Block {index + 1}, row {i + 1}, value {j + 1}: invalid value."
                    );
                }
                data[i][j] = value;
            }
        }

        return data;
    }
}