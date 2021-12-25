namespace AdventOfCode2021.Days;
internal class Day17 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        var left = int.Parse(lines[0]);
        var right = int.Parse(lines[1]);
        var top = int.Parse(lines[2]);
        var bottom = int.Parse(lines[3]);

        var possibleHorVelocity = new List<int>();

        for (int x = 5; x <= right; x++)
        {
            var velocity = x;
            var position = 0;

            while (velocity > 0 && position <= right)
            {
                position += velocity;
                if (position >= left && position <= right)
                {
                    possibleHorVelocity.Add(x);
                    break;
                }
                velocity--;
            }
        }

        var possibleVerVelocity = new List<int>();
        for (int y = 400; y >= bottom; y--)
        {
            var velocity = y;
            var position = 0;

            while (position >= bottom)
            {
                position += velocity;
                if (position <= top && position >= bottom)
                {
                    possibleVerVelocity.Add(y);
                    break;
                }
                velocity--;
            }
        }
        
        var count = 0;
        var maxY = 0;

        foreach (var horVelo in possibleHorVelocity.Distinct())
        {
            foreach (var verVelo in possibleVerVelocity.Distinct())
            {
                var horPos = 0;
                var verPos = 0;
                var currentHorVelo = horVelo;
                var currentVerVelo = verVelo;
                var newMax = 0;

                while(horPos <= right
                    && verPos >= bottom)
                {
                    horPos += currentHorVelo;
                    verPos += currentVerVelo;
                    currentHorVelo--;
                    currentVerVelo--;
                    if (verPos >= maxY && verPos > newMax) newMax = verPos;
                    if (currentHorVelo < 0) currentHorVelo = 0;
                    if (horPos >= left && horPos <= right
                        && verPos <= top && verPos >= bottom)
                    {
                        count++;
                        if (newMax != 0) maxY = newMax;
                        break;
                    }
                }
            }
        }
        Result(maxY);
        Result(count);
    }
}
