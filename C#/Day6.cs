using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventSolutions
{
    /// <summary>
    /// Day 6 solution: Simulating guard patrol until leaving the map or detecting loops when adding new obstacles.
    /// </summary>
    public static class Day6
    {
        private static char TurnRight(char direction)
        {
            return direction switch
            {
                '^' => '>',
                '>' => 'v',
                'v' => '<',
                '<' => '^',
                _ => direction
            };
        }

        private static (int x, int y) ForwardPos(int x, int y, char direction)
        {
            return direction switch
            {
                '^' => (x - 1, y),
                'v' => (x + 1, y),
                '<' => (x, y - 1),
                '>' => (x, y + 1),
                _ => (x, y)
            };
        }

        /// <summary>
        /// Simulate the patrol until leaving the map, returning visited positions.
        /// </summary>
        private static (HashSet<(int,int)> visited, bool leftMap) SimulatePatrol(List<char[]> grid, int startX, int startY, char startDir)
        {
            int rows = grid.Count;
            int cols = grid[0].Length;
            char direction = startDir;
            int x = startX, y = startY;

            var visited = new HashSet<(int,int)>();
            visited.Add((x,y));

            while (true)
            {
                var (fx, fy) = ForwardPos(x, y, direction);
                if (fx < 0 || fx >= rows || fy < 0 || fy >= cols)
                {
                    // Leaves the map
                    return (visited, true);
                }

                if (grid[fx][fy] == '#')
                {
                    direction = TurnRight(direction);
                }
                else
                {
                    x = fx;
                    y = fy;
                    visited.Add((x, y));
                }
            }
        }

        /// <summary>
        /// Simulate patrol and detect loop if guard revisits a (x, y, direction) state.
        /// Returns true if guard leaves map, false if stuck in a loop.
        /// </summary>
        private static bool SimulatePatrolWithLoopCheck(List<char[]> grid, int startX, int startY, char startDir)
        {
            int rows = grid.Count;
            int cols = grid[0].Length;
            char direction = startDir;
            int x = startX, y = startY;

            var visitedStates = new HashSet<(int, int, char)>();
            visitedStates.Add((x, y, direction));

            while (true)
            {
                var (fx, fy) = ForwardPos(x, y, direction);
                if (fx < 0 || fx >= rows || fy < 0 || fy >= cols)
                {
                    // Leaves map
                    return true;
                }

                if (grid[fx][fy] == '#')
                {
                    direction = TurnRight(direction);
                }
                else
                {
                    x = fx;
                    y = fy;
                }

                var state = (x, y, direction);
                if (visitedStates.Contains(state))
                {
                    return false; // loop
                }
                visitedStates.Add(state);
            }
        }

        public static void Run()
        {
            string filePath = "../data/Day6.txt";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Day6 data file not found.");
                return;
            }

            var grid = File.ReadAllLines(filePath).Select(line => line.ToCharArray()).ToList();

            int rows = grid.Count;
            int cols = rows > 0 ? grid[0].Length : 0;

            // Find guard start position and direction
            var directions = new char[] { '^', '>', 'v', '<' };
            int startX = -1, startY = -1;
            char startDir = '^';

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (directions.Contains(grid[i][j]))
                    {
                        startX = i;
                        startY = j;
                        startDir = grid[i][j];
                        break;
                    }
                }
                if (startX != -1) break;
            }

            // Replace the starting symbol with '.'
            grid[startX][startY] = '.';

            // Part One
            var (visited, _) = SimulatePatrol(grid, startX, startY, startDir);
            Console.WriteLine("Day 6 - Part 1 Result: " + visited.Count);

            // Part Two
            // Try placing an obstacle in each '.' position (except the start) and check for loops.
            int loopCount = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == startX && j == startY)
                        continue;

                    if (grid[i][j] == '.')
                    {
                        grid[i][j] = '#';
                        bool leftMap = SimulatePatrolWithLoopCheck(grid, startX, startY, startDir);
                        if (!leftMap)
                            loopCount++;
                        grid[i][j] = '.'; // revert
                    }
                }
            }

            Console.WriteLine("Day 6 - Part 2 Result: " + loopCount);
        }
    }
}
