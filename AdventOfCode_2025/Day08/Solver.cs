using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2025.Day08
{
    internal class Box
    {
        public int X;
        public int Y;
        public int Z;
        public int Index;
        public int Circuit;
    };

    internal class BoxPair
    {
        public required Box B1;
        public required Box B2;
        public double Distance;
    };

    internal class Solver
    {
        public static void Solve(string inputFilePath)
        {
            var lines = File.ReadAllLines(inputFilePath);
            var boxes = new List<Box>();
            var circuits = new Dictionary<int, List<Box>>();
            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',', StringSplitOptions.TrimEntries);
                var box = new Box
                {
                    X = int.Parse(parts[0]),
                    Y = int.Parse(parts[1]),
                    Z = int.Parse(parts[2]),
                    Index = i,
                    Circuit = i
                };
                boxes.Add(box);
                circuits[i] = new List<Box> { box };
            }

            var boxPairs = new List<BoxPair>();
            for (int i = 0; i < boxes.Count; i++)
                for (int j = i + 1; j < boxes.Count; j++)
                    boxPairs.Add(new BoxPair
                    {
                        B1 = boxes[i],
                        B2 = boxes[j],
                        Distance = Distance(boxes[i], boxes[j])
                    });
            boxPairs = boxPairs.OrderBy(bp => bp.Distance).ToList();

            for (int i = 0; i < 1000; ++i)
            {
                var box1 = boxPairs[i].B1;
                var box2 = boxPairs[i].B2;
                Console.WriteLine($"Boxes {box1.Index} and {box2.Index} (dist. {boxPairs[i].Distance})");
                if (box1.Circuit != box2.Circuit)
                {
                    var circuit1 = circuits[box1.Circuit];
                    var circuit2 = circuits[box2.Circuit];
                    if (circuit1.Count < circuit2.Count)
                    {
                        int circuitToRemove = box1.Circuit;
                        for (int k = 0; k < circuit1.Count; ++k)
                        {
                            circuit1[k].Circuit = box2.Circuit;
                            circuit2.Add(circuit1[k]);
                        }
                        circuits.Remove(circuitToRemove);
                    }
                    else
                    {
                        int circuitToRemove = box2.Circuit;
                        for (int k = 0; k < circuit2.Count; ++k)
                        {
                            circuit2[k].Circuit = box1.Circuit;
                            circuit1.Add(circuit2[k]);
                        }
                        circuits.Remove(circuitToRemove);
                    }
                }
            }

            var circuitSizes = circuits.Values.Select(c => c.Count).OrderByDescending(c => c).ToList();
            long result = circuitSizes[0] * circuitSizes[1] * circuitSizes[2];

            Console.WriteLine($"Day 8 part 1: {result}");

            // part 2
            int ii = 1000;
            int lastPairMerged = 0;
            while (circuits.Count > 1)
            {
                var box1 = boxPairs[ii].B1;
                var box2 = boxPairs[ii].B2;
                //Console.WriteLine($"Boxes {box1.Index} and {box2.Index} (dist. {boxPairs[ii].Distance})");
                if (box1.Circuit != box2.Circuit)
                {
                    lastPairMerged = ii;
                    var circuit1 = circuits[box1.Circuit];
                    var circuit2 = circuits[box2.Circuit];
                    if (circuit1.Count < circuit2.Count)
                    {
                        int circuitToRemove = box1.Circuit;
                        for (int k = 0; k < circuit1.Count; ++k)
                        {
                            circuit1[k].Circuit = box2.Circuit;
                            circuit2.Add(circuit1[k]);
                        }
                        circuits.Remove(circuitToRemove);
                    }
                    else
                    {
                        int circuitToRemove = box2.Circuit;
                        for (int k = 0; k < circuit2.Count; ++k)
                        {
                            circuit2[k].Circuit = box1.Circuit;
                            circuit1.Add(circuit2[k]);
                        }
                        circuits.Remove(circuitToRemove);
                    }
                }
                ++ii;
            }

            long resultPart2 = (long)boxPairs[lastPairMerged].B1.X * (long)boxPairs[lastPairMerged].B2.X;
            Console.WriteLine($"Day 8 part 2: {resultPart2}");
        }

        internal static double Distance(Box p1, Box p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
        }
    }
}
