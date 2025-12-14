using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2025.Day10
{
    internal class Machine
    {
        public required bool[] IndicatorLightsDiagram { get; set; }

        public required bool[][] WiringSchematics { get; set; }
    }

    internal class ButtonCombination
    {
        public int NbButtonPresses { get; set; }

        public required bool[] ButtonPresses { get; set; }
    }

    internal class Solver
    {
        public static void Solve(string inputFilePath)
        {
            // read input
            var lines = File.ReadAllLines(inputFilePath);

            var machines = new List<Machine>();
            int maxNbButtons = 0;
            foreach (var line in lines)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var indicators = ParseIndicators(parts[0]);
                var wiringSchematics = parts.Skip(1).SkipLast(1).Select(x => ParseWiringSchematics(x, indicators.Length)).ToArray();
                maxNbButtons = Math.Max(maxNbButtons, wiringSchematics.Length);

                machines.Add(new Machine
                {
                    IndicatorLightsDiagram = indicators, 
                    WiringSchematics = wiringSchematics
                });
                Console.WriteLine($"Line: {indicators.Length} indicators {wiringSchematics.Length} schematics");
            }

            // compute all lists of possible combinations
            var combos = new Dictionary<int, List<ButtonCombination>>();
            combos[0] = new List<ButtonCombination>
            {
                new ButtonCombination
                {
                    NbButtonPresses = 0,
                    ButtonPresses = []
                }
            };
            for (int i = 1; i <= maxNbButtons; ++i)
            {
                // compute combinations of 'i' buttons
                combos[i] = new List<ButtonCombination>();
                foreach (var combo in combos[i-1])
                {
                    var newCombo0 = new ButtonCombination
                    {
                        NbButtonPresses = combo.NbButtonPresses,
                        ButtonPresses = new bool[i]
                    };
                    var newCombo1 = new ButtonCombination
                    {
                        NbButtonPresses = combo.NbButtonPresses + 1,
                        ButtonPresses = new bool[i]
                    };
                    Array.Copy(combo.ButtonPresses, newCombo0.ButtonPresses, i - 1);
                    Array.Copy(combo.ButtonPresses, newCombo1.ButtonPresses, i - 1);
                    newCombo0.ButtonPresses[i - 1] = false;
                    newCombo1.ButtonPresses[i - 1] = true;
                    combos[i].Add(newCombo0);
                    combos[i].Add(newCombo1);
                }
                combos[i] = combos[i].OrderBy(x => x.NbButtonPresses).ToList();
            }

            int totalNbButtonPresses = 0;
            foreach (var machine in machines)
            {
                var comboList = combos[machine.WiringSchematics.Length];
                foreach (var combo in comboList)
                {
                    bool[] comboResult = new bool[machine.IndicatorLightsDiagram.Length];

                    // apply button combo
                    for (int i = 0; i < combo.ButtonPresses.Length; ++i)
                    {
                        if (combo.ButtonPresses[i])
                        {
                            bool[] wiring = machine.WiringSchematics[i];
                            for (int j = 0; j < wiring.Length; ++j)
                            {
                                comboResult[j] ^= wiring[j];
                            }
                        }
                    }

                    if (Check(comboResult, machine.IndicatorLightsDiagram))
                    {
                        totalNbButtonPresses += combo.NbButtonPresses;
                        break;
                    }
                }
            }

            Console.WriteLine($"Day 10 part 1: {totalNbButtonPresses}");
        }

        private static bool Check(bool[] a1, bool[] a2)
        {
            if (a1.Length != a2.Length)
                return false;
            for (int i = 0; i < a1.Length; ++i)
                if (a1[i] != a2[i])
                    return false;
            return true;
        }

        private static bool[] ParseIndicators(string inputStr)
        {
            var res = new bool[inputStr.Length - 2];
            for (int i = 0; i < inputStr.Length - 2; i++)
                res[i] = inputStr[i + 1] == '#';
            return res;
        }

        private static bool[] ParseWiringSchematics(string inputStr, int nbIndicators)
        {
            var res = new bool[nbIndicators];
            var parts = inputStr[1..^1].Split(',', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
                res[int.Parse(parts[i])] = true;
            return res;
        }

    }
}
