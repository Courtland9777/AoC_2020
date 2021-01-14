using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020.Helpers
{
    public static class EnumerableCombinations
    {
        private static IEnumerable<int[]> GetCombinations(int m, int n)
        {
            var result = new int[m];
            var stack = new Stack<int>(m);
            stack.Push(0);
            while (stack.Count > 0)
            {
                var index = stack.Count - 1;
                var value = stack.Pop();
                while (value < n)
                {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index != m) continue;
                    yield return (int[])result.Clone();
                    break;
                }
            }
        }

        public static IEnumerable<T[]> GetCombinations<T>(T[] array, int m)
        {
            if (array.Length < m)
                throw new ArgumentException("Array length can't be less than number of selected elements");
            if (m < 1)
                throw new ArgumentException("Number of selected elements can't be less than 1");

            return GetCombinations2();

            IEnumerable<T[]> GetCombinations2()
            {
                var result = new T[m];
                foreach (var j in GetCombinations(m, array.Length))
                {
                    for (var i = 0; i < m; i++)
                    {
                        result[i] = array[j[i]];
                    }
                    yield return result;
                }
            }
        }
    }
}
