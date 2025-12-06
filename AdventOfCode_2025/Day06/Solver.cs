namespace AdventOfCode_2025.Day06
{
    internal class Solver
    {
        internal static void Solve(string inputFilePath)
        {
            var lines = File.ReadAllLines(inputFilePath);

            Console.WriteLine($"Day 6 part 1: {SolvePart1(lines)}");
            Console.WriteLine($"Day 6 part 2: {SolvePart2(lines)}");
        }

        private static long SolvePart1(string[] input)
        {
            string[][] table = new string[input.Length][];
            int i = 0;
            foreach (var line in input)
                table[i++] = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            long result = 0;
            for (int col = 0; col < table[0].Length; ++col)
            {
                if (table[4][col] == "*")
                {
                    long colResult = 1;
                    for (int row = 0; row < 4; ++row)
                    {
                        colResult *= long.Parse(table[row][col]);
                    }
                    result += colResult;
                }
                else // "+"
                {
                    long colResult = 0;
                    for (int row = 0; row < 4; ++row)
                    {
                        colResult += long.Parse(table[row][col]);
                    }
                    result += colResult;

                }
            }
            return result;
        }

        private static long SolvePart2(string[] input)
        {
            bool[] isSeparatorCol = new bool[input[0].Length];
            int[] number = new int[input[0].Length];

            for (int col = 0; col < input[0].Length; ++col)
            {
                isSeparatorCol[col] = input[0][col] == ' ' && input[1][col] == ' ' && input[2][col] == ' ' && input[3][col] == ' ';
                if (!isSeparatorCol[col])
                {
                    number[col] = 0;
                    for (int row = 0; row < 4; ++row)
                    {
                        if (input[row][col] != ' ')
                            number[col] = number[col] * 10 + (input[row][col] - '0');
                    }
                }
            }
            
            long result = 0;
            for (int i = 0; i < input[4].Length; ++i)
            {
                if (input[4][i] == ' ')
                    continue;
                else if (input[4][i] == '*')
                {
                    long product = number[i];
                    int j = i + 1;
                    while (j < input[4].Length && !isSeparatorCol[j])
                    {
                        product *= number[j];
                        ++j;
                    }
                    result += product;
                }
                else if (input[4][i] == '+')
                {
                    long sum = number[i];
                    int j = i + 1;
                    while (j < input[4].Length && !isSeparatorCol[j])
                    {
                        sum += number[j];
                        ++j;
                    }
                    result += sum;
                }
            }
            return result;
        }
    }
}
