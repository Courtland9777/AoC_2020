using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Helpers;

namespace AoC_2020.Day9
{
    public static class EncodingError
    {
        public static void Day9()
        {
            var path = $"{SD.Path}9{SD.Ext}";
            const int preambleLength = 25;
            var offset = 0;

            var cypheredData = File.ReadLines(path).Select(long.Parse).ToArray();
            
            for (var currentIndexPosition = preambleLength; currentIndexPosition < cypheredData.Length; currentIndexPosition++)
            {
                var isSumPresent = IsSumPresent(cypheredData.Skip(offset).Take(preambleLength).ToArray(),
                    cypheredData[currentIndexPosition], 2);
                if (!isSumPresent)
                {
                    var invalidNumber = cypheredData[currentIndexPosition];
                    Console.WriteLine($"First number without matching property = {invalidNumber}");
                    Console.WriteLine($"Encryption weakness in list of numbers = {GetDay9Part2(cypheredData, invalidNumber)}");
                    break;
                }

                offset++;
            }
        }

        private static long GetDay9Part2(IReadOnlyList<long> arrayToCheck, long invalidNumber)
        {
            var numStack = new Stack<long>();
            long result = 0;
            for (var i = 0; i < arrayToCheck.Count; i++)
            {
                numStack.Push(arrayToCheck[i]);
                for (var j = i+1; j < arrayToCheck.Count; j++)
                {
                    numStack.Push(arrayToCheck[j]);
                    var total = numStack.Sum();
                    if (total > invalidNumber) break;
                    if (total != invalidNumber) continue;
                    result = numStack.Min() + numStack.Max();
                    break;
                }
                numStack.Clear();
            }

            return result;
        }
        private static bool IsSumPresent(long[] arrayToCheck, long currentIndexValue, int numberOfElementsToSum) =>
             EnumerableCombinations.GetCombinations(arrayToCheck, numberOfElementsToSum).Any(combination => combination.Sum() == currentIndexValue);
    }
}
