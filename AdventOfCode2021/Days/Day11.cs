namespace AdventOfCode2021.Days;
internal class Day11 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines().Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
        var count = 0;
        var sync = 0;
        
        for (int steps = 0; steps < 100; steps++)
        {
            Increase(ref lines);
            count += Flash(ref lines);
            Zero(ref lines);
            if (sync == 0 && CheckSync(lines))
                sync = steps + 1;
        }
        
        Result(count, 1);

        var index = 100;
        while (sync == 0)
        {
            index++;
            Increase(ref lines);
            Flash(ref lines);
            Zero(ref lines);
            if (sync == 0 && CheckSync(lines))
                sync = index;
        }

        Result(sync, 2);
    }

    private static bool CheckSync(int[][] lines) => lines.SelectMany(x => x).All(n => n == 0);

    private static void Increase(ref int[][] grid)
    {
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                grid[y][x]++;
            }
        }
    }

    private static int Flash(ref int[][] grid)
    {
        bool[] flashed = new bool[grid.Length * grid[0].Length];
        var found = false;
        do
        {
            found = false;
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (grid[y][x] > 9 && !flashed[y * grid.Length + x])
                    {
                        IncreaseNeighbours(ref grid, y, x);
                        flashed[y * grid.Length + x] = true;
                        found = true;
                    }
                }
            }
        } while (found);
        return flashed.Where(x => x).Count();
    }

    private static void IncreaseNeighbours(ref int[][] grid, int y, int x)
    {
        var right = grid[y].Length - 1;
        var bottom = grid.Length - 1;

        if (y != 0 && x != 0) grid[y - 1][x - 1]++;
        if (y != 0) grid[y - 1][x]++;
        if (y != 0 && x != right) grid[y - 1][x + 1]++;
        
        
        if (x != 0) grid[y][x - 1]++;
        if (x != right) grid[y][x + 1]++;
        
        if (y != bottom && x != 0) grid[y + 1][x - 1]++;
        if (y != bottom) grid[y + 1][x]++;
        if (y != bottom && x != right) grid[y + 1][x + 1]++;
    }

    private static void Zero(ref int[][] grid)
    {
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] > 9) grid[y][x] = 0;
            }
        }
    }
}