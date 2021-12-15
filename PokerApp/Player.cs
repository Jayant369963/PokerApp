using PokerApp.Core.Enums;

namespace PokerApp.Core;

public class Player
{
    public string PlayerName { get; set; }
    public int HandsWon { get; set; }
    public Dictionary<int, PlayerHandRanking> PlayerHandRankings { get; set; } = new();

    public Player(string playerName)
    {
        PlayerName = playerName;
    }
}
