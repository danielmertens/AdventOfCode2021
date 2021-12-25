namespace AdventOfCode2021.Days;
internal class Day13 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        var points = new List<Point>();

        var index = 0;
        while (lines[index]!= String.Empty)
        {
            var split = lines[index].Split(',').Select(x => int.Parse(x)).ToArray();
            points.Add(new Point() { X = split[0], Y = split[1] });
            index++;
        }

        var folds = new List<Point>();
        while(++index < lines.Length)
        {
            var fold = lines[index].Split(' ').Skip(2).First().Split('=').ToArray();
            if (fold[0] == "x") folds.Add(new Point() { X = int.Parse(fold[1]) });
            else folds.Add(new Point() { Y = int.Parse(fold[1]) });
        }

        // PART 1
        Folds(points, folds[0]);
        Result(points.Distinct().Count(), 1);

        // PART 2
        for (int i = 1; i < folds.Count; i++)
        {
            Folds(points, folds[i]);
        }

        Print(points);
    }

    private void Print(List<Point> points)
    {
        var maxX = points.Select(x => x.X).Max();
        var maxY = points.Select(x => x.Y).Max();

        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                if (points.Any(p => p.X == x && p.Y == y)) Console.Write("#");
                else Console.Write(".");
            }
            Console.WriteLine();
        }
    }

    private void Folds(List<Point> points, Point fold)
    {
        foreach (var point in points)
        {
            if (fold.Y > 0 && point.Y > fold.Y)
            {
                var diff = point.Y - fold.Y;
                point.Y = fold.Y - diff;
            }

            if (fold.X > 0 && point.X > fold.X)
            {
                var diff = point.X - fold.X;
                point.X = fold.X - diff;
            }
        }
    }

    private class Point: IEquatable<Point>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Equals(Point? other) => other != null && X == other.X && Y == other.Y;
        public override int GetHashCode() => X * 3 + Y * 11;
    }
}
