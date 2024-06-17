using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class RoomListManager : MonoBehaviour
{
    [SerializeField] private ConnectionManager _conManager;

    [SerializeField] private GameObject _roomButtonPrefab;

    private List<GameObject> _roomsList;

    public void UpdateList()
    {
        List<RoomInfo> roomInfoList = _conManager.RoomList;
    }



}
