using Newtonsoft.Json;
using PokerApp.Core;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var deck = new Deck();
const int numberOfRounds = 3;
const int numberOfPlayers = 2;
var game = new Game(deck, numberOfRounds, numberOfPlayers);

for (var i = 0; i < numberOfRounds; i++)
{
    game.DealHandsToAllPlayersInARound();
}

foreach (var player in game.Players)
{
    Console.WriteLine(JsonConvert.SerializeObject(player, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter())); 
}

foreach (var player in game.Players.OrderByDescending(p => p.HandsWon))
{
    Console.WriteLine($"Player: {player.PlayerName} won {player.HandsWon} hands");
}

var playerWithHandWins = game.Players.OrderByDescending(p => p.HandsWon).First();
Console.WriteLine($"Player: {playerWithHandWins.PlayerName} won the game with {playerWithHandWins.HandsWon} hands won");


Console.ReadLine();
