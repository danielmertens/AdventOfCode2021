namespace AdventOfCode2021.Days;
internal class Day21 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        var player1 = new Player()
        {
            Id = 1,
            Position = int.Parse(lines[0].Split(' ').Last()) -1,
            Score = 0
        };

        var player2 = new Player()
        {
            Id = 1,
            Position = int.Parse(lines[1].Split(' ').Last()) -1,
            Score = 0
        };

        var dice = new DeterministicDice();

        var step = 0;
        while (player1.Score < 1000 && player2.Score < 1000)
        {
            if (step %2 == 0)
            {
                var roll = dice.Roll();
                player1.Move(roll);
            }
            else
            {
                var roll = dice.Roll();
                player2.Move(roll);
            }
            step++;
        }

        var result = (player1.Score >= 1000 ? player2.Score : player1.Score) * (step * 3);

        Result(result);

        player1 = new Player()
        {
            Id = 1,
            Position = int.Parse(lines[0].Split(' ').Last()) - 1,
            Score = 0
        };

        player2 = new Player()
        {
            Id = 1,
            Position = int.Parse(lines[1].Split(' ').Last()) - 1,
            Score = 0
        };

        var rolls = CreateQuantumRolls();
        Rec(player1, player2, 0, rolls, 1);
        Result(player1.Wins);
    }

    private RollGrouping[] CreateQuantumRolls()
    {
        var list = new List<int>();
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                for (int k = 1; k <= 3; k++)
                {
                    list.Add(i + j + k);
                }
            }
        }
        return list.GroupBy(x => x)
            .Select(g => new RollGrouping{ Roll = g.Key, Dimensions = g.Count() })
            .ToArray();
    }

    struct RollGrouping
    {
        public int Roll;
        public int Dimensions;
    }

    private void Rec(Player p1, Player p2, int step, RollGrouping[] possibleRolls, double universes)
    {
        if (p1.Score >= 21)
        {
            p1.Wins += universes;
            return;
        }
        else if (p2.Score >= 21)
        {
            p2.Wins += universes;
            return;
        }

        var activePlayer = step % 2 == 0 ? p1 : p2;

        for (int i = 0; i < possibleRolls.Length; i++)
        {
            universes *= possibleRolls[i].Dimensions;
            activePlayer.Move(possibleRolls[i].Roll);
            Rec(p1, p2, step + 1, possibleRolls, universes);
            activePlayer.MoveBack(possibleRolls[i].Roll);
            universes /= possibleRolls[i].Dimensions;
        }
    }

    private class DeterministicDice
    {
        private int _nextNumber = 1;

        public int NextNumber 
        {
            get { return _nextNumber; }
            set {
                if (value == 101)
                {
                    _nextNumber = 1;
                }
                else
                {
                    _nextNumber = value;
                }
            } 
        }

        public int Roll()
        {
            return NextNumber++ + NextNumber++ + NextNumber++;
        }
    }

    private class Player
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public int Score { get; set; }
        public double Wins { get; set; }
        public double Universes { get; set; } = 1;

        public void Move(int steps)
        {
            Position = (Position + steps) % 10;
            Score += Position + 1;
        }

        public void MoveBack(int steps)
        {
            Score -= Position + 1;
            Position = (Position + 10 - steps) % 10;
        }
    }
}
