using System;
using UnityEngine;
using MSLIMA;
using MSLIMA.Serializer;
using ExitGames.Client.Photon;
using Photon.Pun;
using System.Linq;
public class EndGamePlayerData
{

	private int _actorNumber;
	public int ActorNumber
	{
		get { return _actorNumber; }
		set { _actorNumber = value; }
	}
	private string _name;

	public string Name
	{
		get { return _name; }
		set { _name = value; }
	}

	private int _configID;

	public int ConfigID
	{
		get { return _configID; }
		set { _configID = value; }
	}

	private int _score;

	public int Score
	{
		get { return _score; }
		set { _score = value; }
	}

    public static readonly byte[] memData = new byte[4 * 3];


    public EndGamePlayerData() { }
	public EndGamePlayerData(int actor, int configID, int score)
	{
		_actorNumber = actor;
		_name = "";
		_configID = configID;
		_score = score;
	}


	public static short Serialize(StreamBuffer outStream, object customobject)
	{
		EndGamePlayerData data = (EndGamePlayerData)customobject;
		lock (memData)
		{
			byte[] bytes = memData;
			int index = 0;
			Protocol.Serialize(data._actorNumber, bytes, ref index);
			Protocol.Serialize(data._configID, bytes, ref index);
			Protocol.Serialize(data._score, bytes, ref index);

			outStream.Write(bytes, 0, bytes.Length);
		}

		return (short)memData.Length;
	}

	public static object Deserialize(StreamBuffer inStream, short length)
	{
		EndGamePlayerData data = new EndGamePlayerData();
		lock (memData)
		{
			inStream.Read(memData, 0, memData.Length);
			int index = 0;

			Protocol.Deserialize(out data._actorNumber, memData, ref index);
			Protocol.Deserialize(out data._configID, memData, ref index);
			Protocol.Deserialize(out data._score, memData, ref index);

			data._name = PhotonNetwork.PlayerList.Where((p) => p.ActorNumber == data._actorNumber).FirstOrDefault().NickName;
		}

		return data;
	}

}
