
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class StreamManager : MonoBehaviourPunCallbacks
{
    public bool isRoomEntered = false;

    private void Start()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    public void SendEvent()
    {
        if (!isRoomEntered)
        {
            Debug.Log("Room Entered First");
            return;
        }
        
        // Custom Event 0: Used as "MoveUnitsToTargetPosition" event
        byte eventCode = 1;
        print("[Set] eventCode");

        // Array contains the target position and the IDs of the selected units
        object content = 123;
        print("[Set] content");

        // You would have to set the Receivers to All in order to receive this event on the local client as well
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        print("[Set] raiseEventOptions");
        
        SendOptions sendOptions = new SendOptions { Reliability = true };
        print("[Set] sendOptions");

        PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, sendOptions);
        print("[Send] RaiseEvent");

        print($"[Sender] eventCode = {eventCode}");
        print($"[Sender] content = {content}");
    }

    public void EnterRoom()
    {
        print("[Enter Room]");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void DisconnectRoom()
    {
        print("[Disconnect Room]");
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        print("[On Connected To Master]");

        // NullReferenceException: Object reference not set to an instance of an object
        string name = LoginManager.firebaseLoginManager.User.DisplayName;
        print("[Get Name]");

        PhotonNetwork.LocalPlayer.NickName = name;
        print("[Set Name]");

        print($"Name = {name}");

        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 20 }, null);
        print("JoinOrCreateRoom");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("[OnJoinedRoom] : Join Success");
        isRoomEntered = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("[OnDisconnected] : Disconnect Success");
        isRoomEntered = false;
    }
}
