using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AoC_2020.Day20;

namespace AoC_2020.Helpers
{
    public static class SatelliteImageHelper
    {
        public static HashSet<Tuple<int, int>> GetRoughWaterPositions(
            IList<string> image,
            IList<string> targetPattern,
            char targetCharacter)
        {
            var result = GetTargetCharacterPositions(image, targetCharacter);

            var targetPatternTile = new Tile(-1, targetPattern);
            var patterns = targetPatternTile.Orientations
                .Select(kvp => kvp.Value)
                .ToList();

            foreach (var pattern in patterns)
            {
                var targetCharacterPositionsWithinPattern = GetTargetCharacterPositions(
                    pattern,
                    targetCharacter);

                var patternHeight = pattern.Count;
                var patternWidth = pattern[0].Length;

                // Move the pattern along the image, one position at a time
                // Every time each target character is hit, flag the positions
                for (int imageRowIndex = 0; imageRowIndex < image.Count; imageRowIndex++)
                {
                    if (imageRowIndex + patternHeight > image.Count)
                    {
                        break;
                    }
                    for (int imageColIndex = 0; imageColIndex < image[imageRowIndex].Length; imageColIndex++)
                    {
                        if (imageColIndex + patternWidth > image[imageRowIndex].Length)
                        {
                            break;
                        }

                        // At this position within the image, loop through the target characters within the pattern
                        var matchedPositions = new HashSet<Tuple<int, int>>();
                        foreach (var targetCharacterPositionWithinPattern in targetCharacterPositionsWithinPattern)
                        {
                            var characterRowIndex = imageRowIndex + targetCharacterPositionWithinPattern.Item1;
                            var characterColIndex = imageColIndex + targetCharacterPositionWithinPattern.Item2;
                            var position = new Tuple<int, int>(characterRowIndex, characterColIndex);
                            var testCharacter = image[characterRowIndex][characterColIndex];
                            if (!testCharacter.Equals(targetCharacter))
                            {
                                break;
                            }
                            matchedPositions.Add(position);
                        }
                        if (matchedPositions.Count == targetCharacterPositionsWithinPattern.Count)
                        {
                            foreach (var matchedPosition in matchedPositions)
                            {
                                if (result.Contains(matchedPosition))
                                {
                                    result.Remove(matchedPosition);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static HashSet<Tuple<int, int>> GetTargetCharacterPositions(
            IList<string> image,
            char targetCharacter)
        {
            var result = new HashSet<Tuple<int, int>>();
            for (int rowIndex = 0; rowIndex < image.Count; rowIndex++)
            {
                for (int colIndex = 0; colIndex < image[rowIndex].Length; colIndex++)
                {
                    if (targetCharacter.Equals(image[rowIndex][colIndex]))
                    {
                        var position = new Tuple<int, int>(rowIndex, colIndex);
                        result.Add(position);
                    }
                }
            }
            return result;
        }

        public static IList<string> GetImageFromTilePlacementStrings(
            IList<IList<IList<string>>> tilePlacementStrings,
            int borderThickness = 1)
        {
            var result = new List<string>();

            var tileHeight = tilePlacementStrings.Count > 0
                ? (tilePlacementStrings[0].Count > 0 ? tilePlacementStrings[0][0].Count : 0)
                : 0;

            foreach (var tileRow in tilePlacementStrings)
            {
                for (int rowIndex = borderThickness; rowIndex < tileHeight - borderThickness; rowIndex++)
                {
                    var rowStringBuilder = new StringBuilder();
                    foreach (var tile in tileRow)
                    {
                        var tileRowString = tile[rowIndex];
                        var tileRowImageString = string.Empty;
                        if (tileRowString.Length > borderThickness * 2)
                        {
                            tileRowImageString = tileRowString.Substring(borderThickness, tileRowString.Length - (2 * borderThickness));
                        }
                        rowStringBuilder.Append(tileRowImageString);
                    }
                    result.Add(rowStringBuilder.ToString());
                }
            }

            return result;
        }

        public static IList<IList<IList<string>>> GetTilePlacementStringsFromTilePlacements(
            IList<Tile> tiles,
            IList<IList<Tuple<int, TileOrientation>>> tilePlacements)
        {
            var result = new List<IList<IList<string>>>();

            var tileIdDictionary = tiles.ToDictionary(tile => tile.TileId);

            foreach (var tileRow in tilePlacements)
            {
                var resultTileRow = new List<IList<string>>();
                foreach (var tilePlacement in tileRow)
                {
                    var tile = tileIdDictionary[tilePlacement.Item1];
                    var tileOrientationStrings = tile.Orientations[tilePlacement.Item2];
                    resultTileRow.Add(tileOrientationStrings);
                }
                result.Add(resultTileRow);
            }
            return result;
        }

        public static void TryGetTilePositionsAndOrientations(IList<Tile> tiles,
            out IList<IList<Tuple<int, TileOrientation>>> tilePlacements)
        {
            var tileIdDictionary = tiles.ToDictionary(tile => tile.TileId);
            var tileIdSet = tiles.Select(tile => tile.TileId).ToHashSet();

            tilePlacements = null;

            if (!TryGetSquareEdgeLength(tiles.Count, out int edgeLength))
            {
                throw new Exception($"Cannot create square from tiles given: {tiles.Count}");
            }

            // Each candidate contains the currently placed tiles/orientations, and the set of remaining tile ids to place
            var candidates = new Stack<Tuple<IList<IList<Tuple<int, TileOrientation>>>, HashSet<int>>>();

            // Seed the candidates with each tile's possible orientations at the top-left
            foreach (var tile in tiles)
            {
                foreach (var orientation in TileOrientation.TileOrientations)
                {
                    var candidateGrid = new List<IList<Tuple<int, TileOrientation>>>();
                    var candidateFirstRow = new List<Tuple<int, TileOrientation>>
                    {
                        new(tile.TileId, orientation)
                    };
                    candidateGrid.Add(candidateFirstRow);
                    var remainingTileIds = tileIdSet.ToHashSet();
                    remainingTileIds.Remove(tile.TileId);

                    var candidate = new Tuple<IList<IList<Tuple<int, TileOrientation>>>, HashSet<int>>(
                        candidateGrid,
                        remainingTileIds);
                    candidates.Push(candidate);
                }
            }

            while (candidates.Count > 0)
            {
                var (currentPlacements, remainingTileIds) = candidates.Pop();
                var candidatesToPush = new List<Tuple<IList<IList<Tuple<int, TileOrientation>>>, HashSet<int>>>();

                // Calculate the next position to consider
                var lastPlacedRow = currentPlacements.Count;
                var lastPlacedColumn = currentPlacements[lastPlacedRow - 1].Count;
                var nextRowIndex = lastPlacedRow - 1;
                var nextColumnIndex = lastPlacedColumn;
                if (lastPlacedColumn >= edgeLength)
                {
                    nextRowIndex = lastPlacedRow;
                    nextColumnIndex = 0;
                }

                // Get the left and up adjacent tiles
                Tuple<int, TileOrientation> tileToLeft = null;
                string tileToLeftEdgeKeyRight = string.Empty;
                Tuple<int, TileOrientation> tileToUp = null;
                string tileToUpEdgeKeyDown = string.Empty;
                if (nextColumnIndex > 0)
                {
                    tileToLeft = currentPlacements[nextRowIndex][nextColumnIndex - 1];
                    tileToLeftEdgeKeyRight = tileIdDictionary[tileToLeft.Item1].OrientationEdgeKeys[tileToLeft.Item2][MovementDirection.Right];
                }
                if (nextRowIndex > 0)
                {
                    tileToUp = currentPlacements[nextRowIndex - 1][nextColumnIndex];
                    tileToUpEdgeKeyDown = tileIdDictionary[tileToUp.Item1].OrientationEdgeKeys[tileToUp.Item2][MovementDirection.Down];
                }

                // Loop through all remaining tile ids
                // If any orientations are valid, add that as a new candidate
                foreach (var nextTileId in remainingTileIds)
                {
                    var nextTile = tileIdDictionary[nextTileId];
                    foreach (var orientation in TileOrientation.TileOrientations)
                    {
                        var edgeKeyLeft = nextTile.OrientationEdgeKeys[orientation][MovementDirection.Left];
                        var edgeKeyUp = nextTile.OrientationEdgeKeys[orientation][MovementDirection.Up];
                        var isValidLeft = tileToLeft == null
                            || string.Equals(tileToLeftEdgeKeyRight, edgeKeyLeft);
                        var isValidTop = tileToUp == null
                            || string.Equals(tileToUpEdgeKeyDown, edgeKeyUp);
                        var isValidPlacement = isValidLeft && isValidTop;
                        if (!isValidPlacement) continue;
                        var nextRemainingTileIds = remainingTileIds.ToHashSet();
                        nextRemainingTileIds.Remove(nextTileId);
                        var nextPlacements = currentPlacements.Select(
                            row => row.ToList()).Cast<IList<Tuple<int, TileOrientation>>>().ToList();
                        if (nextColumnIndex == 0)
                        {
                            nextPlacements.Add(new List<Tuple<int, TileOrientation>>());
                        }
                        var placement = new Tuple<int, TileOrientation>(nextTileId, orientation);
                        nextPlacements[nextRowIndex].Add(placement);
                        var candidateToPush = new Tuple<IList<IList<Tuple<int, TileOrientation>>>, HashSet<int>>(
                            nextPlacements,
                            nextRemainingTileIds);
                        if (nextRemainingTileIds.Count == 0)
                        {
                            tilePlacements = nextPlacements;
                            return;
                        }
                        candidatesToPush.Add(candidateToPush);
                    }
                }

                // Add the candidates back to the stack if placement was successful
                foreach (var candidateToPush in candidatesToPush)
                {
                    candidates.Push(candidateToPush);
                }
            }
        }

        private static bool TryGetSquareEdgeLength(int tileCount, out int edgeLength)
        {
            edgeLength = (int)Math.Sqrt(tileCount);
            return tileCount == edgeLength * edgeLength;
        }

        public static IList<Tile> ParseInputLines(IEnumerable<string> inputLines)
        {
            var result = new List<Tile>();
            var currentTileId = -1;
            var currentTileDefinition = new List<string>();
            foreach (var inputLine in inputLines)
            {
                if (string.IsNullOrWhiteSpace(inputLine))
                    continue;

                var matchTileId = Regex.Match(inputLine, @"^\s*Tile (\d+):\s*$");
                var matchTileLine = Regex.Match(inputLine, @"^\s*((\.|#)+)\s*$");
                if (matchTileId.Success)
                {
                    // Add the previous tile
                    if (currentTileId > -1)
                    {
                        var tileToAdd = new Tile(currentTileId, currentTileDefinition);
                        result.Add(tileToAdd);
                    }

                    // Set the new tile id and reset the definition
                    currentTileId = int.Parse(matchTileId.Groups[1].Value);
                    currentTileDefinition = new List<string>();
                }
                else if (matchTileLine.Success)
                {
                    currentTileDefinition.Add(matchTileLine.Groups[1].Value.Trim());
                }
                else
                {
                    throw new Exception($"Unexpected input line: {inputLine}");
                }
            }

            if (currentTileId <= -1) return result;
            var finalTile = new Tile(currentTileId, currentTileDefinition);
            result.Add(finalTile);
            return result;
        }
    }
}
