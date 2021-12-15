using PokerApp.Core.Enums;

namespace PokerApp.Core.Interfaces;

public interface IHand
{
    IList<ICard> Cards { get; }

    /// <summary>
    /// Flush(2 cards, same suit)
    /// </summary>
    bool IsFlush { get; }

    /// <summary>
    /// 1 pair(2 cards of same rank)
    /// </summary>
    bool IsPair { get; }

    /// <summary>
    /// Straight(2 cards of sequential rank, different suit)
    /// </summary>
    bool IsStraight { get; }

    /// <summary>
    /// Straight Flush(2 cards of sequential rank, same suit)
    /// </summary>
    bool IsStraightFlush { get; }

    /// <summary>
    /// High Card(2 cards, different rank, suit and not in sequence.Highest card wins)
    /// </summary>
    bool IsHigh { get; }

    /// <summary>
    /// Get the handranking
    /// </summary>
    HandRanking GetHandRanking();
}