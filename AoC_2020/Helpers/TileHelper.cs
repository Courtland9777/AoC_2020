using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Day24;

namespace AoC_2020.Helpers
{
    public static class TileHelper
    {
        public static IEnumerable<GridPoint> GetBlackTilesAfterNDays(IEnumerable<GridPoint> startingBlackTiles, int numberOfDays)
        {
            var currentBlackTiles = startingBlackTiles;
            for (var i = 1; i <= numberOfDays; i++)
            {
                currentBlackTiles = GetNextDayBlackTiles(currentBlackTiles);
            }
            return currentBlackTiles;
        }

        private static IEnumerable<GridPoint> GetNextDayBlackTiles(IEnumerable<GridPoint> startingBlackTiles)
        {
            var result = new List<GridPoint>();
            var blackTiles = startingBlackTiles as GridPoint[] ?? startingBlackTiles.ToArray();
            var currentBlackTiles = blackTiles.ToHashSet();

            var tilesToCheck = new HashSet<GridPoint>();
            foreach (var blackTile in blackTiles)
            {
                tilesToCheck.Add(blackTile);
                var adjacentTiles = GetAdjacentHexPoints(blackTile);
                foreach (var adjacentTile in adjacentTiles)
                {
                    if (!tilesToCheck.Contains(adjacentTile))
                    {
                        tilesToCheck.Add(adjacentTile);
                    }
                }
            }

            foreach (var tile in tilesToCheck)
            {
                var isCurrentlyBlack = currentBlackTiles.Contains(tile);
                var numberOfAdjacentBlackTiles = GetAdjacentHexPoints(tile)
                    .Count(gridPoint => currentBlackTiles.Contains(gridPoint));

                switch (isCurrentlyBlack)
                {
                    case true when (numberOfAdjacentBlackTiles == 1
                                    || numberOfAdjacentBlackTiles == 2):
                    case false when numberOfAdjacentBlackTiles == 2:
                        result.Add(tile);
                        break;
                }
            }

            return result;
        }

        public static IEnumerable<GridPoint> GetBlackTiles(IEnumerable<GridPoint> identifiedTiles)
        {
            var tileDictionary = new Dictionary<GridPoint, int>();
            foreach (var identifiedTile in identifiedTiles)
            {
                if (!tileDictionary.ContainsKey(identifiedTile))
                {
                    tileDictionary.Add(identifiedTile, 0);
                }
                tileDictionary[identifiedTile]++;
            }
            var result = tileDictionary
                .Where(kvp => kvp.Value % 2 == 1)
                .Select(kvp => kvp.Key)
                .ToList();
            return result;
        }

        public static IEnumerable<GridPoint> GetIdentifiedTiles(IEnumerable<IEnumerable<HexMovementDirection>> lines) =>
            lines.Select(line => GetPath(GridPoint.Origin, line)).Select(
                path => path.Last()).ToList();

        private static IEnumerable<GridPoint> GetPath(
            GridPoint startPoint,
            IEnumerable<HexMovementDirection> directions)
        {
            var result = new List<GridPoint>() { startPoint };
            var currentPoint = startPoint;
            foreach (var direction in directions)
            {
                var nextPoint = MoveHex(currentPoint, direction);
                result.Add(nextPoint);
                currentPoint = nextPoint;
            }
            return result;
        }

        private static List<HexMovementDirection> HexMovementDirections { get; } = new()
        {
            HexMovementDirection.East,
            HexMovementDirection.SouthEast,
            HexMovementDirection.SouthWest,
            HexMovementDirection.West,
            HexMovementDirection.NorthWest,
            HexMovementDirection.NorthEast
        };

        private static IEnumerable<GridPoint> GetAdjacentHexPoints(GridPoint startingPoint) =>
            HexMovementDirections.ConvertAll(direction => MoveHex(startingPoint, direction));

        private static GridPoint MoveHex(GridPoint startingPoint, HexMovementDirection direction)
        {
            return direction switch
            {
                HexMovementDirection.East => new GridPoint(startingPoint.X + 1, startingPoint.Y),
                HexMovementDirection.SouthEast => new GridPoint(startingPoint.X, startingPoint.Y - 1),
                HexMovementDirection.SouthWest => new GridPoint(startingPoint.X - 1, startingPoint.Y - 1),
                HexMovementDirection.West => new GridPoint(startingPoint.X - 1, startingPoint.Y),
                HexMovementDirection.NorthWest => new GridPoint(startingPoint.X, startingPoint.Y + 1),
                HexMovementDirection.NorthEast => new GridPoint(startingPoint.X + 1, startingPoint.Y + 1),
                _ => throw new Exception($"Unknown direction: {direction}")
            };
        }

        private static IEnumerable<HexMovementDirection> ParseInputLine(string inputLine)
        {
            var result = new List<HexMovementDirection>();
            for (var i = 0; i < inputLine.Length; i++)
            {
                var currentCharacter = inputLine[i];
                char? nextCharacter = i == inputLine.Length - 1 ? null : inputLine[i + 1];
                HexMovementDirection direction;
                switch (currentCharacter)
                {
                    case 'e':
                        direction = HexMovementDirection.East;
                        break;
                    case 'w':
                        direction = HexMovementDirection.West;
                        break;
                    case 's' when 'e'.Equals(nextCharacter):
                        direction = HexMovementDirection.SouthEast;
                        i++;
                        break;
                    case 's' when 'w'.Equals(nextCharacter):
                        direction = HexMovementDirection.SouthWest;
                        i++;
                        break;
                    case 'n' when 'e'.Equals(nextCharacter):
                        direction = HexMovementDirection.NorthEast;
                        i++;
                        break;
                    case 'n' when 'w'.Equals(nextCharacter):
                        direction = HexMovementDirection.NorthWest;
                        i++;
                        break;
                    default:
                        throw new Exception($"Invalid characters: {currentCharacter}, {nextCharacter}");
                }
                result.Add(direction);
            }
            return result;
        }

        public static List<IEnumerable<HexMovementDirection>> ParseInputLines(IEnumerable<string> inputLines) =>
             inputLines.Select(ParseInputLine).ToList();
    }
}
