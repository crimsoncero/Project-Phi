
using ExitGames.Client.Photon;
using Photon.Realtime;

/// <summary>
/// A wrapper class for the player properties hashtable in our game.
/// </summary>
public class PlayerProperties
{
    public Hashtable HashTable {  get; private set; }

	private int _spaceshipConfig;

	public int SpaceshipConfig
	{
		get { return (int)HashTable['c']; }
		set
		{
			_spaceshipConfig = value;
			HashTable['c'] = value;
		}
	}

	public PlayerProperties()
	{
		HashTable = new Hashtable()
		{
			{'c', 0 },
		};
		
	}

	public PlayerProperties(SpaceshipConfig config)
	{
		PlayerProperties prop = new PlayerProperties();
		prop.SpaceshipConfig = config.ID;
	}

	public PlayerProperties (Hashtable customProperties)
	{
		HashTable = customProperties;
	}
	public PlayerProperties (Player player)
	{
		HashTable = player.CustomProperties;
	}


}
