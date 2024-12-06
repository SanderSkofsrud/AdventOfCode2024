using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventSolutions
{
    /// <summary>
    /// Day 2 solution: Checking if reports are safe or can be made safe by removing one level.
    /// </summary>
    public static class Day2
    {
        /// <summary>
        /// Checks if a report is safe: differences between consecutive elements are all in [-3, -1] or [1, 3].
        /// </summary>
        private static bool IsSafeReport(List<int> report)
        {
            if (report.Count < 2) return true; // Trivially safe if not enough data.

            var differences = report.Zip(report.Skip(1), (a, b) => b - a);

            bool allDecreasing = differences.All(d => d >= -3 && d <= -1);
            bool allIncreasing = differences.All(d => d >= 1 && d <= 3);

            return allDecreasing || allIncreasing;
        }

        /// <summary>
        /// Checks if a report can be made safe by removing zero or one element.
        /// </summary>
        private static bool IsSafeWithDampener(List<int> report)
        {
            if (IsSafeReport(report)) return true;

            for (int i = 0; i < report.Count; i++)
            {
                var modified = new List<int>(report);
                modified.RemoveAt(i);
                if (IsSafeReport(modified))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Count the number of safe reports from a file, with optional dampener logic.
        /// </summary>
        private static int CountSafeReports(string filename, bool withDampener = false)
        {
            int safeCount = 0;
            foreach (var line in File.ReadLines(filename))
            {
                var report = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                 .Select(int.Parse)
                                 .ToList();
                bool safe = withDampener ? IsSafeWithDampener(report) : IsSafeReport(report);
                if (safe) safeCount++;
            }
            return safeCount;
        }

        public static void Run()
        {
            string filePath = "../data/Day2.txt";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Day2 data file not found.");
                return;
            }

            // Part 1
            int safeReportsCount = CountSafeReports(filePath, withDampener: false);
            Console.WriteLine($"Day 2 - Part 1: Number of safe reports: {safeReportsCount}");

            // Part 2
            int safeReportsWithDampener = CountSafeReports(filePath, withDampener: true);
            Console.WriteLine($"Day 2 - Part 2: Number of safe reports with Dampener: {safeReportsWithDampener}");
        }
    }
}
