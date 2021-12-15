
namespace PokerApp.Core.Interfaces;

public interface IDeck
{
    IList<ICard> Cards { get; }

    void Shuffle();
}