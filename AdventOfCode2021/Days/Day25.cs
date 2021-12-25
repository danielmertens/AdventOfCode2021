namespace AdventOfCode2021.Days;
internal class Day25 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();
        var grid = new int[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            grid[i] = new int[lines[i].Length];
            for (int j = 0; j < lines[i].Length; j++)
            {
                switch (lines[i][j])
                {
                    case '.': grid[i][j] = 0; break;
                    case '>': grid[i][j] = 1; break;
                    case 'v': grid[i][j] = 2; break;
                }
            }
        }

        var result = Run(grid);
        Result(result);
    }

    private int Run(int[][] grid)
    {
        var steps = 0;
        bool running;
        var movedList = new HashSet<int>();

        do
        {
            running = false;
            movedList.Clear();

            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    var moved = MoveEast(ref grid, y, x, movedList);
                    running = running || moved;
                }
            }

            movedList.Clear();

            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    var moved = MoveSouth(ref grid, y, x, movedList);
                    running = running || moved;
                }
            }

            steps++;
        } while (running);

        return steps;
    }

    private bool MoveEast(ref int[][] grid, int y, int x, HashSet<int> movedList)
    {
        if (grid[y][x] == 1 && !movedList.Contains((y * grid[y].Length) + x))
        {
            if (x + 1 == grid[y].Length && grid[y][0] == 0 && movedList.Contains((y * grid[y].Length) + 1))
            {
                // creature on 0 already moved.
                return false;
            }

            var nextX = x + 1 == grid[y].Length ? 0 : x + 1;
            if (grid[y][nextX] == 0)
            {
                grid[y][nextX] = 1;
                grid[y][x] = 0;
                movedList.Add((y * grid[y].Length) + nextX);
                return true;
            }
        }

        return false;
    }

    private bool MoveSouth(ref int[][] grid, int y, int x, HashSet<int> movedList)
    {
        if (grid[y][x] == 2 && !movedList.Contains((y * grid[y].Length) + x))
        {
            if (y + 1 == grid.Length && grid[0][x] == 0 && movedList.Contains((1 * grid[y].Length) + x))
            {
                return false;
            }

            var nextY = y + 1 == grid.Length ? 0 : y + 1;
            if (grid[nextY][x] == 0)
            {
                grid[nextY][x] = 2;
                grid[y][x] = 0;
                movedList.Add((nextY * grid[y].Length) + x);
                return true;
            }
        }

        return false;
    }
}
