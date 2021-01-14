using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Helpers;
using MoreLinq;

namespace AoC_2020.Day17
{
    public static class ConwayCubes
    {
        public static void Day17()
        {
            var path = $"{SD.Path}17{SD.Ext}";
            var input = File.ReadLines(path).ToList();

            Console.WriteLine("Day 17");
            Console.WriteLine($"Active cubes left after cycle 6 in 3D = {GetDay17Part1(input)}");
            Console.WriteLine($"Active cubes left after cycle 6 in 4D = {GetDay17Part2(input)}");
        }

        private static int GetDay17Part1(IReadOnlyList<string> lines)
        {
            var kernel3D = Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                .SelectMany(y => Enumerable.Range(-1, 3)
                .Select(z => (x, y, z))))
                .Where(coords => coords != (0, 0, 0));
            
            var state = new Dictionary<(int x, int y, int z), bool>();

            for (var x = 0; x < lines.Count; x++)
            {
                for (var y = 0; y < lines[x].Length; y++)
                {
                    state[(x, y, 0)] = lines[x][y] == '#';
                }
            }

            var count = new Dictionary<(int x, int y, int z), int>();
            for (var i = 0; i < 6; i++)
            {
                count.Clear();
                state.Keys.ForEach(k => count[k] = 0);

                foreach (var ((x, y, z), alive) in state.Where(kvp => kvp.Value))
                {
                    foreach (var (dx, dy, dz) in kernel3D)
                    {
                        count[(x + dx, y + dy, z + dz)] =
                            count.GetValueOrDefault((x + dx, y + dy, z + dz)) + 1;
                    }
                }

                foreach (var (p, c) in count)
                {
                    state[p] = (state.GetValueOrDefault(p), c) switch
                    {
                        (true, >= 2 and <= 3) => true,
                        (false, 3) => true,
                        _ => false
                    };
                }
            }

            return state.Values.Count(x => x);
        }

        private static int GetDay17Part2(IReadOnlyList<string> lines)
        {
            var kernel4D = Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                .SelectMany(y => Enumerable.Range(-1, 3)
                .SelectMany(z => Enumerable.Range(-1, 3)
                .Select(w => (x, y, z, w)))))
                .Where(coords => coords != (0, 0, 0, 0));
            
            var state = new Dictionary<(int x, int y, int z, int w), bool>();

            for (var x = 0; x < lines.Count; x++)
            {
                for (var y = 0; y < lines[x].Length; y++)
                {
                    state[(x, y, 0, 0)] = lines[x][y] == '#';
                }
            }

            var count = new Dictionary<(int x, int y, int z, int w), int>();
            for (var i = 0; i < 6; i++)
            {
                count.Clear();
                state.Keys.ForEach(k => count[k] = 0);

                foreach (var ((x, y, z, w), alive) in state.Where(kvp => kvp.Value))
                {
                    foreach (var (dx, dy, dz, dw) in kernel4D)
                    {
                        count[(x + dx, y + dy, z + dz, w + dw)] =
                            count.GetValueOrDefault((x + dx, y + dy, z + dz, w + dw)) + 1;
                    }
                }

                foreach (var (p, c) in count)
                {
                    state[p] = (state.GetValueOrDefault(p), c) switch
                    {
                        (true, >= 2 and <= 3) => true,
                        (false, 3) => true,
                        _ => false
                    };
                }
            }

            return state.Values.Count(x => x);
        }
    }
}
