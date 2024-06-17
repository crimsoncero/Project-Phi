using ExitGames.Client.Photon;
using Photon;


public class RoomProperties
{
    public ExitGames.Client.Photon.Hashtable Hashtable {  get; private set; }

	private Maps _map = Maps.Random;
    public Maps Map
    {
        get { return (Maps)Hashtable["m"]; }
        set
        {
            _map = value;
            Hashtable["m"] = _map;
        }
    }
    private int _time = 120;
    public int Time
    {
        get { return (int)Hashtable["t"]; }
        set
        {
            _time = value;
            Hashtable["t"] = _time;
        }
    }
    private int _score = 0;
    public int Score
    {
        get { return (int)Hashtable["s"]; }
        set
        {
            _score = value;
            Hashtable["s"] = _score;
        }
    }
    private string _nickname = "Match";
    public string Nickname
    {
        get { return (string)Hashtable["n"]; }
        set
        {
            _nickname = value;
            Hashtable["n"] = _nickname;
        }
    }
    private WeaponSpawnPattern _weaponSpawnPattern;
    public WeaponSpawnPattern WeaponSpawnPattern
    {
        get { return (WeaponSpawnPattern)Hashtable["w"]; }
        set
        {
            _weaponSpawnPattern = value;
            Hashtable["w"] = _weaponSpawnPattern;
        }
    }

    public RoomProperties()
    {
        Hashtable = new ExitGames.Client.Photon.Hashtable()
        {
            { "m", _map },
            { "t", _time },
            { "n", _nickname },
            { "s", _score },
            { "w", _weaponSpawnPattern },
        };
    }

    public RoomProperties(Hashtable customPropertiesTable)
    {
        Hashtable = customPropertiesTable;
    }


}
