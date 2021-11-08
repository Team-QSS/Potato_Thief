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
        RoomManager.instance.EnterRandomRoom();
    }
    
    public void RoomIdMatchingButtonDown()
    {
        var roomId = _roomIdInputField.text;
        RoomManager.instance.EnterRoomId(roomId);
    }
    
    public void RoomCreateButtonDown()
    {
        RoomManager.instance.CreatRoom(_isPublicRoomToggle.isOn);
    }
}
