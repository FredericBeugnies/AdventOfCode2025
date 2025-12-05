namespace AdventOfCode_2025.Day05
{
    internal record Range (long Start, long End);

    internal class Solver
    {
        internal static void Solve(string inputFilePath)
        {
            var (ranges, ingredients) = ParseInput(inputFilePath);

            Console.WriteLine($"Day 5 part 1: {SolvePart1(ranges, ingredients)}");
            Console.WriteLine($"Day 5 part 2: {SolvePart2(ranges)}");
        }

        internal static (List<Range>, List<long>) ParseInput(string inputFilePath)
        {
            var lines = File.ReadAllLines(inputFilePath);

            int i = 0;
            var ranges = new List<Range>();
            while (!string.IsNullOrWhiteSpace(lines[i]))
            {
                string[] parts = lines[i].Split('-');
                ranges.Add(new Range(long.Parse(parts[0]), long.Parse(parts[1])));
                ++i;
            }

            ++i; // skip empty line
            var numbers = new List<long>();
            while (i < lines.Length)
            {
                numbers.Add(long.Parse(lines[i]));
                ++i;
            }

            return (ranges, numbers);
        }

        private static int SolvePart1(List<Range> ranges, List<long> ingredients)
        {
            int nbFreshIngredients = 0;
            foreach (var ingredient in ingredients)
            {
                foreach (var range in ranges)
                {
                    if (ingredient >= range.Start && ingredient <= range.End)
                    {
                        ++nbFreshIngredients;
                        break;
                    }
                }
            }
            return nbFreshIngredients;
        }

        private static long SolvePart2(List<Range> ranges)
        {
            long nbFreshIngredientIds = 0;
            long maxFreshId = -1;

            var rangesSorted = ranges.OrderBy(r => r.Start).ToList();
            foreach (var range in rangesSorted)
            {
                long trueStart = Math.Max(range.Start, maxFreshId + 1);
                if (trueStart <= range.End)
                {
                    nbFreshIngredientIds += (range.End - trueStart + 1);
                    maxFreshId = range.End;
                }
            }
            
            return nbFreshIngredientIds;
        }
    }
}
