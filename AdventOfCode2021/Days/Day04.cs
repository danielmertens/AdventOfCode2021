namespace AdventOfCode2021.Days;
internal class Day04 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        var inputs = lines[0].Split(',').Select(n => int.Parse(n)).ToArray();

        var boards = new List<int[][]>();

        for (var i = 2; i < lines.Length; i+= 6)
        {
            var board = new int[5][];
            for (int j = 0; j < 5; j++)
            {
                board[j] = lines[i + j].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
            }
            boards.Add(board);
        }

        // PART 1
        
        var won = -1;
        for (int i = 1; i < inputs.Length && won == -1; i++)
        {
            var currentInputs = inputs.Take(i).ToArray();

            for (int j = 0; j < boards.Count && won == -1; j++)
            {
                won = BoardScore(currentInputs, boards[j]);
            }
        }

        Result(won, 1);

        // PART 2

        won = -1;
        for (int i = 1; i < inputs.Length; i++)
        {
            var currentInputs = inputs.Take(i).ToArray();

            for (int j = 0; j < boards.Count; j++)
            {
                var score = BoardScore(currentInputs, boards[j]);
                if (score != -1)
                {
                    boards.RemoveAt(j);
                    j--;
                    won = score;
                }
            }
        }

        Result(won, 2);
    }

    private int BoardScore(int[] input, int[][] board)
    {
        for (int i = 0; i < 5; i++)
        {
            // Horizontal
            if (board[i].Intersect(input).Count() == 5)
            {
                return CalculateWin(input, board);
            }

            // Vertical
            if (board.Select(row => row[i]).Intersect(input).Count() == 5)
            {
                return CalculateWin(input, board);
            }
        }

        return -1;
    }

    private int CalculateWin(int[] input, int[][] board)
    {
         int count = board.SelectMany(row => row)
            .Except(input)
            .Sum();

        return count * input[input.Length - 1];
    }
}
