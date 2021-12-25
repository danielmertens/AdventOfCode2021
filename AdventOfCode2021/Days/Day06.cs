namespace AdventOfCode2021.Days;
internal class Day06 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var numbers = GetInputLinesSplit(",")[0].Select(x => int.Parse(x)).ToList();
        var days = 256; // 80 for Part 1

        double[] fish = new double[9];

        foreach (var item in numbers)
        {
            fish[item]++;
        }

        for (int d = 0; d < days; d++)
        {
            var next = new double[9];

            next[8] = fish[0];
            next[6] = fish[0];
            for (int i = 1; i <= 8; i++)
            {
                next[i-1] += fish[i];
            }
            fish = next;
        }
        Result(fish.Sum());
    }
}
