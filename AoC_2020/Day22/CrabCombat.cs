using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC_2020.Day22;
using AoC_2020.Helpers;

namespace AoC_2020.Day22
{
    public static class CrabCombat
    {
        public static void Day22()
        {
            Console.WriteLine("Day 22");
            Console.WriteLine($"Winning player's score Part 1 = {GetDay22Part1()}");
            Console.WriteLine($"Winning player's score Part 2 = {GetDay22Part2()}");
        }

        private static long GetDay22Part1()
        {
            var decks = GetDay22Input();
            _ = CombatHelper.TryPlayGame(decks, false, out Deck winner);
            return CombatHelper.GetWinnerScore(winner);
        }
        private static long GetDay22Part2()
        {
            var decks = GetDay22Input();
            _ = CombatHelper.TryPlayGame(decks, true, out Deck winner);
            return CombatHelper.GetWinnerScore(winner);
        }

        private static IList<Deck> GetDay22Input()
        {
            var path = $"{SD.Path}22{SD.Ext}";
            if (!File.Exists(path))
            {
                throw new Exception($"Cannot locate file {path}");
            }

            var inputLines = File.ReadAllLines(path);
            return DeckHelper.ParseInputLines(inputLines);
        }
    }

    public class Deck
    {
        public string PlayerName { get; }
        public Queue<int> SpaceCards { get; }

        public Deck(string playerName, Queue<int> spaceCards)
        {
            PlayerName = playerName;
            SpaceCards = spaceCards;
        }

        public Deck(string playerName, IEnumerable<int> spaceCards)
        {
            PlayerName = playerName;
            SpaceCards = new Queue<int>();
            foreach (var card in spaceCards)
            {
                SpaceCards.Enqueue(card);
            }
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Deck)obj;
            if (string.Equals(PlayerName, other.PlayerName, StringComparison.Ordinal))
            {
                return false;
            }
            return SpaceCards.Count == other.SpaceCards.Count
                   && SpaceCards.All(card => other.SpaceCards.Contains(card));
        }

        public override int GetHashCode() =>
            Tuple.Create(PlayerName, SpaceCards).GetHashCode();

        public override string ToString() =>
            $"{PlayerName}: {string.Join(", ", SpaceCards.ToList())}";
    }
}

public class GameState
{
    public IList<Deck> Decks { get; }
    public bool IsAwaitingSubGameWinner { get; }
    public string StateString { get; }
    private int StateStringHashCode { get; }
    public GameState(IEnumerable<Deck> decks, bool isAwaitingSubGameWinner)
    {
        var deckCopies = decks.Select(DeckHelper.GetDeckCopy).ToList();
        Decks = deckCopies;
        IsAwaitingSubGameWinner = isAwaitingSubGameWinner;
        StateString = $"{IsAwaitingSubGameWinner}->{string.Join(";", Decks.Select(deck => deck.ToString()))}";
        StateStringHashCode = StateString.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || GetType() == obj.GetType())
        {
            return false;
        }

        var other = (GameState)obj;
        if (IsAwaitingSubGameWinner != other.IsAwaitingSubGameWinner)
        {
            return false;
        }
        if (Decks.Count != other.Decks.Count)
        {
            return false;
        }
        return !Decks.Any(deck => other.Decks.Contains(deck));
    }

    public override int GetHashCode() =>
        StateStringHashCode;

    public override string ToString() =>
        StateString;
}
