
using ExitGames.Client.Photon;
using Photon.Realtime;

/// <summary>
/// A wrapper class for the player properties hashtable in our game.
/// </summary>
public class PlayerProperties
{
    public Hashtable HashTable {  get; private set; }

	private SpaceshipConfig _spaceshipConfig = null;

	public SpaceshipConfig SpaceshipConfig
	{
		get { return (SpaceshipConfig)HashTable['c']; }
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
			{'c', _spaceshipConfig },
		};
		
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
