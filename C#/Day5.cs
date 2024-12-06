using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AdventSolutions
{
    /// <summary>
    /// Day 5 solution: Checking ordering rules on updates and topologically sorting them.
    /// </summary>
    public static class Day5
    {
        private static (List<(int, int)> Rules, List<List<int>> Updates) ParseInput(string filename)
        {
            var lines = File.ReadAllLines(filename).Select(l => l.Trim()).ToList();
            int blankIndex = lines.IndexOf("");

            var ruleLines = lines.Take(blankIndex).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var updateLines = lines.Skip(blankIndex + 1).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            var rules = new List<(int, int)>();
            foreach (var line in ruleLines)
            {
                var parts = line.Split('|', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && int.TryParse(parts[0].Trim(), out int x) && int.TryParse(parts[1].Trim(), out int y))
                {
                    rules.Add((x, y));
                }
            }

            var updates = new List<List<int>>();
            foreach (var line in updateLines)
            {
                var pages = line.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => int.Parse(s.Trim())).ToList();
                updates.Add(pages);
            }

            return (rules, updates);
        }

        private static bool IsCorrectlyOrdered(List<int> update, List<(int,int)> rules)
        {
            var pagePositions = update.Select((p, i) => (p, i)).ToDictionary(x => x.p, x => x.i);
            foreach (var (x, y) in rules)
            {
                if (pagePositions.ContainsKey(x) && pagePositions.ContainsKey(y))
                {
                    if (pagePositions[x] > pagePositions[y])
                        return false;
                }
            }
            return true;
        }

        private static List<int> TopologicalSort(HashSet<int> pages, List<(int,int)> relevantRules)
        {
            var graph = new Dictionary<int, List<int>>();
            var inDegree = new Dictionary<int, int>();
            foreach (var p in pages)
            {
                graph[p] = new List<int>();
                inDegree[p] = 0;
            }

            foreach (var (x, y) in relevantRules)
            {
                graph[x].Add(y);
                inDegree[y]++;
            }

            var queue = new Queue<int>(inDegree.Where(kv => kv.Value == 0).Select(kv => kv.Key));
            var sorted = new List<int>();
            while (queue.Count > 0)
            {
                int node = queue.Dequeue();
                sorted.Add(node);
                foreach (var neighbor in graph[node])
                {
                    inDegree[neighbor]--;
                    if (inDegree[neighbor] == 0)
                        queue.Enqueue(neighbor);
                }
            }

            return sorted;
        }

        private static int PartOne(List<(int,int)> rules, List<List<int>> updates)
        {
            int total = 0;
            foreach (var upd in updates)
            {
                if (IsCorrectlyOrdered(upd, rules))
                {
                    int middleIndex = upd.Count / 2;
                    total += upd[middleIndex];
                }
            }
            return total;
        }

        private static int PartTwo(List<(int,int)> rules, List<List<int>> updates)
        {
            int total = 0;
            var incorrectUpdates = updates.Where(upd => !IsCorrectlyOrdered(upd, rules)).ToList();

            foreach (var upd in incorrectUpdates)
            {
                var pageSet = upd.ToHashSet();
                var relevantRules = rules.Where(r => pageSet.Contains(r.Item1) && pageSet.Contains(r.Item2)).ToList();
                var sortedUpd = TopologicalSort(pageSet, relevantRules);
                int middleIndex = sortedUpd.Count / 2;
                total += sortedUpd[middleIndex];
            }

            return total;
        }

        public static void Run()
        {
            string filePath = "../data/Day5.txt";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Day5 data file not found.");
                return;
            }

            var (rules, updates) = ParseInput(filePath);

            // Part One
            int partOneResult = PartOne(rules, updates);
            Console.WriteLine("Day 5 - Part 1 Result: " + partOneResult);

            // Part Two
            int partTwoResult = PartTwo(rules, updates);
            Console.WriteLine("Day 5 - Part 2 Result: " + partTwoResult);
        }
    }
}
