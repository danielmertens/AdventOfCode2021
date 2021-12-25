namespace AdventOfCode2021.Days;
internal class Day20 : AbstractDay
{
    protected override void Execute()
    {
        Console.WriteLine("Reading file");
        var lines = GetInputLines();

        var algo = lines[0];
        var image = lines.Skip(2).Select(l => l.Select(x => x == '#' ? 1 : 0).ToArray()).ToArray();

        for (int i = 0; i < 50; i++)
        {
            image = GrowImage(image, i);
            image = EnhanceImage(image, algo, i);
        }

        var lit = image.SelectMany(x => x).Sum();

        Result(lit);
    }

    private static int[][] EnhanceImage(int[][] image, string algo, int step)
    {
        var newImage = new int[image.Length][];
        for (var i = 0; i < newImage.Length; i++)
        {
            newImage[i] = new int[image[0].Length];
        }

        for (int i = 0; i < image.Length; i++)
        {
            for (int j = 0; j < image[i].Length; j++)
            {
                var number = GetImageNumber(image, i, j, step);
                newImage[i][j] = algo.ElementAt(number) == '#' ? 1 : 0;
            }
        }
        return newImage;
    }

    private static int GetImageNumber(int[][] image, int middleY, int middleX, int step)
    {
        int number = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (middleY + i >= image.Length
                    || middleY + i < 0
                    || middleX + j >= image[0].Length
                    || middleX + j < 0)
                {
                    number = (number << 1) + (step % 2);
                }
                else
                {
                    number = (number << 1) + image[middleY + i][middleX + j];
                }
            }
        }
        return number;
    }

    private static int[][] GrowImage(int[][] image, int step)
    {
        // Important note: The input algorithm has a # at position 0
        // and a . at position 512. This means to infinity the edge
        // switches from light to dark every step! Fill makes sure the
        // edge has the correct state when the image is grown.
        // Same is done in GetImageNumber(...)
        int fill = step % 2;

        var newImage = new int[image.Length + 1 * 2][];
        for (var i = 0; i < newImage.Length; i++)
        {
            newImage[i] = new int[image[0].Length + 1 * 2];
            for (var j = 0; j < newImage[0].Length; j++)
            {
                newImage[i][j] = fill;
            }
        }

        for (var i = 0; i < image.Length; i++)
        {
            for (int j = 0; j < image[i].Length; j++)
            {
                newImage[i + 1][j + 1] = image[i][j];
            }
        }
        return newImage;
    }
}
