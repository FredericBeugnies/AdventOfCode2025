namespace AdventOfCode_2025.Day09
{
    internal record Point(int X, int Y);

    internal record Segment(Point P1, Point P2);

    internal class Solver
    {
        public static void Solve(string inputFilePath)
        {
            // read input
            var lines = File.ReadAllLines(inputFilePath);

            var redTiles = new List<Point>();
            foreach (var line in lines)
            {
                var parts = line.Split(',', StringSplitOptions.TrimEntries);
                redTiles.Add(new Point(int.Parse(parts[0]), int.Parse(parts[1])));
            }

            var segments = new List<Segment>();
            for (int i = 0; i < redTiles.Count - 1; i++)
                segments.Add(new Segment(redTiles[i], redTiles[i + 1]));
            segments.Add(new Segment(redTiles[redTiles.Count - 1], redTiles[0])); // close the loop

            // compute part 1 and part 2 in the same loop
            long maxArea_part1 = 0;
            long maxArea_part2 = 0;
            for (int i = 0; i < redTiles.Count; i++)
            {
                for (int j = i + 1; j < redTiles.Count; j++)
                {
                    long area = ComputeArea(redTiles[i], redTiles[j]);
                    
                    if (area > maxArea_part1)
                        maxArea_part1 = area;

                    bool rectangleIsCrossedBySegment = false;
                    foreach (var seg in segments)
                    {
                        if (RectangleIsCrossedBySegment(redTiles[i], redTiles[j], seg))
                        {
                            rectangleIsCrossedBySegment = true;
                            break;
                        }
                    }

                    if (!rectangleIsCrossedBySegment && area > maxArea_part2)
                        maxArea_part2 = area;
                }
            }
            Console.WriteLine($"Day 9 part 1: {maxArea_part1}");
            Console.WriteLine($"Day 9 part 2: {maxArea_part2}");
        }

        private static bool IsVertical(Segment segment)
        {
            return segment.P1.X == segment.P2.X;
        }

        private static long ComputeArea(Point p1, Point p2)
        {
            return (long)(Math.Abs(p1.X - p2.X) + 1) * (long)(Math.Abs(p1.Y - p2.Y) + 1);
        }

        private static bool RectangleIsCrossedBySegment(Point rectP1, Point rectP2, Segment segment)
        {
            int rectX1 = Math.Min(rectP1.X, rectP2.X);
            int rectX2 = Math.Max(rectP1.X, rectP2.X);
            int rectY1 = Math.Min(rectP1.Y, rectP2.Y);
            int rectY2 = Math.Max(rectP1.Y, rectP2.Y);

            if (IsVertical(segment))
            {
                int segMinY = Math.Min(segment.P1.Y, segment.P2.Y);
                int segMaxY = Math.Max(segment.P1.Y, segment.P2.Y);
                if (segment.P1.X > rectX1 && segment.P1.X < rectX2 && segMinY <= rectY1 && segMaxY >= rectY2)
                {
                    return true;
                }
            }
            else // horizontal
            {
                int segMinX = Math.Min(segment.P1.X, segment.P2.X);
                int segMaxX = Math.Max(segment.P1.X, segment.P2.X);
                if (segment.P1.Y > rectY1 && segment.P1.Y < rectY2 && segMinX <= rectX1 && segMaxX >= rectX2)
                {
                    return true;
                }
            }

            if (RectangleStrictlyContainsPoint(rectX1, rectX2, rectY1, rectY2, segment.P1) ||
                RectangleStrictlyContainsPoint(rectX1, rectX2, rectY1, rectY2, segment.P2))
                return true;

            return false;
        }

        private static bool RectangleStrictlyContainsPoint(int rectX1, int rectX2, int rectY1, int rectY2, Point point)
        {
            return 
                point.X > rectX1 && 
                point.X < rectX2 && 
                point.Y > rectY1 && 
                point.Y < rectY2;
        }
    }
}
