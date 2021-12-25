namespace AdventOfCode2021.Days;
internal class Day02 : AbstractDay
{
    /*
     * NOTES
     * 
     * PART 1
     * 
     * Calculate the horizontal and vertical movement of the submarine.
     * 
     * result is horizontal * depth.
     * 
     * PART 2
     * 
     * Depth is calculated by the aim (think diagonal movement)
     * 
     * result is horizontal * depth.
     * 
     */

    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        (string Dir, int Value)[] lines = GetInputLinesSplit<string, int>();

        // PART 1
        int result;
        int hor = 0;
        int depth = 0;
        
        foreach (var line in lines)
        {
            if(line.Dir == "forward") hor+= line.Value;
            else if (line.Dir == "down") depth+= line.Value;
            else depth -= line.Value;
        }

        result = hor * depth;
        Result(result, 1);

        // Part 2

        hor = 0;
        depth = 0;
        int aim = 0;

        foreach (var line in lines)
        {
            if (line.Dir == "forward")
            {
                hor += line.Value;
                depth += aim * line.Value;
            }
            else if (line.Dir == "down") aim += line.Value;
            else aim -= line.Value;
        }

        result = hor * depth;
        Result(result, 2);
    }
}
