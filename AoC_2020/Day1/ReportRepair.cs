using AoC_2020.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2020.Day1
{
    public static class ReportRepair
    {
        public static void Day1()
        {
            var path = $"{SD.Path}1{SD.Ext}";

            var expenseReport = File.ReadLines(path).Select(int.Parse).OrderBy(x => x);
            Console.WriteLine("Day 1");
            Console.WriteLine(GetDay1Part1(expenseReport));
            Console.WriteLine(GetDay1Part2(expenseReport));
        }
        private static string GetDay1Part2(IEnumerable<int> expenseReport)
        {
            var result = string.Empty;
            foreach (var combination in EnumerableCombinations.GetCombinations(expenseReport.ToArray(), 3))
            {
                if (combination[0] + combination[1] + combination[2] != 2020) continue;
                result = $"Part 2 Product of combinations = {combination[0] * combination[1] * combination[2]}";
                break;
            }

            return result;
        }

        private static string GetDay1Part1(IOrderedEnumerable<int> expenseReport)
        {
            var result = string.Empty;
            foreach (var expense in expenseReport)
            {
                if (!expenseReport.Contains(2020 - expense)) continue;
                result = $"Part 1 = {(2020 - expense) *expense}";
                break;
            }

            return result;
        }
    }
}
