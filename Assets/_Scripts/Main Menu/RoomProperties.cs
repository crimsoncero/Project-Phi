using ExitGames.Client.Photon;
using Photon;
using Photon.Pun;


public class RoomProperties
{
    public static string MAP_PROP_KEY = "m";
    public static string TIME_PROP_KEY = "t";
    public static string SCORE_PROP_KEY = "s";
    public static string NICKNAME_PROP_KEY = "n";
    public static string WEAPON_PROP_KEY = "w";

    public Hashtable Hashtable { get; private set; }

	private Maps _map = Maps.Random;
    public Maps Map
    {
        get { return (Maps)Hashtable[MAP_PROP_KEY]; }
        set
        {
            _map = value;
            Hashtable[MAP_PROP_KEY] = _map;
        }
    }
    private int _time = 300;
    public int Time
    {
        get { return (int)Hashtable[TIME_PROP_KEY]; }
        set
        {
            _time = value;
            Hashtable[TIME_PROP_KEY] = _time;
        }
    }
    private int _score = 0;
    public int Score
    {
        get { return (int)Hashtable[SCORE_PROP_KEY]; }
        set
        {
            _score = value;
            Hashtable[SCORE_PROP_KEY] = _score;
        }
    }
    private string _nickname = PhotonNetwork.NickName + "'s Game";
    public string Nickname
    {
        get { return (string)Hashtable[NICKNAME_PROP_KEY]; }
        set
        {
            _nickname = value;
            Hashtable[NICKNAME_PROP_KEY] = _nickname;
        }
    }
    private WeaponSpawnPattern _weaponSpawnPattern;
    public WeaponSpawnPattern WeaponSpawnPattern
    {
        get { return (WeaponSpawnPattern)Hashtable[WEAPON_PROP_KEY]; }
        set
        {
            _weaponSpawnPattern = value;
            Hashtable[WEAPON_PROP_KEY] = _weaponSpawnPattern;
        }
    }

    public RoomProperties()
    {
        Hashtable = new Hashtable()
        {
            { MAP_PROP_KEY, _map },
            { TIME_PROP_KEY, _time },
            { NICKNAME_PROP_KEY, _nickname },
            { SCORE_PROP_KEY, _score },
            { WEAPON_PROP_KEY, _weaponSpawnPattern },
        };
    }

    public RoomProperties(Hashtable customPropertiesTable)
    {
        Hashtable = customPropertiesTable;
    }


}
