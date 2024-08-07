using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameListPanel : MonoBehaviour
{
    [SerializeField] private GameTag _gameTagPrefab;
    [SerializeField] private GameObject _tagContainer;
    private ConnectionManager Con { get { return ConnectionManager.Instance; } }
    private MainMenuManager MainMenu {  get { return MainMenuManager.Instance; } }

    // A dictionary of Room Names(ID) and the corresponding GameTag.
    private Dictionary<string, GameTag> _gamesList;
    

    private void OnEnable()
    {
        Con.OnUpdatedRoomList += RefreshList;

        if(_gamesList == null)
            _gamesList = new Dictionary<string, GameTag>();

        UpdateList();
    }

    private void OnDisable()
    {
        Con.OnUpdatedRoomList -= RefreshList;
    }

    private void UpdateList()
    {
        List<RoomInfo> roomList = Con.RoomList;

        // Check to remove before adding new tags.

        string[] namesArr = _gamesList.Keys.ToArray();
        // Remove rooms from the list that are not in roomList.
        for(int i = 0; i < namesArr.Length; i++)
        {
            // Check if there is no game with the given id currently in the roomList.
            if(roomList.FindIndex((r) => r.Name == namesArr[i]) == -1)
            {
                // Remove tag from list.
                RemoveGameTag(namesArr[i]);
            }
        }

        // Add/UpdateTag room in the list.
        foreach (RoomInfo info in roomList)
        {
            if(_gamesList.ContainsKey(info.Name))
                _gamesList[info.Name].Init(info);
            else
            {
                GameTag tag = CreateGameTag(info);
                _gamesList.Add(info.Name, tag);
            }
        }

        // UpdateTag all Game Tags
        foreach(var tag in _gamesList)
        {
            tag.Value.UpdateTag();
        }
    }

    private GameTag CreateGameTag(RoomInfo info)
    {
        GameTag tag = Instantiate(_gameTagPrefab, _tagContainer.transform);
        tag.Init(info);
        return tag;
    }

    private void RemoveGameTag(string gameName)
    {
        Destroy(_gamesList[gameName].gameObject);
        _gamesList.Remove(gameName);
    }

    public void ReturnToMainMenu()
    {
        MainMenu.ActivateMenuPanel();
    }

    public void RefreshList()
    {
        UpdateList();
    }

}
