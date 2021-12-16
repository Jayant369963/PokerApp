using Ardalis.GuardClauses;
using PokerApp.Core.Enums;
using PokerApp.Core.Interfaces;

namespace PokerApp.Core;

public class Hand : IHand
{
    public IList<ICard> Cards { get; } = new List<ICard>();

    public Hand(ICard card1, ICard card2)
    {
        Guard.Against.Null(card1, nameof(card1));
        Guard.Against.Null(card2, nameof(card2));
        Guard.Against.AgainstExpression(x => !x.card1.Equals(x.card2), (card1, card2),
            "Cannot have duplicate cards in a hand");
        Cards.Add(card1);
        Cards.Add(card2);
    }

    public HandRanking GetHandRanking()
    {
        if (IsStraightFlush)
            return HandRanking.StraightFlush;
        if (IsFlush)
            return HandRanking.Flush;
        if (IsStraight)
            return HandRanking.Straight;
        return IsPair ? HandRanking.Pair : HandRanking.HighCard;
    }

    /// <summary>
    ///     Straight Flush(2 cards of sequential rank, same suit)
    /// </summary>
    private bool IsStraightFlush => IsStraight && IsFlush;

    /// <summary>
    ///     Flush(2 cards, same suit)
    /// </summary>
    private bool IsFlush => Cards.GroupBy(h => h.Suit).Count() == 1;

    /// <summary>
    ///     Straight(2 cards of sequential rank, different suit)
    /// </summary>
    private bool IsStraight
    {
        get
        {
            var ordered = Cards.OrderBy(h => h.Value).ToArray();
            var straightStart = (int) ordered.First().Value;
            for (var i = 1; i < ordered.Length; i++)
                if ((int) ordered[i].Value != straightStart + i)
                    return false;

            return true;
        }
    }

    /// <summary>
    ///     1 pair(2 cards of same rank)
    /// </summary>
    private bool IsPair => Cards
        .GroupBy(h => h.Value)
        .Count(g => g.Count() == 2) == 1;
}