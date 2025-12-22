namespace AdventOfCode_2025.Day03
{
    internal class Solver
    {
        internal static void Solve(string inputFilePath)
        {
            var banks = ParseInput(inputFilePath);

            Console.WriteLine($"Day 3 part 1: {SolvePart1(banks)}");
            Console.WriteLine($"Day 3 part 2: {SolvePart2(banks)}");
        }

        private static List<int[]> ParseInput(string inputFilePath)
        {
            var banks = new List<int[]>();
            var lines = File.ReadAllLines(inputFilePath);
            foreach (string line in lines)
            {
                banks.Add(ConvertInputLine(line));
            }
            return banks;
        }

        private static int[] ConvertInputLine(string line)
        {
            int[] bank = new int[line.Length];
            for (int i = 0; i < line.Length; ++i)
                bank[i] = (int)(line[i] - '0');
            return bank;
        }

        private static int SolvePart1(List<int[]> banks)
        {
            int result = 0;

            foreach (int[] bank in banks)
                result += FindMaxPossibleJoltage_Part1(bank);

            return result;
        }

        private static long SolvePart2(List<int[]> banks)
        {
            long result = 0;

            foreach (int[] bank in banks)
            {
                long joltage = FindMaxPossibleJoltage_Part2(bank);
                Console.WriteLine($"{joltage}");
                result += joltage;
            }
            return result;
        }

        private static int FindMaxPossibleJoltage_Part1(int[] bank)
        {
            int maxJoltage = 0;

            int[] maxLeft = new int[bank.Length];
            int[] maxRight = new int[bank.Length];

            maxLeft[0] = bank[0];
            for (int i = 1; i < bank.Length; ++i)
                maxLeft[i] = Math.Max(maxLeft[i - 1], bank[i]);

            maxRight[bank.Length - 1] = bank[bank.Length - 1];
            for (int i = bank.Length - 2; i >= 0; --i)
                maxRight[i] = Math.Max(maxRight[i + 1], bank[i]);

            for (int i = 0; i < bank.Length - 1; ++i)
            {
                int joltage = maxLeft[i] * 10 + maxRight[i + 1];
                if (joltage > maxJoltage)
                    maxJoltage = joltage;
            }

            return maxJoltage;
        }

        private static long FindMaxPossibleJoltage_Part2(int[] bank)
        {
            int remainingRemovals = bank.Length - 12;
            bool[] removed = new bool[bank.Length];

            int i = 0;
            while (remainingRemovals > 0 && i < bank.Length)
            {
                int m = i;
                for (int j = i + 1; j < bank.Length && j <= i + remainingRemovals; ++j)
                    if (bank[j] > bank[m])
                        m = j;
                
                for (int j = i; j < m; ++j)
                    if (bank[j] < bank[m])
                    {
                        removed[j] = true;
                        --remainingRemovals;
                    }
                
                i = m + 1;
            }

            if (remainingRemovals > 0)
                for (int j = bank.Length - 1; j >= 0 && remainingRemovals > 0; --j)
                    if (!removed[j])
                    {
                        removed[j] = true;
                        --remainingRemovals;
                    }

            return GetJoltage(bank, removed);
        }

        private static long GetJoltage(int[] bank, bool[] removed)
        {
            long joltage = 0;
            for (int i = 0; i < bank.Length; ++i)
                if (!removed[i])
                    joltage = joltage * 10 + bank[i];
            return joltage;
        }
    }
}
