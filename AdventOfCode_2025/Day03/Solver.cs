namespace AdventOfCode_2025.Day03
{
    internal class Solver
    {
        internal static void Solve(string inputFilePath)
        {
            var banks = ParseInput(inputFilePath);

            Console.WriteLine($"Day 3 part 1: {SolvePart1(banks)}");
            //Console.WriteLine($"Day 3 part 2: {SolvePart2(banks)}");
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
            // find next local maximum
            int i = 0;
            int remainingRemovals = bank.Length - 12;
            bool[] removed = new bool[bank.Length];

            while (remainingRemovals > 0 && i < bank.Length)
            {
                int local_max_index = -1;
                int local_max = 0;
                for (int j = i; j < Math.Min(i + remainingRemovals + 1, bank.Length); ++j)
                {
                    if (bank[j] > local_max)
                    {
                        local_max = bank[j];
                        local_max_index = j;
                    }
                }

                if (local_max_index != i)
                {
                    for (int k = i; k < local_max_index; ++k)
                    {
                        removed[k] = true;
                        --remainingRemovals;
                    }
                    i = local_max_index + 1;
                }
                else if (bank[i] == bank[i+1])
                {
                    removed[i] = true;
                    --remainingRemovals;
                    ++i;
                }
                else
                {
                    ++i;
                }
            }


            //int i = 0;
            //int digitToRemove = 1;
            //while (nbRemainingDigits > 12)
            //{
            //    if (!removed[i] && bank[i] == digitToRemove)
            //    {
            //        removed[i] = true;
            //        --nbRemainingDigits;
            //    }
            //    ++i;
            //    if (i == bank.Length)
            //    {
            //        // debug
            //        for (int j = 0; j < bank.Length; ++j)
            //            if (!removed[j])
            //                Console.Write(bank[j]);
            //        Console.WriteLine($"nbRemainingDigits: {nbRemainingDigits}");
            //        i = 0;
            //        ++digitToRemove;
            //    }
            //}

            long maxJoltage = 0;
            for (i = 0; i < bank.Length; ++i)
            {
                if (!removed[i])
                    maxJoltage = maxJoltage * 10 + bank[i];
            }
            return maxJoltage;
        }
    }
}
