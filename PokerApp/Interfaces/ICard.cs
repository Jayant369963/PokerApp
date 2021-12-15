using PokerApp.Core.Enums;

namespace PokerApp.Core.Interfaces;

public interface ICard
{
    Suit Suit { get; set; }
    FaceValue Value { get; set; }

    bool Equals(ICard obj);
}