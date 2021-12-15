using Ardalis.GuardClauses;
using PokerApp.Core.Enums;
using PokerApp.Core.Interfaces;

namespace PokerApp.Core;

public class Game
{
    private const int NumberOfCardsPerPlayer = 2;
    public readonly IDeck Deck;
    public readonly List<Player> Players = new();
    public int NumberOfRounds { get; }
    public int NumberOfPlayers { get; }
    public int CurrentRound { get; private set; }

    public Game(IDeck deck, int numberOfRounds, int numberOfPlayers)
    {
        Deck = deck;
        NumberOfRounds = numberOfRounds;
        NumberOfPlayers = numberOfPlayers;
        CurrentRound = 0;
        InitializePlayers();
    }

    public void DealHandsToAllPlayersInARound()
    {
        Guard.Against.AgainstExpression(a => a.NumberOfRounds > a.CurrentRound, (CurrentRound, NumberOfRounds),
            $"Cannot deal more than the number of rounds specified in the game: {NumberOfRounds}, current round: {CurrentRound}");

        Deck.Shuffle();

        for (var playerCount = 0; playerCount < NumberOfPlayers; playerCount++)
        {
            var cards = Deck.Cards.Skip(playerCount * NumberOfCardsPerPlayer).Take(NumberOfCardsPerPlayer).ToArray();
            var player = Players.FirstOrDefault(p => p.PlayerName == $"P{playerCount + 1}");
            Guard.Against.Null(player, nameof(player));
            var hand = new Hand(cards[0], cards[1]);
            var roundRanking = new PlayerHandRanking
            {
                Hand = hand,
                HandRanking = hand.GetHandRanking()
            };
            player.PlayerHandRankings.Add(CurrentRound, roundRanking);
        }

        AssignPlayerRanking();
        CurrentRound++;
    }

    private void InitializePlayers()
    {
        for (var playerCount = 0; playerCount < NumberOfPlayers; playerCount++)
        {
            Player player = new($"P{playerCount + 1}");
            Players.Add(player);
        }
    }

    private void AssignPlayerRanking()
    {
        // get the winner of the hand
        // check number of distinct handRankings
        if (Players.Count == Players.Select(p => p.PlayerHandRankings[CurrentRound].HandRanking).Distinct().Count())
        {
            var initialScore = 0;
            var players = Players.OrderBy(t => t.PlayerHandRankings[CurrentRound].HandRanking).ToList();
            players.ForEach(p => { p.PlayerHandRankings[CurrentRound].Score = initialScore++; });
            var player = players.LastOrDefault();
            Guard.Against.Null(player, nameof(Player));
            player.HandsWon++;
        }
        // if there is a "tie" (same HandRanking)
        // && hand ranking is only "highCard" (1)
        // find the highest single card
        else if (Players.All(t => t.PlayerHandRankings[CurrentRound].HandRanking == HandRanking.HighCard))
        {
            ApplyRankingBasedOnTheCardValueAndSuit();
        }
        // tie of handRanking (not "highCard")
        // take highest of ranked cards
        else
        {
            var playersHandRankingsOrdered =
                Players.OrderBy(p => p.PlayerHandRankings[CurrentRound].HandRanking).Select(p =>
                        new
                        {
                            Player = p,
                            CardValue = p.PlayerHandRankings[CurrentRound].Hand.Cards.Max(t => Convert.ToInt32(t.Value))
                        })
                    .ToList();

            var playersSortedByHandRankingCardValueAndSuit = playersHandRankingsOrdered.Select(t => new
            {
                t.Player, t.CardValue,
                HandRanking = Convert.ToInt32(t.Player.PlayerHandRankings[CurrentRound].HandRanking),
                t.Player.PlayerHandRankings[CurrentRound].Hand.Cards
                    .FirstOrDefault(p => Convert.ToInt32(p.Value) == t.CardValue)?.Suit
            });

            var initialScore = 0;

            var playersSortedList = playersSortedByHandRankingCardValueAndSuit
                .OrderBy(t => t.HandRanking)
                .ThenBy(t => t.CardValue)
                .ThenBy(t => t.Suit)
                .ToList();
            playersSortedList.ForEach(p => { p.Player.PlayerHandRankings[CurrentRound].Score = initialScore++; });
            var playerRank = playersSortedList.LastOrDefault();
            Guard.Against.Null(playerRank, nameof(playerRank));
            playerRank.Player.HandsWon++;
        }
    }

    private void ApplyRankingBasedOnTheCardValueAndSuit()
    {
        var playerRanks = Players.Select(t => new
        {
            Player = t, CardValue = t.PlayerHandRankings[CurrentRound].Hand.Cards.Max(p => Convert.ToInt32(p.Value))
        }).ToList();

        // if no players have the same card value
        // if some players have the same card value then order them by suit

        var playerSuitRanks = playerRanks
            .Select(t => new
            {
                t.CardValue,
                t.Player,
                t.Player.PlayerHandRankings[CurrentRound].Hand.Cards
                    .FirstOrDefault(p => Convert.ToInt32(p.Value) == t.CardValue)?.Suit
            }).ToList();
        var initialScore = 0;
        playerSuitRanks = playerSuitRanks.OrderBy(t => t.CardValue).ThenBy(t => t.Suit).ToList();
        playerSuitRanks.ForEach(p => { p.Player.PlayerHandRankings[CurrentRound].Score = initialScore++; });
        var playerRank = playerSuitRanks.LastOrDefault();
        Guard.Against.Null(playerRank, nameof(Player));
        playerRank.Player.HandsWon++;
    }
}