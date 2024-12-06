using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventSolutions
{
    /// <summary>
    /// Day 3 solution: Parsing "mul(x,y)" instructions, and handling "do()" / "don't()" toggles.
    /// </summary>
    public static class Day3
    {
        /// <summary>
        /// Extract and sum all mul(X,Y) instructions from memory.
        /// </summary>
        private static int ExtractAndSumMulInstructions(string memory)
        {
            var pattern = @"mul\(\d+,\d+\)";
            var matches = Regex.Matches(memory, pattern);

            int total = 0;
            foreach (Match match in matches)
            {
                var numbers = Regex.Matches(match.Value, @"\d+").Select(m => int.Parse(m.Value)).ToArray();
                if (numbers.Length == 2)
                {
                    total += numbers[0] * numbers[1];
                }
            }
            return total;
        }

        /// <summary>
        /// Extract mul instructions, respecting do() and don't() toggles.
        /// </summary>
        private static int ExtractAndSumMulInstructionsWithConditions(string memory)
        {
            var mulPattern = @"mul\(\d+,\d+\)";
            var controlPattern = @"do\(\)|don't\(\)";

            var mulMatches = Regex.Matches(memory, mulPattern).Cast<Match>();
            var controlMatches = Regex.Matches(memory, controlPattern).Cast<Match>();

            var allMatches = mulMatches.Concat(controlMatches).OrderBy(m => m.Index).ToList();

            int total = 0;
            bool mulEnabled = true;

            foreach (var match in allMatches)
            {
                string text = match.Value;
                if (text == "do()")
                {
                    mulEnabled = true;
                }
                else if (text == "don't()")
                {
                    mulEnabled = false;
                }
                else if (mulEnabled && text.StartsWith("mul("))
                {
                    var numbers = Regex.Matches(text, @"\d+").Select(m => int.Parse(m.Value)).ToArray();
                    if (numbers.Length == 2)
                        total += numbers[0] * numbers[1];
                }
            }

            return total;
        }

        public static void Run()
        {
            string filePath = "../data/Day3.txt";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Day3 data file not found.");
                return;
            }

            string memory = File.ReadAllText(filePath);

            // Part 1
            int part1Result = ExtractAndSumMulInstructions(memory);
            Console.WriteLine($"Day 3 - Part 1: Total sum of valid mul instructions: {part1Result}");

            // Part 2
            int part2Result = ExtractAndSumMulInstructionsWithConditions(memory);
            Console.WriteLine($"Day 3 - Part 2: Total sum of enabled mul instructions: {part2Result}");
        }
    }
}
