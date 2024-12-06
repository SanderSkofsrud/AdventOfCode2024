using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AdventSolutions
{
    /// <summary>
    /// Day 1 solution translated to C#, handling calculation of total distance and similarity score.
    /// </summary>
    public static class Day1
    {
        /// <summary>
        /// Reads pairs of integers from a file into two lists.
        /// </summary>
        private static (List<int> LeftList, List<int> RightList) ReadListsFromFile(string filename)
        {
            var leftList = new List<int>();
            var rightList = new List<int>();

            foreach (var line in File.ReadLines(filename))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && int.TryParse(parts[0], out int left) && int.TryParse(parts[1], out int right))
                {
                    leftList.Add(left);
                    rightList.Add(right);
                }
            }

            return (leftList, rightList);
        }

        /// <summary>
        /// Calculate the total distance between corresponding elements of two lists after sorting them.
        /// </summary>
        private static int CalculateTotalDistance(List<int> leftList, List<int> rightList)
        {
            var leftSorted = leftList.OrderBy(x => x).ToList();
            var rightSorted = rightList.OrderBy(x => x).ToList();

            return leftSorted.Zip(rightSorted, (l, r) => Math.Abs(l - r)).Sum();
        }

        /// <summary>
        /// Calculate similarity score defined as sum of (element * frequency_of_element_in_other_list).
        /// </summary>
        private static int CalculateSimilarityScore(List<int> leftList, List<int> rightList)
        {
            var rightCount = rightList.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

            int similarityScore = 0;
            foreach (var num in leftList)
            {
                if (rightCount.TryGetValue(num, out int count))
                {
                    similarityScore += num * count;
                }
            }

            return similarityScore;
        }

        public static void Run()
        {
            string filePath = "../data/Day1.txt";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Day1 data file not found.");
                return;
            }

            var (leftList, rightList) = ReadListsFromFile(filePath);

            // Part 1
            int totalDistance = CalculateTotalDistance(leftList, rightList);
            Console.WriteLine($"Day 1 - Part 1: Total distance: {totalDistance}");

            // Part 2
            int similarityScore = CalculateSimilarityScore(leftList, rightList);
            Console.WriteLine($"Day 1 - Part 2: Similarity score: {similarityScore}");
        }
    }
}
