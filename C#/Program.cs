using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventSolutions
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var dayRunners = DiscoverDayRunners();

            if (dayRunners.Count == 0)
            {
                Console.WriteLine("No day solutions found.");
                return;
            }

            Console.WriteLine("Welcome to the Advent Solutions Hub!");
            Console.WriteLine("Available days:");

            foreach (var kvp in dayRunners.OrderBy(k => k.Key))
            {
                Console.WriteLine($"Day {kvp.Key}");
            }

            while (true)
            {
                Console.WriteLine("Press the day number you wish to run, or any other key to exit:");

                // Read a single key without requiring ENTER
                var keyInfo = Console.ReadKey(intercept: true);
                char keyChar = keyInfo.KeyChar;

                // Check if it's a digit
                if (char.IsDigit(keyChar))
                {
                    int dayNumber = keyChar - '0'; // Convert char '0'...'9' to int 0...9

                    if (dayRunners.TryGetValue(dayNumber, out MethodInfo runMethod))
                    {
                        Console.WriteLine($"\nRunning Day {dayNumber}...");
                        runMethod.Invoke(null, null);
                    }
                    else
                    {
                        Console.WriteLine($"\nNo solution found for Day {dayNumber}. Exiting...");
                        break;
                    }
                }
                else
                {
                    // Non-digit key pressed, exit
                    Console.WriteLine("\nInvalid input. Exiting...");
                    break;
                }
            }

            Console.WriteLine("Goodbye!");
        }

        /// <summary>
        /// Uses reflection to find all classes named "DayX" where X is an integer,
        /// and retrieves their public static Run() method.
        /// Returns a dictionary mapping the day number to the MethodInfo for Run().
        /// </summary>
        private static Dictionary<int, MethodInfo> DiscoverDayRunners()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var dayPattern = new Regex(@"^Day(\d+)$");
            var result = new Dictionary<int, MethodInfo>();

            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (!type.IsClass || !type.IsPublic) continue;

                var match = dayPattern.Match(type.Name);
                if (!match.Success) continue;

                if (int.TryParse(match.Groups[1].Value, out int dayNumber))
                {
                    var runMethod = type.GetMethod("Run", BindingFlags.Public | BindingFlags.Static);

                    if (runMethod != null && runMethod.GetParameters().Length == 0 && runMethod.ReturnType == typeof(void))
                    {
                        result[dayNumber] = runMethod;
                    }
                }
            }

            return result;
        }
    }
}
