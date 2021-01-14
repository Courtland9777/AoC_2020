using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Helpers;

namespace AoC_2020.Day10
{
    public static class AdapterArray
    {
        public static void Day10()
        {
            var path = $"{SD.Path}10{SD.Ext}";
            var rawData = File.ReadLines(path);
            var adapters = BuildAdapters(rawData);

            Console.WriteLine($"Part 1 = {GetDay10Part1(adapters)}");
            Console.WriteLine($"Part 2 = {GetDay10Part2(adapters)}");
        }

        private static long GetDay10Part1(ImmutableSortedSet<int> adapters)
        {
            var rangeCount = new List<int>();
            for (var a = 0; a < adapters.Count - 1; a++)
            {
                rangeCount.Add(adapters[a + 1] - adapters[a]);
            }
            return rangeCount.Count(o => o == 1) * rangeCount.Count(t => t == 3);
        }

        private static long GetDay10Part2(ImmutableSortedSet<int> adapters)
        {
            var oneRunLengths = new List<int>();
            var consecutiveOnesCount = 0;

            for (var i = 0; i < adapters.Count-1; i++)
            {
                if (adapters[i + 1] - adapters[i] == 1)
                {
                    consecutiveOnesCount++;
                }
                else
                {
                    consecutiveOnesCount--;
                    if (consecutiveOnesCount >= 1)
                    {
                        oneRunLengths.Add(consecutiveOnesCount);
                    }

                    consecutiveOnesCount = 0;
                }
            }

            int[] runCombinations = {1, 2, 4, 7};
            return oneRunLengths.Aggregate<int, long>(1, (current, length) => current * runCombinations[length]);
        }

        private static ImmutableSortedSet<int> BuildAdapters(IEnumerable<string> data)
        {
            var list = new List<int>
            {
                0
            };
            list.AddRange(data.Select(int.Parse));
            list.Add(list.Max()+3);
            return list.OrderBy(x => x).ToImmutableSortedSet();
        }
    }
}
