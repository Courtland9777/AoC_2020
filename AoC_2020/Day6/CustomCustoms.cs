using AoC_2020.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2020.Day6
{
    public static class CustomCustoms
    {
        public static void Day6()
        {
            var path = $"{SD.Path}6{SD.Ext}";
            var rawCustomsForms = File.ReadLines(path);
            var customsForms = ConcatImportData.ConcatPassportData(
                rawCustomsForms).Select(x => x.Replace(" ", string.Empty));

            Console.WriteLine("Day 6");
            Console.WriteLine($"Number of questions = {GetDay6Part1(customsForms)}");
            Console.WriteLine($"Number of questions corrected = {GetDay6Part2(rawCustomsForms)}");
        }

        private static int GetDay6Part1(IEnumerable<string> customsForms) =>
            customsForms.Sum(f => (f.Distinct().Count()));

        private static int GetDay6Part2(IEnumerable<string> rawCustomsForms)
        {
            var tempStack = new Stack<string>();
            var yesCount = 0;
            foreach (var rawCustomsForm in rawCustomsForms)
            {
                if (string.IsNullOrEmpty(rawCustomsForm))
                {
                    yesCount += GetMatches(tempStack);
                    tempStack.Clear();
                    continue;
                }
                tempStack.Push(rawCustomsForm);
            }

            return yesCount + GetMatches(tempStack);
        }

        private static int GetMatches(IReadOnlyCollection<string> tempStack) =>
            string.Concat(tempStack)
                .GroupBy(c => c).Select(c =>
                    new
                    {
                        Char = c.Key,
                        chCount = c.Count()
                    })
                .Count(x => x.chCount == tempStack.Count);
    }
}
