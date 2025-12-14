using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2025.Day11
{
    internal class Solver
    {
        public static void Solve(string inputFilePath)
        {
            // read input
            var lines = File.ReadAllLines(inputFilePath);

            var nodeIndex = new Dictionary<string, int>();
            int i = 0;
            foreach (var line in lines)
            {
                string nodeName = line[..3];
                nodeIndex[nodeName] = i++;
            }
            nodeIndex["out"] = i++;

            int nbNodes = nodeIndex.Count;
            long[,,] nbPaths = new long[nbNodes, nbNodes, nbNodes];
            foreach (var line in lines)
            {
                string fromNode = line[..3];
                int fromNodeIdx = nodeIndex[fromNode];
                var toNodes = line[4..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (string toNode in toNodes)
                {
                    int toNodeIdx = nodeIndex[toNode];
                    nbPaths[fromNodeIdx, toNodeIdx, 1] = 1;
                }
            }

            // solve part 1
            for (int k = 2; k < nbNodes; ++k)
            {
                Console.WriteLine($"k = {k}");
                for (int iNode = 0; iNode < nbNodes; ++iNode)
                {
                    for (int jNode = 0; jNode < nbNodes; ++jNode)
                    {
                        for (int mNode = 0; mNode < nbNodes; ++mNode)
                        {
                            if (nbPaths[mNode, jNode, 1] != 0)
                                nbPaths[iNode, jNode, k] += nbPaths[iNode, mNode, k - 1];
                        }
                    }
                }
            }

            long res = 0;
            int youIdx = nodeIndex["you"];
            int outIdx = nodeIndex["out"];
            for (int k = 0; k < nbNodes; ++k)
                res += nbPaths[youIdx, outIdx, k];
            Console.WriteLine($"Day 11 part 1: {res}");
        }
    }
}
