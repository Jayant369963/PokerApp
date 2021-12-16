using PokerApp.Core.Enums;

namespace PokerApp.Core.Interfaces;

public interface IHand
{
    IList<ICard> Cards { get; }

    /// <summary>
    /// Get the handranking
    /// </summary>
    HandRanking GetHandRanking();
}