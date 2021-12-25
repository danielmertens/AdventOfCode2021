namespace AdventOfCode2021.Days;
internal class Day08 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        var oneSegments = 2;
        var fourSegments = 4;
        var sevenSegments = 3;
        var eightSegments = 7;

        // PART 1
        var count = 0;
        foreach (var line in lines)
        {
            var input = line.Split(" | ")[0];
            var output = line.Split(" | ")[1].Split(' ');

            foreach (var digits in output)
            {
                if (digits.Length == oneSegments
                    || digits.Length == fourSegments
                    || digits.Length == sevenSegments
                    || digits.Length == eightSegments)
                {
                    count++;
                }
            }
        }
        Result(count);

        // PART 2
        count = 0;
        foreach (var line in lines)
        {
            var input = line.Split(" | ")[0];
            var output = line.Split(" | ")[1].Split(" ");

            var one = input.Split(' ').Where(i => i.Length == oneSegments).First();
            var four = input.Split(' ').Where(i => i.Length == fourSegments).First();
            var seven = input.Split(' ').Where(i => i.Length == sevenSegments).First();
            var eight = input.Split(' ').Where(i => i.Length == eightSegments).First();

            // 0 -> 6 seg
            // 6 -> 6 seg
            // 9 -> 6 seg
            
            // 2 -> 5 seg
            // 3 -> 5 seg
            // 5 -> 5 seg
            
            // 1 -> 2 seg
            // 4 -> 4 seg
            // 7 -> 3 seg
            // 8 -> 7 seg

            // Process of elimination.
            var six = input.Split(' ')
                .Where(i => i.Length == 6)
                .Where(i => i.Intersect(one.ToCharArray()).Count() == 1)
                .First();

            var three = input.Split(' ')
                .Where(i => i.Length == 5)
                .Where(i => i.Intersect(one).Count() == 2)
                .First();

            var two = input.Split(' ')
                .Where(i => i.Length == 5)
                .Where(i => i.Intersect(one).Count() == 1 && i.Intersect(six).Count() == 4)
                .First();

            var five = input.Split(' ')
                .Where(i => i.Length == 5)
                .Where(i => i != three && i != two)
                .First();

            var nine = input.Split(' ')
                .Where(i => i.Length == 6)
                .Where(i => i != six)
                .Where(i => i.Intersect(five).Count() == 5)
                .First();

            var zero = input.Split(' ')
                .Where(i => i.Length == 6)
                .Where(i => i != six && i != nine)
                .First();

            string[] numbers = new string[] { zero, one, two, three, four, five, six, seven, eight, nine };

            var number = 0;
            foreach (var op in output)
            {
                var index = -1;
                for (int i = 0; i < numbers.Length; i++)
                {
                    // Segments are scrambled again in the output!!!
                    if (numbers[i].Length == op.Length && numbers[i].Intersect(op).Count() == op.Length)
                    {
                        index = i; 
                        break;
                    }
                }
                number = (number * 10) + index;
            }
            count += number;
        }

        Result(count);
    }
}
