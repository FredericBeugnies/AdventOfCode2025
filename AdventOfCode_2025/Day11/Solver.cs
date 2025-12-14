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
                nodeIndex[line[..3]] = i++;
            nodeIndex["out"] = i++;

            int nbNodes = nodeIndex.Count;
            bool[,] adjacency = new bool[nbNodes, nbNodes];
            foreach (var line in lines)
            {
                int fromIdx = nodeIndex[line[..3]];
                var neighbors = line[4..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (string neighbor in neighbors)
                    adjacency[fromIdx, nodeIndex[neighbor]] = true;
            }

            // solve part 1
            long[,] nbPaths = new long[nbNodes, nbNodes]; // nbPaths[j, k] = number of paths from start point to node 'j' of length 'k'

            int youIdx = nodeIndex["you"];
            for (int j = 0; j < nbNodes; ++j)
                nbPaths[j, 1] = adjacency[youIdx, j] ? 1 : 0;
            
            for (int k = 2; k < nbNodes; ++k)
                for (int j = 0; j < nbNodes; ++j)
                    for (int m = 0; m < nbNodes; ++m)
                        if (adjacency[m, j])
                            nbPaths[j, k] += nbPaths[m, k - 1];

            long res = 0;
            int outIdx = nodeIndex["out"];
            for (int k = 0; k < nbNodes; ++k)
                res += nbPaths[outIdx, k];
            Console.WriteLine($"Day 11 part 1: {res}");
        }
    }
}
