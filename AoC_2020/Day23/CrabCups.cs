using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Helpers;

namespace AoC_2020.Day23
{
    public static class CrabCups
    {
        public static void Day23()
        {
            Console.WriteLine("Day 23");
            Console.WriteLine($"Labels on cups after cup 1 = {GetDay23Part1()}");
            Console.WriteLine($"Product of labels = {GetDay23Part2()}");
        }

        private static string GetDay23Part1()
        {
            var startingNumbers = GetDay23Input();
            var finalState = CrabCupHelper.PlayCrabCups(startingNumbers, 3, 100);
            var result = CrabCupHelper.GetCanonicalCrabCupString(finalState);
            return result;
        }

        private static long GetDay23Part2()
        {
            var initialStartingNumbers = GetDay23Input();
            var startingNumbers = CrabCupHelper.GetPart2StartingNumbers(initialStartingNumbers);
            var finalState = CrabCupHelper.PlayCrabCups(startingNumbers, 3, 10000000);
            var oneIndex = finalState.IndexOf(1);
            var label1Index = (oneIndex + 1) % 1000000;
            var label2Index = (oneIndex + 2) % 1000000;
            var label1 = finalState[label1Index];
            var label2 = finalState[label2Index];
            return (long)label1 * (long)label2;
        }

        private static IList<int> GetDay23Input()
        {
            var path = $"{SD.Path}23{SD.Ext}";
            if (!File.Exists(path))
            {
                throw new Exception($"Cannot locate file {path}");
            }

            var inputLines = File.ReadAllLines(path);
            return CrabCupHelper.ParseInputLine(inputLines[0]);
        }


    }
}
