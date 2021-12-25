namespace AdventOfCode2021.Days;
internal class Day01 : AbstractDay
{
    /*
     * NOTES
     * 
     * PART 1
     * 
     * Count increases relative to the previous value.
     * 
     * PART 2
     * 
     * Same as part 1 but use a sliding window with the sum of 3 numbers.
     * 
     */

    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        int[] numbers = GetInputNumbers();

        // PART 1
        int counter = 0;
        for (int i = 1; i < numbers.Length; i++)
        {
            if(numbers[i] > numbers[i-1]) counter++;
        }
        Console.WriteLine("PART 1: " + counter);

        // Part 2
        counter = 0;
        for (int i = 3; i < numbers.Length; i++)
        {
            if (GetWindow(i, numbers) > GetWindow(i - 1, numbers)) counter++; 
        }
        Console.WriteLine("PART 2: " + counter);
    }

    private int GetWindow(int index, int[] list)
    {
        return list[index-2] + list[index-1] + list[index];
    }
}
