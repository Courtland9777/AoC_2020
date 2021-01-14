using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2020.Day22;

namespace AoC_2020.Helpers
{
    public static class CombatHelper
    {
        public static long GetWinnerScore(Deck winner)
        {
            long result = 0;
            var cards = winner.SpaceCards.ToList();
            while (cards.Count > 0)
            {
                var multiplier = cards.Count;
                var card = cards[0];
                cards.RemoveAt(0);
                var partialScore = card * multiplier;
                result += partialScore;
            }
            return result;
        }



        public static bool TryPlayGame(
            IList<Deck> decks,
            bool isRecursive,
            out Deck winner)
        {
            winner = null;
            if (decks.Count == 0)
            {
                return false;
            }

            // Stack to keep track of each game at each level
            // Item 1: current state
            // Item 2: previous states
            var gameStack = new Stack<Tuple<GameState, HashSet<string>>>();

            // Seed the stack
            var initialGameState = new GameState(decks, false);
            var initialGame = new Tuple<GameState, HashSet<string>>(
                initialGameState,
                new HashSet<string>());
            gameStack.Push(initialGame);

            // Variable to keep track of the winner from the previous subgame
            Deck subgameWinner = null;

            // Start the loop
            int round = 0;
            while (gameStack.Count > 0)
            {
                round++;
                var subgame = gameStack.Pop();
                var subgameState = subgame.Item1;
                var previousSubgameStates = subgame.Item2;

                //Console.WriteLine($"---Round: {round}");
                //Console.WriteLine($"--Game: {gameStack.Count}");
                //subgameState.Print();
                //if (round % 100 == 0)
                //{
                //    Console.ReadKey();
                //}

                if (isRecursive)
                {
                    // Check if this state has occurred before
                    // If it has, set the first player as the winner and end this subgame
                    // (pop to the next higher game level)
                    if (previousSubgameStates.Contains(subgameState.StateString))
                    {
                        subgameWinner = subgameState.Decks[0];
                        continue;
                    }
                }
                previousSubgameStates.Add(subgameState.StateString);

                // Check if there is a winner for the current subgame
                var isSubgameWinner = TryGetWinner(subgameState, out Deck possibleSubgameWinner);
                if (isSubgameWinner)
                {
                    // Set the subgame winner and end this subgame
                    // (pop to the next higher game level)
                    subgameWinner = possibleSubgameWinner;
                    continue;
                }

                // Check if this subgame is awaiting the winner from a lower-level subgame
                // If it is, then the winner of the subgame has been determined
                // Calculate the next state based on that winner
                // Push the game back into the stack
                if (subgameState.IsAwaitingSubGameWinner)
                {
                    if (subgameWinner == null)
                    {
                        throw new Exception("No subgame winner found");
                    }
                    subgameState = GetNextGameStateGivenSubgameWinner(subgameState, subgameWinner);
                    subgame = new Tuple<GameState, HashSet<string>>(subgameState, previousSubgameStates);
                    gameStack.Push(subgame);
                    continue;
                }

                // Play round:
                // Draw cards
                // If recursive, AND If both players have at least as many 
                // cards remaining in their deck as the value of the card they 
                // just drew, start a new subgame
                // Otherwise, the winner of the round is the player with the 
                // higher-value card.
                if (isRecursive)
                {
                    var isStartingSubgame = TryStartSubgame(
                        subgameState,
                        out GameState newSubgameState);
                    if (isStartingSubgame)
                    {
                        subgameState = new GameState(subgameState.Decks, true);
                        subgame = new Tuple<GameState, HashSet<string>>(
                            subgameState,
                            previousSubgameStates);
                        gameStack.Push(subgame);
                        var newSubgame = new Tuple<GameState, HashSet<string>>(
                            newSubgameState,
                            new HashSet<string>());
                        gameStack.Push(newSubgame);
                        continue;
                    }
                }

                // Classic game: winner of round is player who drew highest card
                var isRoundPlayed = TryPlayClassicRound(
                    subgameState,
                    out GameState finalGameState);
                if (!isRoundPlayed)
                {
                    throw new Exception($"Failed to play round");
                }
                subgame = new Tuple<GameState, HashSet<string>>(
                    finalGameState,
                    previousSubgameStates);
                gameStack.Push(subgame);

            }

            if (subgameWinner != null)
            {
                // Console.WriteLine($"Winner found int {round} round(s)!");
                winner = subgameWinner;
                return true;
            }
            return false;
        }

        public static bool TryPlayClassicRound(
            GameState initialGameState,
            out GameState finalGameState)
        {
            finalGameState = initialGameState;
            var deckCopies = new List<Deck>();
            foreach (var deck in initialGameState.Decks)
            {
                var deckCopy = DeckHelper.GetDeckCopy(deck);
                deckCopies.Add(deckCopy);
            }

            var topCards = new List<int>();
            var bestCardPlayerIndex = -1;
            var bestCard = -1;
            for (int playerIndex = 0; playerIndex < deckCopies.Count; playerIndex++)
            {
                var deck = deckCopies[playerIndex];
                if (deck.SpaceCards.Count == 0)
                {
                    continue;
                }
                var topCard = deck.SpaceCards.Dequeue();
                topCards.Add(topCard);

                if (topCard > bestCard)
                {
                    bestCard = topCard;
                    bestCardPlayerIndex = playerIndex;
                }
            }

            // If one or fewer players still have cards, then the game is over
            if (topCards.Count < 2)
            {
                return false;
            }

            // Add the cards to the winner's deck
            if (bestCardPlayerIndex >= 0)
            {
                var deck = deckCopies[bestCardPlayerIndex];
                var cardsToAdd = topCards
                    .OrderByDescending(card => card)
                    .Select(card => card)
                    .ToList();
                foreach (var card in cardsToAdd)
                {
                    deck.SpaceCards.Enqueue(card);
                }
            }

            finalGameState = new GameState(deckCopies, false);
            return true;
        }

        public static bool TryStartSubgame(GameState gameState, out GameState subgameState)
        {
            subgameState = null;
            var subgameDecks = new List<Deck>();
            foreach (var deck in gameState.Decks)
            {
                var deckCopy = DeckHelper.GetDeckCopy(deck);
                if (deckCopy.SpaceCards.Count == 0)
                {
                    continue;
                }
                var topCard = deckCopy.SpaceCards.Dequeue();
                if (deckCopy.SpaceCards.Count < topCard)
                {
                    return false;
                }
                var subgameDeckCards = new List<int>();
                for (int i = 0; i < topCard; i++)
                {
                    subgameDeckCards.Add(deckCopy.SpaceCards.Dequeue());
                }
                var subgameDeck = new Deck(deckCopy.PlayerName, subgameDeckCards);
                subgameDecks.Add(subgameDeck);
            }
            if (subgameDecks.Count < 2)
            {
                return false;
            }
            subgameState = new GameState(subgameDecks, false);
            return true;
        }

        public static GameState GetNextGameStateGivenSubgameWinner(
            GameState initialGameState,
            Deck roundWinner)
        {
            //var result = new GameState(initialGameState.Decks, false);

            if (TryGetWinner(initialGameState, out Deck _))
            {
                return initialGameState;
            }
            var decks = DeckHelper.GetDeckCopies(initialGameState.Decks);
            var topCards = new List<int>();
            Deck winnerDeck = null;
            foreach (var deck in decks)
            {
                if (deck.SpaceCards.Count > 0)
                {
                    var topCard = deck.SpaceCards.Dequeue();
                    if (roundWinner.PlayerName.Equals(deck.PlayerName))
                    {
                        topCards.Insert(0, topCard);
                        winnerDeck = deck;
                    }
                    else
                    {
                        topCards.Add(topCard);
                    }
                }
            }
            foreach (var card in topCards)
            {
                winnerDeck.SpaceCards.Enqueue(card);
            }
            var result = new GameState(decks, false);
            return result;
        }

        public static bool TryGetWinner(GameState gameState, out Deck winner)
        {
            winner = null;
            var potentialWinners = new List<Deck>();
            foreach (var deck in gameState.Decks)
            {
                if (deck.SpaceCards.Count > 0)
                {
                    potentialWinners.Add(deck);
                }
                if (potentialWinners.Count > 1)
                {
                    return false;
                }
            }
            if (potentialWinners.Count == 0)
            {
                return false;
            }
            winner = potentialWinners[0];
            return true;
        }
    }
}
