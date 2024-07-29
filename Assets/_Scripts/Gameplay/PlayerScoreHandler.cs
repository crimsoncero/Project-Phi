using Photon.Realtime;
using System.Collections.Generic;


public class PlayerScoreHandler
{
    private class ScoreLock
    {
        public int ScoreCarry;
        public bool IsLocked;

        public ScoreLock(int score, bool isLocked)
        {
            ScoreCarry = score;
            IsLocked = isLocked;
        }
    }

    private Dictionary<Player, ScoreLock> _scoreMemory;

    public PlayerScoreHandler(List<Player> playerList)
    {
        _scoreMemory = new Dictionary<Player, ScoreLock>();

        foreach (var player in playerList)
        {
            _scoreMemory.Add(player, new ScoreLock(0, false));
        }
    }

    
    public void IncreasePlayerScore(Player player, int amount)
    {
        if (!_scoreMemory.ContainsKey(player)) return;

        if (_scoreMemory[player].IsLocked == true)
        {
            _scoreMemory[player].ScoreCarry += 1;
        }
        else
        {
            _scoreMemory[player].IsLocked = true;

            int newScore = player.GetPlayerKills() + amount;
            player.SetPlayerKills(newScore);

        }
    }

    public void UnlockPlayerScore(Player player)
    {
        if(! _scoreMemory.ContainsKey(player)) return;

        if (_scoreMemory[player].ScoreCarry == 0)
        {
            _scoreMemory[player].IsLocked = false;
        }
        else
        {
            int newScore = player.GetPlayerKills() + _scoreMemory[player].ScoreCarry;
            _scoreMemory[player].ScoreCarry = 0;
            player.SetPlayerKills(newScore);
        }
    }


}
