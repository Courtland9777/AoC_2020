using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC_2020.Extensions;
using AoC_2020.Helpers;
using static AoC_2020.Helpers.SatelliteImageHelper;


namespace AoC_2020.Day20
{
    public static class JurassicJigsaw
    {
        public static void Day20()
        {
            Console.WriteLine("Day 20");
            Console.WriteLine($"Product of four corners = {GetDay20Part1()}");
            Console.WriteLine($"# that are not part of the sea monster = {GetDay20Part2()}");
        }

        private static long GetDay20Part1()
        {
            var tiles = GetDay20Input();
            TryGetTilePositionsAndOrientations(tiles, out IList<IList<Tuple<int, TileOrientation>>> tilePlacements);

            long result = 1;
            result *= tilePlacements[0][0].Item1;
            result *= tilePlacements[0][^1].Item1;
            result *= tilePlacements[^1][0].Item1;
            result *= tilePlacements[^1][^1].Item1;
            return result;
        }

        private static long GetDay20Part2()
        {
            var tiles = GetDay20Input();
            TryGetTilePositionsAndOrientations(tiles, out var tilePlacements);
            var tilePlacementStrings = GetTilePlacementStringsFromTilePlacements(tiles, tilePlacements);
            var image = GetImageFromTilePlacementStrings(tilePlacementStrings, 1);
            var targetPattern = new List<string>()
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   ",
            };
            var roughWaterPositions = 
                GetRoughWaterPositions(image, targetPattern, '#');
            return roughWaterPositions.Count;
        }

        private static IList<Tile> GetDay20Input()
        {
            var path = $"{SD.Path}1{SD.Ext}";
            if (!File.Exists(path))
            {
                throw new Exception($"Cannot locate file {path}");
            }

            var inputLines = File.ReadAllLines(path);
            return ParseInputLines(inputLines);
        }
    }
    public class TileOrientation
    {
        public int RotationDegrees { get; }
        public bool IsReflectedHorizontally { get; }

        private TileOrientation(int rotationDegrees, bool isReflectedHorizontally)
        {
            RotationDegrees = rotationDegrees % 360;
            if (RotationDegrees != 0
                && RotationDegrees != 90
                && RotationDegrees != 180
                && RotationDegrees != 270)
            {
                throw new Exception($"Invalid rotation degrees: {RotationDegrees}");
            }
            IsReflectedHorizontally = isReflectedHorizontally;
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (TileOrientation)obj;
            return (RotationDegrees == other.RotationDegrees)
                   && (IsReflectedHorizontally == other.IsReflectedHorizontally);
        }

        public override int GetHashCode() =>
            Tuple.Create(RotationDegrees, IsReflectedHorizontally).GetHashCode();

        public override string ToString() => 
            string.Format($"Orientation(Rotation: {RotationDegrees}, H-Reflection:{IsReflectedHorizontally})");

        public static IEnumerable<TileOrientation> TileOrientations { get; } = new List<TileOrientation>()
        {
            new(0, false),
            new(0, true),
            new(90, false),
            new(90, true),
            new(180, false),
            new(180, true),
            new(270, false),
            new(270, true),
        };
    }

    public class Tile
    {
        public int TileId { get; }
        private IList<string> TileDefinition { get; }
        public IDictionary<TileOrientation, IList<string>> Orientations { get; }
        public IDictionary<TileOrientation, IDictionary<MovementDirection, string>> OrientationEdgeKeys { get; }
        public Tile(int tileId, IList<string> tileDefinition)
        {
            TileId = tileId;
            TileDefinition = tileDefinition;
            Orientations = TileHelper20.GetTileOrientations(TileDefinition);
            OrientationEdgeKeys = TileHelper20.GetOrientationEdgeKeys(tileDefinition);
        }
    }
}
