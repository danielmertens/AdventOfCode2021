namespace AdventOfCode2021.Days;
internal class Day03 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        // PART 1
        var counter = Enumerable.Range(0, lines[0].Length).Select(i => lines.Count(l => l.ElementAt(i) == '1'));
        var gamma = counter.Select(count => count > lines.Length / 2 ? "1" : "0").Aggregate((a, b) => a + b);
        var epsilon = gamma.Select(c => c == '1' ? "0" : "1").Aggregate((a, b) => a + b);
        
        var gammaInt = Convert.ToInt32(gamma, 2);
        var epsilonInt = Convert.ToInt32(epsilon, 2);
        
        Result(gammaInt * epsilonInt, 1);

        // Part 2
        var oxygen = FilterList(lines, (zeros, ones) => ones.Count() >= zeros.Count());
        var co2 = FilterList(lines, (zeros, ones) => ones.Count() < zeros.Count());

        Result(oxygen * co2, 2);
    }

    private int FilterList(IEnumerable<string> originalList, Func<IEnumerable<string>, IEnumerable<string>, bool> criteria)
    {
        var running = true;
        var keepList = originalList;
        int index = 0;

        while (running)
        {
            var zeroList = keepList.Where(item => item.ElementAt(index) == '0').ToList();
            var oneList = keepList.Where(item => item.ElementAt(index) == '1').ToList();

            keepList = criteria(zeroList, oneList) ? oneList : zeroList;
            
            if (keepList.Count() == 1)
            {
                return Convert.ToInt32(keepList.First(), 2);
            }
            index++;
        }

        return -1;
    }
}
