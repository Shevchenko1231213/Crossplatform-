namespace Algorithms;


public static class Calculator
{
    public static bool CheckIsValid(int[][] data)
    {
        const int
            NotVisited = 0,
            During = 1,
            Visited = 2;


        var n = data.GetLength(0);
        var state = new int[n][];
        var sentI = new Dictionary<int, int>();
        var sentJ = new Dictionary<int, int>();

        var computedUsed = new List<int>();
        var sentUsed = new List<int>();
        for (var i = 0; i < n; ++i)
        {
            var m = data[i].Length;
            state[i] = new int[m];

            if (m > 0)
            {
                computedUsed.Add(i);
            }
            for (var j = 0; j < m; ++j)
            {
                var value = data[i][j];
                if (value > 0)
                {
                    sentI[value] = i;
                    sentJ[value] = j;
                    sentUsed.Add(value);
                }
            }
        }

        var isValid = true;
        for (var i = 0; i < n; ++i)
        {
            for (var j = 0; j < data[i].Length; ++j)
            {
                if (state[i][j] == NotVisited)
                {
                    Visit(i, j);
                }
            }
        }
        return isValid;


        void Visit(int i, int j)
        {
            if (state[i][j] == Visited)
            {
                return;
            }
            if (state[i][j] == During)
            {
                isValid = false;
                return;
            }
            state[i][j] = During;
            if (j > 0)
            {
                Visit(i, j - 1);
            }
            if (data[i][j] < 0)
            {
                var value = -data[i][j];
                Visit(sentI[value], sentJ[value]);
            }
            state[i][j] = Visited;
        }
    }
}