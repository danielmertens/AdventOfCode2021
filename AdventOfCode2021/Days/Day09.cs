namespace AdventOfCode2021.Days;
internal class Day09 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        var map = lines.Select(x => 
                        x.ToCharArray()
                        .Select(i => int.Parse(i.ToString()))
                        .Select(n => new Node { Depth = n})
                        .ToArray()
                    ).ToArray();
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                var neighbours = new List<Node>();
                if (y != 0)
                    neighbours.Add(map[y - 1][x]);
                if (x != 0)
                    neighbours.Add(map[y][x-1]);
                if (y != map.Length - 1)
                    neighbours.Add(map[y + 1][x]);
                if (x != map[y].Length - 1)
                    neighbours.Add(map[y][x + 1]);

                map[y][x].Neighbours = neighbours;

                if (map[y][x].Depth < neighbours.Select(x => x.Depth).Min())
                {
                    map[y][x].IsLowPoint = true;
                }
            }
        }

        var count = map.SelectMany(n => n)
            .Where(x => x.IsLowPoint)
            .Select(x => x.Depth + 1)
            .Sum();

        Result(count);

        // PART 2
        count = map.SelectMany(x => x).Where(n => n.IsLowPoint)
            .Select(n => CalculateBasin(n))
            .OrderByDescending(n => n)
            .Take(3)
            .Aggregate((s, n) => s * n);

        Result(count);

    }

    private int CalculateBasin(Node start)
    {
        var visited = new List<Node>() { start };
        var activeNodes = new List<Node>() { start };

        while(activeNodes.Count > 0)
        {
            var toAdd = activeNodes[0].Neighbours
                .Where(n => !visited.Contains(n) && n.Depth != 9);
            activeNodes.AddRange(toAdd);
            visited.AddRange(toAdd);
            activeNodes.RemoveAt(0);
        }

        return visited.Count();
    }

    private class Node
    {
        public int Depth { get; set; }
        public List<Node> Neighbours { get; set; } = new List<Node>();
        public bool IsLowPoint { get; set; }
    }
}
