using System.Linq;
using NUnit.Framework;

namespace PokerApp.Core.Tests;

public class Tests
{
    private const int NumberOfRounds = 6;
    private const int NumberOfPlayers = 5;
    private const int CurrentRound = 0;

    [Test]
    public void ShuffledDeckShouldAlwaysReturnDifferentOrderOfCards()
    {
        // Arrange

        // I have a deck of cards in a game
        var deck = new Deck();
        var game = new Game(deck, NumberOfRounds, NumberOfPlayers);

        // Act
        // I shuffle cards once
        deck.Shuffle();
        // I record the order of the cards
        var shuffleResult1 = string.Join(",", deck.Cards.Select(t => t.Value.ToString() + t.Suit));

        // I shuffle cards again
        deck.Shuffle();
        // I record the order of the cards again
        var shuffleResult2 = string.Join(",", deck.Cards.Select(t => t.Value.ToString() + t.Suit));

        Assert.AreNotEqual(shuffleResult1, shuffleResult2);
    }

    [Test]
    public void EachPlayerInARoundShouldHaveUniqueCards()
    {
        // Arrange
        // I have a deck of cards in a game
        var deck = new Deck();
        var game = new Game(deck, NumberOfRounds, NumberOfPlayers);
        deck.Shuffle();

        // Act
        game.DealHandsToAllPlayersInARound();

        //Assert 
        var players = game.Players;
        foreach(var player in players)
        {
            var cards = player.PlayerHandRankings[CurrentRound].Hand.Cards.Select(c => $"{c.Suit}-{c.Value}")
                .ToList();
            Assert.AreEqual(cards.Count, cards.Distinct().Count());
        }
    }

    [Test]
    public void PlayerWithHighestScoreWinsTheRound()
    {
        // Arrange
        // I have a deck of cards in a game
        var deck = new Deck();
        var game = new Game(deck, NumberOfRounds, NumberOfPlayers);
        deck.Shuffle();

        // Act
        game.DealHandsToAllPlayersInARound();

        // Assert 
        var playerWithHighestScore = game.Players.OrderByDescending(p => p.PlayerHandRankings[CurrentRound].Score).First();

        // At the end of each round, each player is assigned a score (0 – weakest to strongest x-1
        // (where x = number of players)).
        Assert.AreEqual(playerWithHighestScore.PlayerHandRankings[CurrentRound].Score, NumberOfPlayers-1);
        Assert.AreEqual(playerWithHighestScore.HandsWon, game.CurrentRound);
    }

    [Test]
    public void PlayerWithHighestHandsWonIsTheWinner()
    {
        // Arrange
        // I have a deck of cards in a game
        var deck = new Deck();
        var game = new Game(deck, NumberOfRounds, NumberOfPlayers);
        deck.Shuffle();

        // Act
        for (var i = 0; i < game.NumberOfRounds; i++)
        {
            game.DealHandsToAllPlayersInARound();
        }

        // Assert 
        var playerWithHandWins = game.Players.OrderByDescending(p => p.HandsWon).First();
        var playerWithHighestScore =
            game.Players
                .Select(p => new
                {
                    Player = p,
                    NumberOfHighScores = p.PlayerHandRankings.Values.Count(t => t.Score == NumberOfPlayers - 1)
                }).OrderByDescending(t => t.NumberOfHighScores).First();

 
        Assert.AreEqual(playerWithHighestScore.NumberOfHighScores, playerWithHandWins.HandsWon);
    }
}
