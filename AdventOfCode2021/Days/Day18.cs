namespace AdventOfCode2021.Days;
internal class Day18 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();
        var pairs = lines.Select(line => CreatePair(line));
        Pair cummulativePair = pairs.First();

        foreach (var pair in pairs.Skip(1))
        {
            var add = new Pair();
            add.AddLeft(cummulativePair);
            add.AddRight(pair);
            cummulativePair = add;
            
            while (SearchExplode(cummulativePair, 1) || SearchSplit(cummulativePair)) { }
        }

        Result(cummulativePair.Magnitude(), 1);

        int max = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines.Length; j++)
            {
                if (i == j) continue;
                var pair = new Pair();
                pair.AddLeft(CreatePair(lines[i]));
                pair.AddRight(CreatePair(lines[j]));

                while (SearchExplode(pair, 1) || SearchSplit(pair)){ }

                var mag = pair.Magnitude();
                if (mag > max) max = mag;
            }
        }

        Result(max, 2);
    }

    private bool SearchExplode(Pair pair, int depth)
    {
        if (depth > 4 && !pair.HasValue)
        {
            Explode(pair);
            return true;
        }

        if (!pair.HasValue && (SearchExplode(pair.Left, depth + 1) || SearchExplode(pair.Right, depth + 1)))
        {
            return true;
        }

        return false;
    }

    private bool SearchSplit(Pair pair)
    {
        if (pair.Value >= 10)
        {
            Split(pair);
            return true;
        }

        if (!pair.HasValue && (SearchSplit(pair.Left) || SearchSplit(pair.Right)))
        {
            return true;
        }

        return false;
    }

    private void Split(Pair pair)
    {
        pair.AddLeft(new Pair{ Value = (int)Math.Floor(pair.Value / 2.0m) });
        pair.AddRight(new Pair { Value = (int)Math.Ceiling(pair.Value / 2.0m) });
        pair.Value = -1;
    }

    private void Explode(Pair pair)
    {
        var left = SearchFirstLeft(pair);
        var right = SearchFirstRight(pair);

        if (left != null) left.Value += pair.Left.Value;
        if (right != null) right.Value += pair.Right.Value;

        pair.Clear(0);
    }

    private Pair? SearchFirstLeft(Pair pair)
    {
        var current = pair.Parent;
        
        while (current != null)
        {
            if (!current.Left.Contains(pair))
            {
                return FindRightMostValue(current.Left);
            }
            current = current.Parent;
        }

        return null;
    }

    private Pair? SearchFirstRight(Pair pair)
    {
        var current = pair.Parent;

        while (current != null)
        {
            if (!current.Right.Contains(pair))
            {
                return FindLeftMostValue(current.Right);
            }
            current=current.Parent;
        }

        return null;
    }

    private Pair FindRightMostValue(Pair pair)
    {
        if (pair.HasValue) return pair;
        else return FindRightMostValue(pair.Right);
    }

    private Pair FindLeftMostValue(Pair pair)
    {
        if (pair.HasValue) return pair;
        else return FindLeftMostValue(pair.Left);
    }

    public Pair CreatePair(string input)
    {
        Pair root = null;
        Pair active = null;

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '[')
            {
                if (root == null)
                {
                    root = new Pair();
                    active = root;
                }
                else
                {
                    if (active.Left == null)
                    {
                        var p = new Pair();
                        active.AddLeft(p);
                        active = p;
                    }
                    else
                    {
                        var p = new Pair();
                        active.AddRight(p);
                        active = p;
                    }
                }
            }
            else if (input[i] == ']')
            {
                if (active.Parent != null)
                {
                    active = active.Parent;
                }
            }
            else if (input[i] >= '0' && input[i] <= '9')
            {
                if (active.Left == null)
                {
                    var p = new Pair()
                    {
                        Value = int.Parse(input[i].ToString()),
                    };
                    active.AddLeft(p);
                }
                else
                {
                    var p = new Pair()
                    {
                        Value = int.Parse(input[i].ToString()),
                    };
                    active.AddRight(p);
                }
            }
        }

        return root;
    }

    public void Print(Pair pair)
    {
        if (pair.HasValue)
        {
            Console.Write(pair.Value);
        }
        else
        {
            Console.Write("[");
            Print(pair.Left);
            Console.Write(",");
            Print(pair.Right);
            Console.Write("]");
        }
        if (pair.Parent == null)
        {
            Console.WriteLine();
        }
    }

    public class Pair
    {
        public Pair Parent { get; set; }
        public Pair Left { get; private set; }
        public Pair Right { get; private set; }
        public int Value { get; set; } = -1;
        public bool HasValue => Value != -1;

        public void AddLeft(Pair pair)
        {
            Left = pair;
            pair.Parent = this;
        }

        public void AddRight(Pair pair)
        {
            Right = pair;
            pair.Parent = this;
        }

        public bool Contains(Pair pair)
        {
            return pair == this 
                || (Left != null && Left.Contains(pair)) 
                || (Right != null && Right.Contains(pair));
        }

        public void Clear(int value = 0)
        {
            Value = value;
            Left = null;
            Right = null;
        }

        public int Magnitude()
        {
            if (HasValue) return Value;
            else
            {
                return Left.Magnitude() * 3 + Right.Magnitude() * 2;
            }
        }
    }
}
