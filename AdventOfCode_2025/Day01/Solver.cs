namespace AdventOfCode_2025.Day01
{
    record Rotation (char Direction, int Distance);

    internal class Solver
    {
        internal static void Solve(string inputFilePath)
        {
            var rotations = ParseInput(inputFilePath);

            Console.WriteLine($"Day 1 part 1: {SolvePart1(rotations)}");
            Console.WriteLine($"Day 1 part 2: {SolvePart2(rotations)}");
        }

        private static List<Rotation> ParseInput(string inputFilePath)
        {
            var lines = File.ReadAllLines(inputFilePath);
            var rotations = new List<Rotation>();
            foreach (var line in lines)
            {
                char direction = line[0];
                int distance = int.Parse(line.Substring(1));
                rotations.Add(new Rotation(direction, distance));
            }
            return rotations;
        }

        private static int SolvePart1(List<Rotation> rotations)
        {
            int currentPos = 50;
            int result = 0;

            foreach (var r in rotations)
            {
                currentPos += r.Direction == 'R' ? r.Distance : -r.Distance;

                // bring back 'currentPos' into [0,100) if needed
                while (currentPos < 0)
                    currentPos += 100;
                while (currentPos >= 100)
                    currentPos -= 100;
                
                if (currentPos == 0)
                    ++result;
            }

            return result;
        }

        private static int SolvePart2(List<Rotation> rotations)
        {
            int currentPos = 50;
            int result = 0;

            foreach (var r in rotations)
            {
                result += CountCrossings(currentPos, r);

                currentPos += r.Direction == 'R' ? r.Distance : -r.Distance;
                
                // bring back 'currentPos' into [0,100) if needed
                while (currentPos < 0)
                    currentPos += 100;
                while (currentPos >= 100)
                    currentPos -= 100;
            }

            return result;
        }

        private static int CountCrossings(int currentPos, Rotation rotation)
        {
            int nbCrossings = 0;
            int distanceToApply = rotation.Distance;
            
            int remainingDistanceToZero = rotation.Direction == 'R' ? 100 - currentPos : currentPos;
            if (currentPos != 0 && distanceToApply >= remainingDistanceToZero)
            {
                nbCrossings++;
                distanceToApply -= remainingDistanceToZero;
            }

            nbCrossings += distanceToApply / 100;

            return nbCrossings;
        }
    }
}
