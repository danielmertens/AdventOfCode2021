namespace AdventOfCode2021.Days;
internal class Day12 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        List<Node> nodes = new List<Node>();
        foreach (var line in lines)
        {
            var split = line.Split('-');
            var node1 = nodes.FirstOrDefault(n => n.Name == split[0]) ?? new Node(split[0]);
            var node2 = nodes.FirstOrDefault(n => n.Name == split[1]) ?? new Node(split[1]);
            nodes.Add(node1);
            nodes.Add(node2);
            node1.AddNeighbour(node2);
        }

        var start = nodes.First(n => n.Name == "start");
        var end = nodes.First(n => n.Name == "end");

        var paths = new List<List<Node>>() { new List<Node>() { start } };
        var count = Loop(start, end,
            (path, candidate) => 
                (candidate.IsSmall && !IsVisited(path, candidate)) // Small cave that has not been visited yet
                    || !candidate.IsSmall); // Large cave

        Result(count, 1);

        paths = new List<List<Node>>() { new List<Node>() { start } };
        count = Loop(start, end,
            (path, candidate) => 
                (candidate.IsSmall && !IsVisited(path, candidate)) // Small cave that has not been visited yet
                    || !candidate.IsSmall // Large cave
                    || (candidate.IsSmall && !HasVisitedSmallTwice(path) && candidate != start)); // If no small cave has been visited twice yet

        Result(count, 2);
    }

    private int Loop(Node start, Node end, Func<List<Node>, Node, bool> canVisit)
    {
        var paths = new List<List<Node>>() { new List<Node>() { start } };
        var count = 0;

        while (paths.Count > 0)
        {
            var newPaths = new List<List<Node>>();
            foreach (var path in paths)
            {
                if (path.Last() == end)
                {
                    count++;
                    continue;
                }
                foreach (var neigh in path.Last().Neighbours)
                {
                    if (canVisit(path, neigh))
                    {
                        var p = new List<Node>(path);
                        p.Add(neigh);
                        newPaths.Add(p);
                    }
                }
            }

            paths = newPaths;
        }

        return count;
    }

    private bool HasVisitedSmallTwice(List<Node> path)
    {
        return path.Where(n => n.IsSmall)
            .GroupBy(n => n)
            .Where(g => g.Count() > 1)
            .Count() > 0;
    }

    private bool IsVisited(List<Node> path, Node neigh)
    {
        return path.FirstOrDefault(n => n == neigh) != null;
    }

    private class Node
    {
        public Node(string name)
        {
            Name = name;
            if (name[0] >= 'a' && name[0] <= 'z')
            {
                IsSmall = true;
            }
        }

        public string Name { get; set; }
        public bool IsSmall { get; set; }
        public List<Node> Neighbours { get; set; } = new List<Node>();

        public void AddNeighbour(Node neighbour)
        {
            if (Neighbours.Contains(neighbour))
            {
                return;
            }
            Neighbours.Add(neighbour);
            neighbour.AddNeighbour(this);
        }
    }
}
