using PokerApp.Core.Enums;
using PokerApp.Core.Interfaces;

namespace PokerApp.Core;

public class Card : ICard
{
    public FaceValue Value { get; set; }

    public Suit Suit { get; set; }

    public Card(FaceValue value, Suit suit)
    {
        Suit = suit;
        Value = value;
    }

    public bool Equals(ICard card)
    {
        return Suit == card.Suit && Value == card.Value;
    }
}