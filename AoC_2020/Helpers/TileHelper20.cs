using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC_2020.Day20;

namespace AoC_2020.Helpers
{
    public static class TileHelper20
    {
        public static IDictionary<TileOrientation, IDictionary<MovementDirection, string>> GetOrientationEdgeKeys(IList<string> tileDefinition)
        {
            var result = new Dictionary<TileOrientation, IDictionary<MovementDirection, string>>();
            foreach (var orientation in TileOrientation.TileOrientations)
            {
                var tileOrientation = GetTileOrientation(tileDefinition, orientation);
                var edgeKeys = new Dictionary<MovementDirection, string>();
                var edges = new List<MovementDirection>()
                {
                    MovementDirection.Up,
                    MovementDirection.Down,
                    MovementDirection.Left,
                    MovementDirection.Right
                };
                foreach (var edge in edges)
                {
                    var edgeKey = GetTileEdgeKey(tileOrientation, edge);
                    edgeKeys.Add(edge, edgeKey);
                }

                result.Add(orientation, edgeKeys);
            }
            return result;
        }

        private static string GetTileEdgeKey(IList<string> tileDefinition, MovementDirection edge)
        {
            if (tileDefinition == null || tileDefinition.Count == 0)
            {
                return string.Empty;
            }

            switch (edge)
            {
                case MovementDirection.Up:
                    return tileDefinition[0];
                case MovementDirection.Down:
                    return tileDefinition[^1];
                case MovementDirection.Left:
                {
                    var result = new StringBuilder();
                    foreach (var t in tileDefinition)
                    {
                        result.Append(t[0]);
                    }
                    return result.ToString();
                }
                case MovementDirection.Right:
                {
                    var result = new StringBuilder();
                    foreach (var t in tileDefinition)
                    {
                        result.Append(t[^1]);
                    }
                    return result.ToString();
                }
                default:
                    throw new Exception($"Invalid edge: {edge}");
            }
        }

        public static IDictionary<TileOrientation, IList<string>> GetTileOrientations(IList<string> tileDefinition)
        {
            var result = new Dictionary<TileOrientation, IList<string>>();
            foreach (var orientation in TileOrientation.TileOrientations)
            {
                var tileOrientation = GetTileOrientation(tileDefinition, orientation);
                result.Add(orientation, tileOrientation);
            }
            return result;
        }

        private static IList<string> GetTileOrientation(
            IList<string> tileDefinition,
            TileOrientation tileOrientation)
        {
            var result = GetTileRotation(tileDefinition, tileOrientation.RotationDegrees);
            if (tileOrientation.IsReflectedHorizontally)
            {
                result = GetTileReflectionHorizontal(result);
            }
            return result;
        }

        private static IList<string> GetTileReflectionHorizontal(IList<string> tileDefinition)
        {
            var result = tileDefinition.ToArray();
            for (var i = 0; i < result.Length; i++)
            {
                var charArray = result[i].ToCharArray();
                Array.Reverse(charArray);
                result[i] = new string(charArray);
            }
            return result;
        }

        private static IList<string> GetTileRotation(IList<string> tileDefinition, int rotationDegrees)
        {
            if (tileDefinition == null || tileDefinition.Count == 0)
                return tileDefinition;

            var stringLength = tileDefinition[0].Length;
            switch (rotationDegrees)
            {
                case 0:
                    return tileDefinition.ToList();
                case 90:
                {
                    var result = new List<string>();
                    for (var row = 0; row < stringLength; row++)
                    {
                        var rowString = new StringBuilder();
                        foreach (var t in tileDefinition)
                        {
                            rowString.Append(t[stringLength - 1 - row]);
                        }
                        result.Add(rowString.ToString());
                    }
                    return result;
                }
                case 180:
                {
                    var result = tileDefinition.ToList();
                    for (var i = 0; i < result.Count; i++)
                    {
                        var charArray = result[i].ToCharArray();
                        Array.Reverse(charArray);
                        result[i] = new string(charArray);
                    }
                    result.Reverse();
                    return result;
                }
                case 270:
                {
                    var result = new List<string>();
                    for (var row = 0; row < stringLength; row++)
                    {
                        var rowString = new StringBuilder();
                        for (var col = 0; col < tileDefinition.Count; col++)
                        {
                            rowString.Append(tileDefinition[tileDefinition.Count - 1 - col][row]);
                        }
                        result.Add(rowString.ToString());
                    }
                    return result;
                }
                default:
                    throw new Exception($"Invalid rotation degrees: {rotationDegrees}");
            }
        }
    }
}
