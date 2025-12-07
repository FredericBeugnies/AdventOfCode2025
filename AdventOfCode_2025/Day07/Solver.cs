namespace AdventOfCode_2025.Day07
{
    internal class Solver
    {
        internal static void Solve(string inputFilePath)
        {
            var lines = File.ReadAllLines(inputFilePath).Select(s => s.ToArray()).ToArray();

            Console.WriteLine($"Day 7 part 1: {SolvePart1(lines)}");
            Console.WriteLine($"Day 7 part 2: {SolvePart2(lines)}");
        }

        private static int SolvePart1(char[][] lines)
        {
            bool[,] reached = new bool[lines.Length, lines[0].Length];

            for (int j = 0; j < lines[0].Length; ++j)
                if (lines[0][j] == 'S')
                    reached[0, j] = true;

            int nbSplits = 0;
            for (int i = 1; i < lines.Length; ++i)
            {
                for (int j = 0; j < lines[i].Length; ++j)
                {
                    if (lines[i][j] == '.' && reached[i - 1, j])
                        reached[i, j] = true;
                    else if (lines[i][j] == '^' && reached[i - 1, j])
                    {
                        ++nbSplits;
                        reached[i, j - 1] = true;
                        reached[i, j + 1] = true;
                    }
                }
            }

            return nbSplits;
        }

        private static long SolvePart2(char[][] lines)
        {
            long[,] nbPaths = new long[lines.Length, lines[0].Length];

            for (int i = lines.Length - 1; i >= 0; --i)
                for (int j = 0; j < lines[i].Length; ++j)
                    if (lines[i][j] == '^')
                        nbPaths[i, j] = CountPaths(lines, nbPaths, i, j - 1) + CountPaths(lines, nbPaths, i, j + 1);
                    else if (lines[i][j] == 'S')
                        return CountPaths(lines, nbPaths, i, j);

            return 0; // should not happen, there is always a 'S'
        }

        private static long CountPaths(char[][] grid, long[,] nbPaths, int row, int col)
        {
            for (int r = row + 1; r < grid.Length; ++r)
                if (grid[r][col] == '^')
                    return nbPaths[r, col];
            return 1;
        }
    }
}
