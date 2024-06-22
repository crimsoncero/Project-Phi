using Photon.Realtime;
using System.Collections.Generic;
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
        if(_gamesList == null)
            _gamesList = new Dictionary<string, GameTag>();

        UpdateList();
    }

    private void UpdateList()
    {
        List<RoomInfo> roomList = Con.RoomList;

        // Check to remove before adding new tags.


        // Remove rooms from the list that are not in roomList.
        foreach(string gameName in _gamesList.Keys)
        {
            // Check if there is no game with the given id currently in the roomList.
            if(roomList.FindIndex((r) => r.Name == gameName) == -1)
            {
                // Remove tag from list.
                RemoveGameTag(gameName);
            }
        }

        // Add/Update room in the list.
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

        // Update all Game Tags
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
}
