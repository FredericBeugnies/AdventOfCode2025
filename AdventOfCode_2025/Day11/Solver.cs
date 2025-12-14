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
            int youIdx = nodeIndex["you"];
            int outIdx = nodeIndex["out"];
            Console.WriteLine($"Day 11 part 1: {ComputeNbPaths(adjacency, youIdx, outIdx)}");

            // solve part 2
            int svrIdx = nodeIndex["svr"];
            int dacIdx = nodeIndex["dac"];
            int fftIdx = nodeIndex["fft"];
            long svr_dac = ComputeNbPaths(adjacency, svrIdx, dacIdx);
            long svr_fft = ComputeNbPaths(adjacency, svrIdx, fftIdx);
            long dac_fft = ComputeNbPaths(adjacency, dacIdx, fftIdx);
            long fft_dac = ComputeNbPaths(adjacency, fftIdx, dacIdx);
            long dac_out = ComputeNbPaths(adjacency, dacIdx, outIdx);
            long fft_out = ComputeNbPaths(adjacency, fftIdx, outIdx);
            long res_part2 = svr_dac * dac_fft * fft_out + svr_fft * fft_dac * dac_out;
            Console.WriteLine($"Day 11 part 2: {res_part2}");
        }

        private static long ComputeNbPaths(bool[,] adjacency, int from, int to)
        {
            int nbNodes = adjacency.GetLength(0);
            long[,] nbPaths = new long[nbNodes, nbNodes]; // nbPaths[j, k] = number of paths from node 'from' to node 'j' of length 'k'

            for (int j = 0; j < nbNodes; ++j)
                nbPaths[j, 1] = adjacency[from, j] ? 1 : 0;

            for (int k = 2; k < nbNodes; ++k)
                for (int j = 0; j < nbNodes; ++j)
                    for (int m = 0; m < nbNodes; ++m)
                        if (adjacency[m, j])
                            nbPaths[j, k] += nbPaths[m, k - 1];

            long res = 0;
            for (int k = 0; k < nbNodes; ++k)
                res += nbPaths[to, k];

            return res;
        }
    }
}
