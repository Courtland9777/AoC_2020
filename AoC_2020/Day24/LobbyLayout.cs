using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Helpers;

namespace AoC_2020.Day24
{
    public enum HexMovementDirection
    {
        East,
        SouthEast,
        SouthWest,
        West,
        NorthWest,
        NorthEast
    }

    public static class LobbyLayout
    {
        public static void Day24()
        {
            Console.WriteLine("Day 24");
            Console.WriteLine($"Black side up tiles = {GetDay24Part1()}");
            Console.WriteLine($"Black side after 100 days = {GetDay24Part2()}");
        }

        private static int GetDay24Part1()
        {
            var lines = GetDay24Input();
            var identifiedTiles = TileHelper.GetIdentifiedTiles(lines);
            var blackTiles = TileHelper.GetBlackTiles(identifiedTiles);
            return blackTiles.Count();
        }

        private static int GetDay24Part2()
        {
            var lines = GetDay24Input();
            var identifiedTiles = TileHelper.GetIdentifiedTiles(lines);
            var startingBlackTiles = TileHelper.GetBlackTiles(identifiedTiles);
            var endingBlackTiles = TileHelper.GetBlackTilesAfterNDays(startingBlackTiles, 100);
            return endingBlackTiles.Count();
        }

        private static List<IEnumerable<HexMovementDirection>> GetDay24Input()
        {
            var path = $"{SD.Path}24{SD.Ext}";
            if (!File.Exists(path))
            {
                throw new Exception($"Cannot locate file {path}");
            }

            var inputLines = File.ReadAllLines(path);
            return TileHelper.ParseInputLines(inputLines);
        }
    }
}
