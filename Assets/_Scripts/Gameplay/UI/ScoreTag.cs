using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ScoreTag : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _score;

    public Player Player { get; private set; }
    private Color _color;

    public void InitTag(Spaceship ship)
    {
        Player = ship.photonView.Owner;
        SpaceshipConfig config = ship.Config;

        _color = config.Color;
        _name.color = _color;
        _score.color = _color;
        _name.text = Player.NickName;
        _score.text = Player.GetPlayerKills().ToString();
    }


    public void UpdateScore()
    {
        _score.text = Player.GetPlayerKills().ToString();
    }


}
