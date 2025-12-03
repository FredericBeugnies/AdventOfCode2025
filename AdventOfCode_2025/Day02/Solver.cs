namespace AdventOfCode_2025.Day02
{
    record Range (long Start, long End);

    internal class Solver
    {
        internal static void Solve(string inputFilePath)
        {
            var ranges = ReadInput(inputFilePath);

            Console.WriteLine($"Day 2 part 1: {SolvePart1(ranges)}");
            Console.WriteLine($"Day 2 part 2: {SolvePart2(ranges)}");
        }

        private static List<Range> ReadInput(string inputFilePath)
        {
            var ranges = new List<Range>();
            var lines = File.ReadAllLines(inputFilePath);
            var rangeDefs = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (string rangeDef in rangeDefs)
            {
                var parts = rangeDef.Split('-', StringSplitOptions.RemoveEmptyEntries);
                var range = new Range(long.Parse(parts[0]), long.Parse(parts[1]));
                ranges.Add(range);
            }
            return ranges;
        }

        private static long SolvePart1(List<Range> ranges)
        {
            long result = 0;
            foreach (var range in ranges)
                for (long number = range.Start; number <= range.End; ++number)
                    if (IsInvalidId_Part1(number))
                        result += number;
            return result;
        }

        private static long SolvePart2(List<Range> ranges)
        {
            long result = 0;
            foreach (var range in ranges)
                for (long number = range.Start; number <= range.End; ++number)
                    if (IsInvalidId_Part2(number))
                        result += number;
            return result;
        }

        /// <summary>
        /// Checks if <paramref name="number"/> is an invalid ID according to part 1 rules.
        /// </summary>
        /// <returns>True if <paramref name="number"/> is a sequence of digits repeated twice, false otherwise.</returns>
        private static bool IsInvalidId_Part1(long number)
        {
            string numberStr = number.ToString();
            
            if (numberStr.Length %2 != 0)
                return false; // an invalid ID must have an even number of digits, by definition

            int midPos = numberStr.Length / 2;
            for (int i = 0; i < midPos; ++i)
            {
                if (numberStr[i] != numberStr[midPos + i])
                    return false;
            }
            
            return true;
        }

        /// <summary>
        /// Checks if <paramref name="number"/> is an invalid ID according to part 2 rules.
        /// </summary>
        /// <returns>True if <paramref name="number"/> is a sequence of digits repeated at least twice, false otherwise.</returns>
        private static bool IsInvalidId_Part2(long number)
        {
            string numberStr = number.ToString();

            var possibleDivisors = GetDivisors(numberStr.Length);
            foreach (int nbReps in possibleDivisors)
            {
                // we try to divide the number into 'nbReps' sequences
                
                if (nbReps == 1)
                    continue;

                int sequenceLength = numberStr.Length / nbReps;

                if (IsComposedOfRepeatedSequence(numberStr, nbReps, sequenceLength))
                    return true;
            }

            return false; // no split of 'number' led to a repetition
        }

        private static bool IsComposedOfRepeatedSequence(string s, int nbReps, int sequenceLength)
        {
            string sequence = s[..sequenceLength];
            for (int i = 1; i < nbReps; ++i)
            {
                string nextSeq = s.Substring(i * sequenceLength, sequenceLength);
                if (nextSeq != sequence)
                    return false;
            }
            return true;
        }

        private static List<int> GetDivisors(int number)
        {
            var divisors = new List<int>();
            for (int i = 1; i <= Math.Sqrt(number); ++i)
            {
                if (number % i == 0)
                {
                    divisors.Add(i);
                    if (i != number / i)
                        divisors.Add((int)(number / i));
                }
            }
            divisors.Sort();
            return divisors;
        }
    }
}
