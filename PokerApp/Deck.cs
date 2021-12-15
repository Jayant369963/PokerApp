using PokerApp.Core.Enums;
using PokerApp.Core.Interfaces;

namespace PokerApp.Core;

public class Deck : IDeck
{
    private static readonly Random Rng = new();

    public Deck()
    {
        var cards = Enum.GetValues(typeof(Suit))
            .Cast<Suit>()
            .SelectMany(suit => Enum.GetValues(typeof(FaceValue)).Cast<FaceValue>(),
                (suit, value) => new Card(suit: suit, value: value)).Cast<ICard>().ToList();
        Cards = cards;
    }

    public IList<ICard> Cards { get; }

    public void Shuffle()
    {
        var n = Cards.Count;
        while (n > 1)
        {
            n--;
            var k = Rng.Next(n + 1);
            (Cards[k], Cards[n]) = (Cards[n], Cards[k]);
        }
    }
}