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
        Debug.Log($"[Set] eventCode");

        // Array contains the target position and the IDs of the selected units
        object[] content = {"a", "b", "c"};
        Debug.Log($"[Set] content");

        // You would have to set the Receivers to All in order to receive this event on the local client as well
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        Debug.Log($"[Set] raiseEventOptions {raiseEventOptions}");
        
        SendOptions sendOptions = new SendOptions { Reliability = true };
        print("[Set] sendOptions");

        PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, sendOptions);
        Debug.Log("[Send] RaiseEvent");

        Debug.Log("[Sender] eventCode");
        Debug.Log("[Sender] content");
    }

    public void EnterRoom()
    {
        Debug.Log("[Enter Room]");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void DisconnectRoom()
    {
        Debug.Log("[Disconnect Room]");
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("[On Connected To Master]");

        // NullReferenceException: Object reference not set to an instance of an object
        var userName = LoginManager.firebaseLoginManager.User.DisplayName;
        Debug.Log("[Get Name]");

        PhotonNetwork.LocalPlayer.NickName = userName;
        Debug.Log("[Set Name]");

        Debug.Log($"Name = {userName}");

        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 20 }, null);
        Debug.Log("JoinOrCreateRoom");
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
