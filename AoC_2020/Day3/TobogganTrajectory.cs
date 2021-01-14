using AoC_2020.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2020.Day3
{
    public static class TobogganTrajectory
    {
        private const int Interval = 3;

        public static void Day3()
        {
            var path = $"{SD.Path}3{SD.Ext}";
            var map = File.ReadLines(path).ToArray();
            
            Console.WriteLine("Day 3");
            Console.WriteLine($"Tree Count = {GetDay3Part1(map)}");
            Console.WriteLine($"Product of Tree Count = {GetDay3Part2(map)}");
        }
        
        private static int GetDay3Part1(IReadOnlyList<string> map) =>
            CountTrees(map, Interval);

        private static int GetDay3Part2(IReadOnlyList<string> map) =>
            CountTrees(map, 1) *
            CountTrees(map, 3) *
            CountTrees(map, 5) *
            CountTrees(map, 7) *
            CountTrees(map, 1, 2);

        private static int CountTrees(IReadOnlyList<string> map, int right, int down = 1)
        {
            var treeCount = 0;
            var counter = 0;
            var strLen = map[0].Length;

            for (var i = down; i < map.Count; i+=down)
            {
                if (!map[i][(counter += right) % strLen].Equals('#')) continue;
                treeCount++;
            }

            return treeCount;
        }

        
    }
}
