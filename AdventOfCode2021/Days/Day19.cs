using System.Diagnostics;
using System.Numerics;

namespace AdventOfCode2021.Days;
internal class Day19 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        Stopwatch watch = Stopwatch.StartNew();

        var scanners = new List<Scanner>();
        List<int[]> probes = new List<int[]>();
        
        int scannerId = -1;
        foreach (var line in lines)
        {
            if (line.StartsWith("---"))
            {
                if (scannerId == -1) { }
                else if (scannerId == 0)
                {
                    var scanner = new Scanner();
                    scanner.Id = scannerId;
                    scanner.SetPosition(0, 0, 0);
                    scanner.PositionFound = true;
                    scanner.Beacons = probes.Select(p => new Vector3(p[0], p[1], p[2])).ToArray();
                    scanners.Add(scanner);
                }
                else
                {
                    CreateVarScanners(scanners, probes, scannerId);
                }
                probes = new List<int[]>();
                scannerId = int.Parse(line.Split(" ")[2]);
            }
            else if (line.Length > 0)
            {
                var numbers = line.Split(',').Select(x => int.Parse(x)).ToArray();
                probes.Add(new int[] { numbers[0], numbers[1], numbers[2] });
            }
        }

        CreateVarScanners(scanners, probes, scannerId);

        // Eliminate possible rotations untill we have
        // one scanner of each id.
        var history = new Dictionary<Scanner, List<Scanner>>();
        while(scanners.Any(s => !s.PositionFound))
        {
            Loop(scanners, history);
        }

        // Create Vectors from center
        var beacons = new List<Vector3>();
        foreach (var scanner in scanners)
        {
            foreach (var probe in scanner.Beacons)
            {
                beacons.Add(scanner.Position + probe);
            }
        }
        var total = beacons.Distinct().Count();

        // Calculate distance
        var max = 0f;
        for (int i = 0; i < scanners.Count; i++)
        {
            for (int j = i+1; j < scanners.Count; j++)
            {
                var dist = CalculateDistance(scanners[i], scanners[j]);
                if (dist > max) max = dist;
            }
        }

        watch.Stop();
        Console.WriteLine(watch.Elapsed);

        Result(total, 1);
        Result(max, 2);
    }

    private float CalculateDistance(Scanner scanner1, Scanner scanner2)
    {
        var origin1 = scanner1.Position;
        var origin2 = scanner2.Position;
        return (Math.Abs(origin1.X - origin2.X)
            + Math.Abs(origin1.Y - origin2.Y)
            + Math.Abs(origin1.Z - origin2.Z));
    }

    private static void CreateVarScanners(List<Scanner> scanners, List<int[]> probes, int scannerId)
    {
        var probVar = new Vector3[probes.Count][];
        for (int i = 0; i < probes.Count; i++)
        {
            probVar[i] = GetPermutations(probes[i]).ToArray();
        }

        for (int i = 0; i < probVar[0].Length; i++)
        {
            var scanner = new Scanner();
            scanner.Id = scannerId;
            scanner.Beacons = probVar.Select(v => v.Skip(i).First()).ToArray();
            scanners.Add(scanner);
        }
    }

    private void Loop(List<Scanner> scanners, Dictionary<Scanner, List<Scanner>> history)
    {
        foreach (var foundScanner in scanners.Where(s => s.PositionFound))
        {
            for (int i = 0; i < scanners.Count; i++)
            {
                if (scanners[i].PositionFound) continue;
                if (history.ContainsKey(foundScanner) && history[foundScanner].Contains(scanners[i])) continue;

                var result = LoopAssumePositions(foundScanner, scanners[i]);

                if (result)
                {
                    var id = scanners[i].Id;
                    scanners.RemoveAll(s => s.Id == id && !s.PositionFound);
                    return;
                }
                else
                {
                    if (!history.ContainsKey(foundScanner))
                    {
                        history.Add(foundScanner, new List<Scanner>());
                    }
                    history[foundScanner].Add(scanners[i]);
                }
            }
        }
    }

    /// <summary>
    /// We are gonna guess the position of the scanner.
    /// Pick 2 beacons and assume they are the same. Extrapolate the scanner position from there.
    /// After that check if other beacons match.
    /// </summary>
    private bool LoopAssumePositions(Scanner foundScanner, Scanner scanner)
    {
        foreach (var probe in foundScanner.Beacons)
        {
            foreach(var unknownProbe in scanner.Beacons)
            {
                var assumePosition = (foundScanner.Position + probe) - unknownProbe;
                scanner.SetPosition(assumePosition.X, assumePosition.Y, assumePosition.Z);

                if (scanner.CountOverlap(foundScanner) >= 12)
                {
                    scanner.PositionFound = true;
                    return true;
                }
            }
        }
        return false;
    }

    private class Scanner
    {
        public int Id { get; set; }
        public Vector3[] Beacons { get; set; }
        public bool PositionFound { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3 Position => new Vector3(X, Y, Z);

        public void SetPosition(float x, float y, float z)
        {
            X = x; 
            Y = y; 
            Z = z;
        }

        public int CountOverlap(Scanner scanner)
        {
            var count = 0;
            var origin = Position;
            var scannerOrigin = scanner.Position;

            foreach (var vec in scanner.Beacons)
            {
                foreach (var myVec in Beacons)
                {
                    var scannerProbe = scannerOrigin + vec;
                    var myProbe = origin + myVec;
                    if (myProbe == scannerProbe) count++;
                }
            }

            return count;
        }
    }

    // Creates too many permutations!
    public static List<Vector3> GetPermutations(int[] list)
    {
        var output = new List<Vector3>();
        int x = list.Length - 1;
        GetPermutations(list, 0, x, output);
        return output;
    }

    private static void GetPermutations(int[] list, int k, int m, List<Vector3> output)
    {
        if (k == m)
        {
            output.AddRange(VarNegatives(list));
        }
        else
            for (int i = k; i <= m; i++)
            {
                Swap(ref list[k], ref list[i]);
                GetPermutations(list, k + 1, m, output);
                Swap(ref list[k], ref list[i]);
            }
    }

    private static void Swap(ref int a, ref int b)
    {
        if (a == b) return;

        var temp = a;
        a = b;
        b = temp;
    }

    private static List<Vector3> VarNegatives(params int[] input)
    {
        var list = new List<Vector3>();
        for (int i = -1; i < 2; i += 2)
        {
            for (int j = -1; j < 2; j += 2)
            {
                for (int l = -1; l < 2; l += 2)
                {
                    list.Add(new Vector3(input[0] * i, input[1] * j, input[2] * l));
                }
            }
        }
        return list;
    }
}
