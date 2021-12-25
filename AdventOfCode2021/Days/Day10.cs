namespace AdventOfCode2021.Days;
internal class Day10 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        var open = new char[] { '(', '[', '{', '<' };

        var score = 0;
        var part2Scores = new List<long>();

        foreach (var line in lines)
        {
            var stack = new Stack<char>();
            var found = false;
            
            for (int i = 0; i < line.Length; i++)
            {
                if (open.Contains(line[i])) stack.Push(line[i]);
                else if (opposite(stack.Peek()) == line[i]) stack.Pop();
                else
                {
                    score+= CorruptScore(line[i]);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                long part2 = 0;
                while(stack.TryPop(out char activeChar))
                {
                    int value = CompleteScore(opposite(activeChar));
                    part2 *= 5;
                    part2 += value;
                }

                part2Scores.Add(part2);
            }
        }

        Result(score);

        var res = part2Scores.OrderByDescending(n => n)
            .ElementAt(part2Scores.Count / 2);
        Result(res);
    }

    private static char opposite(char character)
    {
        switch (character)
        {
            case '(': return ')';
            case '[': return ']';
            case '{': return '}';
            case '<': return '>';
        }
        throw new ArgumentException();
    }

    private static int CorruptScore(char character)
    {
        switch (character)
        {
            case ')': return 3;
            case ']': return 57;
            case '}': return 1197;
            case '>': return 25137;
        }
        throw new ArgumentException();
    }

    private static int CompleteScore(char character)
    {
        switch (character)
        {
            case ')': return 1;
            case ']': return 2;
            case '}': return 3;
            case '>': return 4;
        }
        throw new ArgumentException();
    }
}
