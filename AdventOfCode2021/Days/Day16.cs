namespace AdventOfCode2021.Days;
internal class Day16 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var line = GetInput();

        var bytes = StringToByteArray(line);
        var byteString = bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')).Aggregate((a,b) => a + b);

        // Create the package structure
        var (end, package) = CreatePackage(byteString);

        // Part 1
        Result(SumVersion(package), 1);

        // Part 2
        Result(Calculate(package), 2);
    }

    private int SumVersion(Package pack)
    {
        if (pack.Type == 4)
        {
            return pack.Version;
        }
        else
        {
            var sum = pack.Version;
            var opPack = (OperatorPackage)pack;
            foreach (var sub in opPack.SubSets)
            {
                sum += SumVersion(sub);
            }
            return sum;
        }
    }

    private long Calculate(Package pack)
    {
        if (pack.Type == 4) return (pack as LiteralPackage).Value;

        var opPack = pack as OperatorPackage;

        if (opPack.Type == 0)
        {
            long sum = 0;
            foreach (var sub in opPack.SubSets)
            {
                sum += Calculate(sub);
            }
            return sum;
        }
        else if (opPack.Type == 1)
        {
            long prod = 1;
            foreach (var sub in opPack.SubSets)
            {
                prod *= Calculate(sub);
            }
            return prod;
        }
        else if (opPack.Type == 2)
        {
            long min = long.MaxValue;
            foreach (var sub in opPack.SubSets)
            {
                var val = Calculate(sub);
                if (val < min)
                    min = val;
            }
            return min;
        }
        else if (opPack.Type == 3)
        {
            long max = long.MinValue;
            foreach (var sub in opPack.SubSets)
            {
                var val = Calculate(sub);
                if (val > max)
                    max = val;
            }
            return max;
        }
        else if (opPack.Type == 5)
        {
            var first = Calculate(opPack.SubSets.First());
            var last = Calculate(opPack.SubSets.Last());
            return first > last ? 1 : 0;
        }
        else if (opPack.Type == 6)
        {
            var first = Calculate(opPack.SubSets.First());
            var last = Calculate(opPack.SubSets.Last());
            return first < last ? 1 : 0;
        }
        else if (opPack.Type == 7)
        {
            var first = Calculate(opPack.SubSets.First());
            var last = Calculate(opPack.SubSets.Last());
            return first == last ? 1 : 0;
        }

        throw new InvalidOperationException();
    }

    private (int, Package) CreatePackage(string byteString)
    {
        var versionString = byteString.Substring(0, 3);
        var type = byteString.Substring(3, 3);

        if (type == "100") // Literal
        {
            var numberString = "";
            var index = 6;
            var stop = false;
            do
            {
                var sub = byteString.Substring(index, 5);
                numberString += sub.Substring(1);
                if (sub[0] == '0') stop = true;
                index += 5;
            } while (!stop);

            return(index, new LiteralPackage
            {
                Version = Convert.ToInt32(versionString, 2),
                Type = Convert.ToInt32(type, 2),
                Value = Convert.ToInt64(numberString, 2)
            });
        }
        else
        {
            var lengthType = byteString.Skip(6).Take(1).First();
            if (lengthType == '0')
            {
                var subPackageLengthString = byteString.Substring(7, 15);
                var subPackageLength = Convert.ToInt32(subPackageLengthString, 2);
                var subPackageString = byteString.Substring(22, subPackageLength);

                var startIndex = 0;
                var packs = new List<Package>();
                do
                {
                    var (pos, pack) = CreatePackage(subPackageString.Substring(startIndex));
                    packs.Add(pack);
                    startIndex += pos;
                } while (startIndex < subPackageLength);

                return (22 + subPackageLength, new OperatorPackage
                {
                    Version = Convert.ToInt32(versionString, 2),
                    Type = Convert.ToInt32(type, 2),
                    SubSets = packs
                });
            }
            else
            {
                var subPackageLengthString = byteString.Substring(7, 11);
                var subPackageCount = Convert.ToInt32(subPackageLengthString, 2);
                var subPackageStart = byteString.Substring(18);
                
                var index = 0;
                var packs = new List<Package>();
                for (int i = 0; i < subPackageCount; i++)
                {
                    var (pos, pack) = CreatePackage(subPackageStart.Substring(index));
                    packs.Add(pack);
                    index += pos;
                }

                return (18 + index, new OperatorPackage
                {
                    Version = Convert.ToInt32(versionString, 2),
                    Type = Convert.ToInt32(type, 2),
                    SubSets = packs
                });
            }
        }
    }

    private static byte[] StringToByteArray(string hex)
    {
        return Enumerable.Range(0, hex.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                         .ToArray();
    }

    private class Package
    {
        public int Version { get; set; }
        public int Type { get; set; }
    }

    private class LiteralPackage : Package
    {
        public long Value { get; set; }
    }

    private class OperatorPackage : Package
    {
        public List<Package> SubSets { get; set; }
    }
}
