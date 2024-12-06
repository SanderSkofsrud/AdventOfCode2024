using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventSolutions
{
    /// <summary>
    /// Day 4 solution: Counting occurrences of "XMAS" and a special X-MAS pattern.
    /// </summary>
    public static class Day4
    {
        /// <summary>
        /// Count occurrences of a word in 8 directions in a grid.
        /// </summary>
        private static int CountWordOccurrences(List<string> grid, string word)
        {
            int rows = grid.Count;
            int cols = grid[0].Length;
            int wordLength = word.Length;

            var directions = new (int dr, int dc)[]
            {
                (0,1), (0,-1), (1,0), (-1,0), (1,1), (-1,-1), (1,-1), (-1,1)
            };

            bool IsValid(int r, int c) => r >= 0 && r < rows && c >= 0 && c < cols;

            int totalCount = 0;

            foreach (int r in Enumerable.Range(0, rows))
            {
                foreach (int c in Enumerable.Range(0, cols))
                {
                    foreach (var (dr, dc) in directions)
                    {
                        bool match = true;
                        for (int i = 0; i < wordLength; i++)
                        {
                            int nr = r + i * dr;
                            int nc = c + i * dc;
                            if (!IsValid(nr, nc) || grid[nr][nc] != word[i])
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match) totalCount++;
                    }
                }
            }

            return totalCount;
        }

        /// <summary>
        /// Count occurrences of a special X-MAS pattern around 'A'.
        /// </summary>
        private static int CountXmasPattern(List<string> grid)
        {
            int rows = grid.Count;
            int cols = grid[0].Length;

            bool IsValid(int r, int c) => r >= 0 && r < rows && c >= 0 && c < cols;

            var patterns = new List<List<(int dr, int dc, char ch)>>
            {
                new() {(-1,-1,'M'),(-1,1,'M'),(1,-1,'S'),(1,1,'S')},
                new() {(-1,-1,'S'),(-1,1,'S'),(1,-1,'M'),(1,1,'M')},
                new() {(-1,-1,'S'),(-1,1,'M'),(1,-1,'S'),(1,1,'M')},
                new() {(-1,-1,'M'),(-1,1,'S'),(1,-1,'M'),(1,1,'S')}
            };

            int total_count = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (grid[r][c] != 'A')
                        continue;

                    foreach (var pattern in patterns)
                    {
                        bool match = true;
                        foreach (var (dr, dc, ch) in pattern)
                        {
                            int nr = r + dr;
                            int nc = c + dc;
                            if (!IsValid(nr, nc) || grid[nr][nc] != ch)
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match) total_count++;
                    }
                }
            }

            return total_count;
        }

        public static void Run()
        {
            string filePath = "../data/Day4.txt";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Day4 data file not found.");
                return;
            }

            var grid = File.ReadAllLines(filePath).ToList();

            // Part 1
            var word = "XMAS";
            int part1Result = CountWordOccurrences(grid, word);
            Console.WriteLine($"Day 4 - Part 1: Total occurrences of '{word}': {part1Result}");

            // Part 2
            int part2Result = CountXmasPattern(grid);
            Console.WriteLine($"Day 4 - Part 2: Total occurrences of X-MAS pattern: {part2Result}");
        }
    }
}
