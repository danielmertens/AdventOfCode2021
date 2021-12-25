using System.Diagnostics;
using TextCopy;

namespace AdventOfCode2021.Days;
internal abstract partial class AbstractDay
{
    public void SolveProblem()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        PrintHeader();

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        Execute();
        stopWatch.Stop();

        PrintFooter(stopWatch);
        Console.ForegroundColor = ConsoleColor.White;
    }


    private void PrintHeader()
    {
        var headerText = $"||  RUNNING {GetType().Name.ToUpper()}  ||";

        Console.WriteLine(new string('=', headerText.Length));
        Console.WriteLine(headerText);
        Console.WriteLine(new string('=', headerText.Length));
    }

    private void PrintFooter(Stopwatch stopWatch)
    {
        var footerText = string.Format("Execution time {0}", stopWatch.Elapsed);
        Console.WriteLine(new string('-', footerText.Length));
        Console.WriteLine(footerText);
    }

    protected void Result<T>(T result)
    {
        var resultText = result.ToString();
        Console.WriteLine("Result: " + resultText);
        Clipboard clipboard = new();
        clipboard.SetText(resultText);
    }

    protected void Result<T>(T result, int part)
    {
        var resultText = result.ToString();
        Console.WriteLine($"PART {part}: {resultText}");
    }

    protected abstract void Execute();
}
