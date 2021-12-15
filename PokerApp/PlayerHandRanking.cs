using PokerApp.Core.Enums;
using PokerApp.Core.Interfaces;

namespace PokerApp.Core;

public class PlayerHandRanking
{
    public IHand Hand { get; set; }
    public HandRanking HandRanking { get; set; }
    public int Score { get; set; }
}
