namespace AdventOfCode_2025.Day04
{
    internal class Grid
    {
        public int NbRows;
        public int NbCols;
        public char[,] Cells;
        
        public Grid(int nbRows, int nbCols)
        {
            NbRows = nbRows;
            NbCols = nbCols;
            Cells = new char[NbRows, NbCols];
        }
    }

    internal class Solver
    {
        internal static void Solve(string inputFilePath)
        {
            var grid = ParseInput(inputFilePath);

            Console.WriteLine($"Day 4 part 1: {SolvePart1(grid)}");
            Console.WriteLine($"Day 4 part 2: {SolvePart2(grid)}");
        }

        private static Grid ParseInput(string inputFilePath)
        {
            var lines = File.ReadAllLines(inputFilePath);
            
            var grid = new Grid(lines.Length, lines[0].Length);
            for (int r = 0; r < lines.Length; ++r)
            {
                for (int c = 0; c < lines[r].Length; ++c)
                {
                    grid.Cells[r, c] = lines[r][c];
                }
            }

            return grid;
        }

        private static int SolvePart1(Grid grid)
        {
            int nbReachableRolls = 0;

            for (int r = 0; r < grid.NbRows; ++r)
            {
                for (int c = 0; c < grid.NbCols; ++c)
                {
                    if (grid.Cells[r, c] != '@')
                        continue;

                    int nbAdjacentRolls = 0;
                    for (int dr = -1; dr <= 1; ++dr)
                    {
                        for (int dc = -1; dc <= 1; ++dc)
                        {
                            if (dr == 0 && dc == 0)
                                continue;
                            int nr = r + dr;
                            int nc = c + dc;
                            if (nr >= 0 && nr < grid.NbRows && nc >= 0 && nc < grid.NbCols)
                            {
                                if (grid.Cells[nr, nc] == '@')
                                    ++nbAdjacentRolls;
                            }
                        }
                    }
                    if (nbAdjacentRolls < 4)
                        ++nbReachableRolls;
                }
            }

            return nbReachableRolls;
        }

        private static int SolvePart2(Grid grid)
        {
            int totalRemovableRolls = 0;
            int nbReachableRolls = 0;

            do
            {
                nbReachableRolls = 0;
                for (int r = 0; r < grid.NbRows; ++r)
                {
                    for (int c = 0; c < grid.NbCols; ++c)
                    {
                        if (grid.Cells[r, c] != '@')
                            continue;

                        int nbAdjacentRolls = 0;
                        for (int dr = -1; dr <= 1; ++dr)
                        {
                            for (int dc = -1; dc <= 1; ++dc)
                            {
                                if (dr == 0 && dc == 0)
                                    continue;
                                int nr = r + dr;
                                int nc = c + dc;
                                if (nr >= 0 && nr < grid.NbRows && nc >= 0 && nc < grid.NbCols)
                                {
                                    if (grid.Cells[nr, nc] == '@')
                                        ++nbAdjacentRolls;
                                }
                            }
                        }
                        if (nbAdjacentRolls < 4)
                        {
                            grid.Cells[r, c] = '.';
                            ++nbReachableRolls;
                        }
                    }
                }
                totalRemovableRolls += nbReachableRolls;
            }
            while (nbReachableRolls > 0);
            
            return totalRemovableRolls;
        }
    }
}
