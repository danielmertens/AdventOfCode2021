namespace AdventOfCode2021.Days;
internal class Day14 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        var transformations = new Dictionary<int, char>();
        for (int i = 2; i < lines.Length; i++)
        {
            transformations.Add(Hash(lines[i].ElementAt(0), lines[i].ElementAt(1)), lines[i].ElementAt(6));
        }

        // PART 1
        var linked = new LinkedList<char>(lines[0].ToCharArray());

        for (int step = 0; step < 10; step++)
        {
            var skip = false;
            for (LinkedListNode<char>? node = linked.First; node.Next != null; node = node.Next)
            {
                if (skip)
                {
                    skip = false;
                    continue;
                }
                if (transformations.TryGetValue(Hash(node.Value, node.Next.Value), out char transform))
                {
                    linked.AddAfter(node, transform);
                    skip = true;
                }
            }
        }


        var group = linked.GroupBy(n => n)
            .Select(g => g.Count());
            
        Result(group.Max() - group.Min(), 1);

        // PART 2
        var input = lines[0];
        Dictionary<int, double> countTable = new Dictionary<int, double>();

        for (int i = 0; i < input.Length - 1; i++)
        {
            var hash = Hash(input[i], input[i + 1]);
            if (countTable.ContainsKey(hash))
            {
                countTable[hash]++;
            }
            else
            {
                countTable.Add(hash, 1);
            }
        }

        for (int step = 0; step < 40; step++)
        {
            var temp = new Dictionary<int, double>();
            foreach (var key in countTable.Keys)
            {
                if (transformations.ContainsKey(key))
                {
                    var insert = transformations[key];
                    var (f, l) = GetChars(key);

                    if (temp.ContainsKey(Hash(f, insert))) temp[Hash(f, insert)] += countTable[key];
                    else temp.Add(Hash(f, insert), countTable[key]);
                    
                    if (temp.ContainsKey(Hash(insert, l))) temp[Hash(insert, l)] += countTable[key];
                    else temp.Add(Hash(insert, l), countTable[key]);
                }
                else
                {
                    temp.Add(key, countTable[key]);
                }
            }
            countTable = temp;
        }

        var counting = countTable.Select(kv =>
            {
                var (f,l) = GetChars(kv.Key);
                return new[]
                {
                    (f,kv.Value),
                    (l, kv.Value)
                };
            })
            .SelectMany(p => p)
            .GroupBy(x => x.Item1)
            .Select(g => g.Sum(x => x.Value/2))
            .ToList();

        Result(counting.Max() - counting.Min(), 2);
    }

    private int Hash(char a, char b)
    {
        return a * 1000 + b;
    }

    private (char f, char l) GetChars(int hash)
    {
        var f = (char)(hash/1000);
        var l = (char)(hash%1000);
        return (f, l);
    }
}
