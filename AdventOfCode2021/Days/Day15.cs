namespace AdventOfCode2021.Days;
internal class Day15 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        var map = lines.Select(x =>
                        x.ToCharArray()
                        .Select(i => int.Parse(i.ToString()))
                        .Select(n => new Node { Risk = n })
                        .ToArray()
                    ).ToArray();

        var largeMap = new Node[map.Length * 5][];
        var width = map[0].Length;
        var height = map.Length;
        for (int y = 0; y < map.Length; y++)
        {
            largeMap[y] = new Node[width * 5];
            for (int x = 0; x < width * 5; x++)
            {
                if (x/width == 0)
                {
                    largeMap[y][x] = new Node { Risk = map[y][x].Risk };
                }
                else 
                {
                    var t = largeMap[y][x - width].Risk + 1;
                    if (t > 9) t = 1;
                    largeMap[y][x] = new Node { Risk =  t};
                }
            }
        }

        for (int y = map.Length; y < largeMap.Length; y++)
        {
            largeMap[y] = new Node[width * 5];
            for (int x = 0; x < width * 5; x++)
            {
                var t = largeMap[y - height][x].Risk + 1;
                if (t > 9) t = 1;
                largeMap[y][x] = new Node { Risk = t };
            }
        }

        map = largeMap;

        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                var neighbours = new List<Node>();
                if (y != 0)
                    neighbours.Add(map[y - 1][x]);
                if (x != 0)
                    neighbours.Add(map[y][x - 1]);
                if (y != map.Length - 1)
                    neighbours.Add(map[y + 1][x]);
                if (x != map[y].Length - 1)
                    neighbours.Add(map[y][x + 1]);

                neighbours.Sort((n1,n2) => n1.Risk - n2.Risk);
                map[y][x].Nodes = neighbours;
            }
        }

        var start = map[0][0];
        var end = map[map.Length - 1][map[0].Length - 1];
        start.RiskToHere = start.Risk;
        var list = new SortedSet<Node>() { start };

        while (list.First() != end)
        {
            var node = list.First();
            list.Remove(node);
            node.Visited = true;
            foreach (var neigh in node.Nodes)
            {
                if(!neigh.Visited)
                {
                    neigh.Previous = node;
                    neigh.RiskToHere = GetCostToHere(neigh, start);
                    neigh.Visited = true;
                    list.Add(neigh);
                }
                else if (neigh.RiskToHere > GetCostToHere(node, start) + neigh.Risk)
                {
                    neigh.Previous = node;
                    neigh.RiskToHere = GetCostToHere(neigh, start);
                    list.Add(neigh);
                }
            }

            //list.Sort((n1, n2) => n1.RiskToHere - n2.RiskToHere);
        }

        Result(end.RiskToHere);
    }

    private static int GetCostToHere(Node node, Node start)
    {
        var count = 0;
        var temp = node;
        while (temp != start)
        {
            count += temp.Risk;
            temp = temp.Previous;
        }
        return count;
    }
    private class Node : IComparable<Node>
    {
        public int Risk { get; set; }
        public List<Node> Nodes { get; set; } = new List<Node>();
        public Node Previous { get; set; }
        public int RiskToHere { get; set; }
        public bool Visited { get; set; }

        public int Compare(Node? x, Node? y)
        {
            return x?.RiskToHere.CompareTo(y?.RiskToHere ?? int.MaxValue) ?? throw new ArgumentException(nameof(x), nameof(y));
        }

        public int CompareTo(Node? other)
        {
            return RiskToHere.CompareTo(other.RiskToHere);
        }
    }
}
