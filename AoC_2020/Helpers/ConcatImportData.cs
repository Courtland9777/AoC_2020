using System.Collections.Generic;

namespace AoC_2020.Helpers
{
    public static class ConcatImportData
    {
        // Day 4 
        public static IEnumerable<string> ConcatPassportData(IEnumerable<string> rawData)
        {
            var singleStringList = new List<string>();
            var quickStack = new Stack<string>();

            foreach (var line in rawData)
            {
                if (string.IsNullOrEmpty(line))
                {
                    AddToList(ref singleStringList, quickStack);
                    quickStack.Clear();
                    continue;
                }

                quickStack.Push(line);
            }
            AddToList(ref singleStringList, quickStack);
            return singleStringList;
        }

        private static void AddToList(ref List<string> singleStringList, IEnumerable<string> quickStack) =>
            singleStringList.Add(string.Join(' ', quickStack));
    }
}
