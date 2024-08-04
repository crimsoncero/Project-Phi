using System;
using System.Linq;
using UnityEngine;

public class EndgameScreen : MonoBehaviour
{
    [SerializeField] Transform _container;
    [SerializeField] ScoreTag _scoreTagPrefab;
    private EndGamePlayerData[] _endGameData;

    public void Init(EndGamePlayerData[] endGameData)
    {
        _endGameData = endGameData;

        _endGameData.OrderByDescending((d) => d.Score);

        foreach(var d in _endGameData)
        {
            ScoreTag tag = Instantiate(_scoreTagPrefab, _container);
            tag.InitEndgame(d);
        }

    }

}
