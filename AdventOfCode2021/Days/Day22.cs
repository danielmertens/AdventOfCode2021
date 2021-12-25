namespace AdventOfCode2021.Days;
internal class Day22 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        var cubes = new List<Cube>();
        foreach (var line in lines)
        {
            var coordPart = line.Split(' ')[1];
            var split = coordPart.Split(',');
            var xPart = split[0].Substring(2).Split("..").Select(x => int.Parse(x));
            var yPart = split[1].Substring(2).Split("..").Select(x => int.Parse(x));
            var zPart = split[2].Substring(2).Split("..").Select(x => int.Parse(x));

            // NOTE: Increase all values by 1 million to eliminate any possible negative x,y,z values.
            // We are only gonna look at the volumes so this increase doesn't change anything.
            // And since we are looking at volumes the end coordinates need to be increased by 1.
            cubes.Add(new Cube()
            {
                TurnOn = line.StartsWith("on"),
                XStart = xPart.First() + 1_000_000L,
                XEnd = xPart.Last() + 1_000_000L + 1,
                YStart = yPart.First() + 1_000_000L,
                YEnd = yPart.Last() + 1_000_000L + 1,
                ZStart = zPart.First() + 1_000_000L,
                ZEnd = zPart.Last() + 1_000_000L + 1,
            });
        }

        Result(CalculateVolume(cubes.ToList()));
    }

    private double CalculateVolume(List<Cube> cubes)
    {
        var temp = new List<Cube>();

        for (int i = 0; i < cubes.Count; i++)
        {
            var cube = cubes[i];
            var intersections = new List<Cube>();
            for (int j = 0; j < temp.Count; j++)
            {
                var other = temp[j];
                if (cube.IsIntersecting(other))
                {
                    var inter = other.Intersection(cube);
                    intersections.Add(inter);
                }
            }
            intersections.ForEach(inter => inter.TurnOn = !inter.TurnOn);
            temp.AddRange(intersections);

            if (cube.TurnOn) temp.Add(cube);
        }
        return temp.Sum(c => c.Volume * (c.TurnOn ? 1 : -1));
    }

    private class Cube
    {
        public bool TurnOn { get; set; }
        public long XStart { get; set; }
        public long XEnd { get; set; }
        public long YStart { get; set; }
        public long YEnd { get; set; }
        public long ZStart { get; set; }
        public long ZEnd { get; set; }

        public bool IsIntersecting(Cube other)
        {
            return GetOverlapVolume(other) != 0;
        }

        public double GetOverlapVolume(Cube other)
            => Math.Max(0, Math.Min(XEnd, other.XEnd) - Math.Max(XStart, other.XStart))
             * Math.Max(0, Math.Min(YEnd, other.YEnd) - Math.Max(YStart, other.YStart))
             * Math.Max(0, Math.Min(ZEnd, other.ZEnd) - Math.Max(ZStart, other.ZStart));

        public double Volume => (XEnd - XStart) * (YEnd - YStart) * (ZEnd - ZStart);

        public Cube Intersection(Cube other)
        {
            return new Cube()
            {
                TurnOn = TurnOn,
                XStart = Math.Max(XStart, other.XStart),
                XEnd = Math.Min(XEnd, other.XEnd),
                YStart = Math.Max(YStart, other.YStart),
                YEnd = Math.Min(YEnd, other.YEnd),
                ZStart = Math.Max(ZStart, other.ZStart),
                ZEnd = Math.Min(ZEnd, other.ZEnd)
            };
        }

        public Cube Clone()
        {
            return new Cube
            {
                TurnOn = this.TurnOn,
                XStart = this.XStart,
                XEnd = this.XEnd,
                YStart = this.YStart,
                YEnd = this.YEnd,
                ZStart = this.ZStart,
                ZEnd = this.ZEnd
            };
        }
    }
}
