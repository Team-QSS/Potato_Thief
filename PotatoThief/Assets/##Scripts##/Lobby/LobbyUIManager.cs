using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private GameObject connectStatusPopupScreen;
    [SerializeField] private GameObject creatRoomPopupScreen;
    [SerializeField] private GameObject enterRoomPopupScreen;
    [SerializeField] private InputField roomIdInputField;
    [SerializeField] private Toggle isPublicRoomToggle;
    [SerializeField] private Text connectStatusText;
    

    private void Start()
    {
        RoomManager.instance.ConnectStatus.Subscribe(x =>
        {
            if (!connectStatusText.IsDestroyed() && connectStatusText.enabled)
            {
                connectStatusText.text = x;
            }
        });
        connectStatusPopupScreen.SetActive(false);
        creatRoomPopupScreen.SetActive(false);
        enterRoomPopupScreen.SetActive(false);
    }
    
    public void RandomMatchingButtonDown()
    {
        Debug.Log("Call EnterRandomRoom");
        ShowConnectStatus();
        RoomManager.instance.EnterPublicRoom();
    }
    
    public void RoomIdMatchingButtonDown()
    {
        Debug.Log("Call RoomIdMatchingButtonDown");
        var roomId = roomIdInputField.text;
        ShowConnectStatus();
        RoomManager.instance.EnterRoomId(roomId);
    }

    public void MatchingPopupButtonDown()
    {
        Debug.Log("Call MatchingPopupButtonDown");
        enterRoomPopupScreen.SetActive(true);
    }
    
    public void CreatPopupButtonDown()
    {
        Debug.Log("Call CreatPopupButtonDown");
        creatRoomPopupScreen.SetActive(true);
    }
    
    public void RoomCreateButtonDown()
    {
        Debug.Log("Call RoomCreateButtonDown");
        ShowConnectStatus();
        RoomManager.instance.CreatRoom(isPublicRoomToggle.isOn);
    }

    public void RoomDisconnectButtonDown()
    {
        RoomManager.instance.DisconnectRoom();
    }

    private void ShowConnectStatus()
    {
        connectStatusText.text = connectStatusText.text = "Connecting to room";
        connectStatusPopupScreen.SetActive(true);
    }

    public void CloseConnectStatus()
    {
        RoomManager.instance.DisconnectRoom();
        connectStatusPopupScreen.SetActive(false);
    }
}
