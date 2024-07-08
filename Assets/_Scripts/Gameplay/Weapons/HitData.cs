using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using MSLIMA.Serializer;
using Photon.Pun;
using System.Linq;

/// <summary>
/// A data structure to send information related to Hit events.
/// </summary>
public struct HitData
{
    private Player _owner;

    public Player Owner
    {
        get { return _owner; }
        private set { _owner = value; }
    }
    private int _damage;

    public int Damage
    {
        get { return _damage; }
        private set { _damage = value; }
    }

    private Vector3 _position;

    public Vector3 Position
    {
        get { return _position; }
        private set { _position = value; }
    }

    public static readonly byte[] memHitData = new byte[4 + 4 + (4*3)];

    public HitData(Player owner, int damage, Vector3 position)
    {
        _owner = owner;
        _damage = damage;
        _position = position;
    }


    public static short SerializeHitData(StreamBuffer outStream, object customobject)
    {
        HitData data = (HitData)customobject;
        lock(memHitData)
        {
            byte[] bytes = memHitData;
            int index = 0;
            Protocol.Serialize(data.Owner.ActorNumber, bytes, ref index);
            
            Protocol.Serialize(data.Damage, bytes, ref index);
            
            Protocol.Serialize(data.Position.x, bytes, ref index);
            Protocol.Serialize(data.Position.y, bytes, ref index);
            Protocol.Serialize(data.Position.z, bytes, ref index);

            outStream.Write(bytes, 0, bytes.Length);
        }

        return (short)memHitData.Length;
    }

    public static object DeserializeHitData(StreamBuffer inStream, short length)
    {

        HitData data = new HitData();
        lock (memHitData)
        {
            inStream.Read(memHitData, 0, memHitData.Length);
            int index = 0;

            int actorNumber = -1;
            Protocol.Deserialize(out actorNumber, memHitData, ref index);
            data.Owner = PhotonNetwork.PlayerListOthers.Where((p) => p.ActorNumber == actorNumber).FirstOrDefault();

            Protocol.Deserialize(out data._damage, memHitData, ref index);
            
            Protocol.Deserialize(out data._position.x, memHitData, ref index);
            Protocol.Deserialize(out data._position.y, memHitData, ref index);
            Protocol.Deserialize(out data._position.z, memHitData, ref index);

        }

        return data;

    }
}
