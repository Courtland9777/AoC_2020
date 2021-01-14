using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2020.Helpers
{
    public static class CrabCupHelper
    {
        public static string GetCanonicalCrabCupString(IList<int> cups)
        {
            var canonicalCupOrder = new List<int>();
            var cup1Index = cups.IndexOf(1);
            for (var indexOffset = 1; indexOffset < cups.Count; indexOffset++)
            {
                var cupIndex = (cup1Index + indexOffset) % cups.Count;
                var cupLabel = cups[cupIndex];
                canonicalCupOrder.Add(cupLabel);
            }
            return string.Join(string.Empty, canonicalCupOrder);
        }
        public static IList<int> PlayCrabCups(
            IList<int> startingNumbers,
            int numberOfCupsToPickUp,
            int numberOfRounds)
        {
            var numberLinkedList = new LinkedList<int>(startingNumbers);
            var labelToNodeDictionary = new Dictionary<int, LinkedListNode<int>>();
            var currentNode = numberLinkedList.First;
            for (var i = 0; i < startingNumbers.Count; i++)
            {
                if (currentNode == null) continue;
                labelToNodeDictionary.Add(currentNode.Value, currentNode);
                currentNode = currentNode.Next;
            }

            var currentCupNode = numberLinkedList.First;
            //var startTime = DateTime.Now;
            //const int pingRounds = 1000000;
            for (var roundNumber = 1; roundNumber <= numberOfRounds; roundNumber++)
            {
                var cupsToRemoveNodes = new List<LinkedListNode<int>>();

                //if (roundNumber % pingRounds == 0)
                //{
                //    var endTime = DateTime.Now;
                //    //var timeDiffSeconds = (endTime - startTime).TotalSeconds;
                //    //var secondsPerRound = (double)timeDiffSeconds / pingRounds;
                //    //var totalSeconds = numberOfRounds * secondsPerRound;
                //    //Console.WriteLine($"---> Round {roundNumber}: {timeDiffSeconds} seconds; Estimated total time for all: {totalSeconds}");
                //    startTime = DateTime.Now;
                //}

                var nextCupNode = currentCupNode;
                for (var i = 0; i < numberOfCupsToPickUp; i++)
                {
                    nextCupNode = nextCupNode?.Next ?? numberLinkedList.First;
                    cupsToRemoveNodes.Add(nextCupNode);
                }

                if (currentCupNode != null)
                {
                    var destinationCup = currentCupNode.Value;
                    for (var i = 1; i < startingNumbers.Count; i++)
                    {
                        destinationCup--;
                        if (destinationCup < 1)
                        {
                            destinationCup += startingNumbers.Count;
                        }
                        if (!cupsToRemoveNodes.Select(n => n.Value).Contains(destinationCup))
                        {
                            break;
                        }
                    }

                    var destinationCupNode = labelToNodeDictionary[destinationCup];

                    // Remove them
                    foreach (var removeNode in cupsToRemoveNodes)
                    {
                        numberLinkedList.Remove(removeNode);
                    }

                    var addAfterNode = destinationCupNode;
                    foreach (var addNode in cupsToRemoveNodes)
                    {
                        numberLinkedList.AddAfter(addAfterNode, addNode);
                        addAfterNode = addNode;
                    }
                }

                currentCupNode = currentCupNode?.Next ?? numberLinkedList.First;
            }

            return numberLinkedList.ToList();
        }

        public static void ProcessRound(
            IList<int> currentState,
            int currentCupIndex,
            int destinationCupIndex,
            IList<int> pickedUpCupIndexes,
            out int nextCurrentCupIndex)
        {
            var totalNumberOfCups = currentState.Count;
            var pickedUpCupLabels = new int[pickedUpCupIndexes.Count];
            for (var i = 0; i < pickedUpCupIndexes.Count; i++)
            {
                pickedUpCupLabels[i] = currentState[pickedUpCupIndexes[i]];
            }

            var removalIndex = currentCupIndex + 1;
            for (var i = 0; i < pickedUpCupLabels.Length; i++)
            {
                if (removalIndex >= currentState.Count)
                {
                    removalIndex = 0;
                }
                currentState.RemoveAt(removalIndex);
                if (removalIndex < currentCupIndex)
                {
                    currentCupIndex--;
                }
                if (removalIndex < destinationCupIndex)
                {
                    destinationCupIndex--;
                }
            }

            for (var i = 0; i < pickedUpCupLabels.Length; i++)
            {
                var cupToInsertLabel = pickedUpCupLabels[pickedUpCupLabels.Length - 1 - i];
                var insertionIndex = destinationCupIndex + 1;
                currentState.Insert(insertionIndex, cupToInsertLabel);
                if (currentCupIndex >= insertionIndex)
                {
                    currentCupIndex++;
                }
            }

            nextCurrentCupIndex = (currentCupIndex + 1) % totalNumberOfCups;
        }

        public static int GetDestinationCupIndex(
            IList<int> currentState,
            int currentCupIndex,
            IEnumerable<int> pickedUpCupIndexes)
        {
            var result = -1;
            var currentCupLabel = currentState[currentCupIndex];
            var pickedUpCupLabels = pickedUpCupIndexes.Select(i => currentState[i]).ToHashSet();

            var maxLabel = currentState.Count;
            const int minLabel = 1;

            var minMaxDelta = maxLabel - minLabel;
            for (var labelOffset = 1; labelOffset <= minMaxDelta; labelOffset++)
            {
                var destinationLabel = currentCupLabel - labelOffset;
                if (destinationLabel < minLabel)
                {
                    destinationLabel += minMaxDelta + 1;
                }
                if (pickedUpCupLabels.Contains(destinationLabel))
                    continue;
                result = currentState.IndexOf(destinationLabel);
                break;
            }

            return result;
        }


        public static IList<int> GetPickedUpCupIndexes(
            IList<int> currentState,
            int currentCupIndex,
            int numberOfCupsToPickUp)
        {
            var result = new List<int>();
            for (var i = 0; i < numberOfCupsToPickUp; i++)
            {
                var cupToPickUpIndex = (currentCupIndex + 1 + i) % currentState.Count;
                result.Add(cupToPickUpIndex);
            }
            return result;
        }

        public static bool GetAreEquivalent(IList<int> left, IList<int> right)
        {
            if (left.Count != right.Count)
                return false;
            if (left.Count == 0)
                return true;
            var startNumber = left[0];
            var rightStartIndex = right.IndexOf(startNumber);
            for (var i = 0; i < left.Count; i++)
            {
                var rightIndex = (rightStartIndex + i) % left.Count;
                if (left[i] != right[rightIndex])
                    return false;
            }
            return true;
        }

        public static IList<int> GetPart2StartingNumbers(IList<int> initialStartingNumbers)
        {
            var result = initialStartingNumbers.ToList();
            for (var i = initialStartingNumbers.Count + 1; i <= 1000000; i++)
            {
                result.Add(i);
            }
            return result;
        }

        public static IList<int> ParseInputLine(string inputLine) =>
            inputLine.ToCharArray().Select(numberString => int.Parse(numberString.ToString())).ToList();
    }
}
