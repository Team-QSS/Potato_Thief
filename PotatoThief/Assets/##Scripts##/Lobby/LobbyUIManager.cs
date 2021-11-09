using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _creatRoomPopupScreen;
    [SerializeField] private GameObject _enterRoomPopupScreen;
    [SerializeField] private InputField _roomIdInputField;
    [SerializeField] private Toggle _isPublicRoomToggle;
    
    void Start()
    {
        _creatRoomPopupScreen.SetActive(false);
        _enterRoomPopupScreen.SetActive(false);
    }
    
    public void RandomMatchingButtonDown()
    {
        Debug.Log("Call EnterRandomRoom");
        RoomManager.instance.EnterPublicRoom();
    }
    
    public void RoomIdMatchingButtonDown()
    {
        Debug.Log("Call RoomIdMatchingButtonDown");
        var roomId = _roomIdInputField.text;
        RoomManager.instance.EnterRoomId(roomId);
    }

    public void MatchingPopupButtonDown()
    {
        Debug.Log("Call MatchingPopupButtonDown");
        _enterRoomPopupScreen.SetActive(true);
    }
    
    public void CreatPopupButtonDown()
    {
        Debug.Log("Call CreatPopupButtonDown");
        _creatRoomPopupScreen.SetActive(true);
    }
    
    public void RoomCreateButtonDown()
    {
        Debug.Log("Call RoomCreateButtonDown");
        RoomManager.instance.CreatRoom(_isPublicRoomToggle.isOn);
    }

    public void RoomDisconnectButtonDown()
    {
        RoomManager.instance.DisconnectRoom();
    }
}
