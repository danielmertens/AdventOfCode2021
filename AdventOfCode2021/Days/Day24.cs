namespace AdventOfCode2021.Days;
internal class Day24 : AbstractDay
{
    protected override void Execute()
    {
        var calculationsPerInput = new Func<int, long, long>[]
        {
            GenericCalculation(1, 12, 1),
            GenericCalculation(1, 13, 9),
            GenericCalculation(1, 12, 11),
            GenericCalculation(26, -13, 6),
            GenericCalculation(1, 11, 6),
            GenericCalculation(1, 15, 13),
            GenericCalculation(26, -14, 13),
            GenericCalculation(1, 12, 5),
            GenericCalculation(26, -8, 7),
            GenericCalculation(1, 14, 2),
            GenericCalculation(26, -9, 10),
            GenericCalculation(26, -11, 14),
            GenericCalculation(26, -6, 7),
            GenericCalculation(26, -5, 1)
        };

        var result = CalculateNumber(calculationsPerInput, new Dictionary<long, long> { { 0, 0 } }, 0);

        Result(result, 2);
    }
    
    private long CalculateNumber(Func<int, long, long>[] calculations, Dictionary<long, long> zNumbers, int step)
    {
        if (step == 14)
        {
            return zNumbers[0];
        }

        var quickMemory = new HashSet<long>();
        var dictionary = new Dictionary<long, long>();
        var numbers = zNumbers.Keys.ToList();
        Console.WriteLine($"{step} {numbers.Count}");

        for (int i = 0; i < numbers.Count; i++)
        {
            for (int j = 1; j < 10; j++)
            {
                var z = calculations[step](j, numbers[i]);
                
                if (quickMemory.Contains(z))
                {
                    // Change condition for part 1 and 2;
                    if (dictionary[z] > zNumbers[numbers[i]] * 10 + j)
                    {
                        dictionary[z] = zNumbers[numbers[i]] * 10 + j;
                    }
                }
                else
                {
                    dictionary.Add(z, zNumbers[numbers[i]] * 10 + j);
                    quickMemory.Add(z);
                }
            }
        }

        return CalculateNumber(calculations, dictionary, step + 1);
    }

    private Func<int, long, long> GenericCalculation(int divZ, int addX, int addW)
        => (w, z) =>
        {
            var x = z % 26;
            z = z / divZ;
            x = x + addX;
            x = x == w ? 0 : 1;

            z = z * ((25L * x) + 1);

            return z + ((w + addW) * x);
        };
}
