namespace AdventOfCode2021.Days;
internal class Day05 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var text = GetInputLines();

        var lines = ParseLines(text);
        
        // Decide the size of the grid.
        int maxX = 0;
        int maxY = 0;
        foreach (var item in lines)
        {
            if (item.Point1.X > maxX) maxX = item.Point1.X;
            if (item.Point2.X > maxX) maxX = item.Point2.X;
            if (item.Point1.Y > maxY) maxY = item.Point1.Y;
            if (item.Point2.Y > maxY) maxY = item.Point2.Y;
        }
        var arr = new int[maxY + 1, maxX + 1];

        foreach (var item in lines)
        {
            // Horizontal
            if (item.Point1.X == item.Point2.X)
            {
                var smallest = Math.Min(item.Point1.Y, item.Point2.Y);
                var largest = Math.Max(item.Point1.Y, item.Point2.Y);
                for (int i = smallest; i <= largest; i++)
                {
                    arr[i, item.Point1.X]++;
                }
            }
            // Vertical
            else if (item.Point1.Y == item.Point2.Y)
            {
                var smallest = Math.Min(item.Point1.X, item.Point2.X);
                var largest = Math.Max(item.Point1.X, item.Point2.X);
                for (int i = smallest; i <= largest; i++)
                {
                    arr[item.Point1.Y, i]++;
                }
            }
            // Diagonal
            else
            {
                Coord left;
                Coord right;
                if (item.Point1.X < item.Point2.X)
                {
                    left = item.Point1;
                    right = item.Point2;    
                }
                else
                {
                    left = item.Point2;
                    right = item.Point1;
                }
                int increment;
                if (right.Y > left.Y) increment = 1;
                else increment = -1;

                for (int i = 0; i <= right.X - left.X; i++)
                {
                    arr[left.Y + i * increment, left.X + i]++;
                }
            }
        }

        var count = 0;
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                if (arr[i, j] > 1) count++;
            }
        }

        Result(count);
    }

    private Line[] ParseLines(string[] text)
    {
        var lines = new Line[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            var firstSplit = text[i].Split(" -> ");
            lines[i] = new Line
            {
                Point1 = new Coord
                {
                    X = int.Parse(firstSplit[0].Split(',')[0]),
                    Y = int.Parse(firstSplit[0].Split(',')[1])
                },
                Point2 = new Coord
                {
                    X = int.Parse(firstSplit[1].Split(',')[0]),
                    Y = int.Parse(firstSplit[1].Split(',')[1])
                }
            };
        }
        return lines;
    }

    public struct Line
    {
        public Coord Point1;
        public Coord Point2;
    }

    public struct Coord
    {
        public int X;
        public int Y;
    }
}
