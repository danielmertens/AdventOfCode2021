namespace AdventOfCode2021.Days;
internal class Day07 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var positions = GetInputLines()[0].Split(',').Select(s => int.Parse(s)).ToArray();
        var max = positions.Max();
        
        // PART 1
        var burn = new int[max + 1];
        for (int i = 1; i <= max; i++)
        {
            burn[i] = burn[i-1] + 1;
        }

        var cost = CalculateCost(positions, burn);
        Result(cost, 1);

        // PART 2
        for (int i = 1;i <= max; i++)
        {
            burn[i] = burn[i-1] + i;
        }

        cost = CalculateCost(positions, burn);
        Result(cost, 2);
    }

    private int CalculateCost(int[] positions, int[] fuel)
    {
        var max = positions.Max();
        return Enumerable.Range(0, max)
            .Select(i => positions.Select(p => fuel[Math.Abs(p - i)]).Sum())
            .Min();
    }
}
