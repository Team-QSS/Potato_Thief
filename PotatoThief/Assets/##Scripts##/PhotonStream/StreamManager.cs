using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class StreamManager : MonoBehaviourPunCallbacks
{
    public bool isRoomEntered = false;

    public void SendEvent()
    {
        if (!isRoomEntered)
        {
            Debug.Log("Room Entered First");
            return;
        }
        // Custom Event 0: Used as "MoveUnitsToTargetPosition" event
        byte eventCode = 1;

        // Array contains the target position and the IDs of the selected units
        object content = 123;

        // You would have to set the Receivers to All in order to receive this event on the local client as well
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, sendOptions);

        PrintLog.instance.LogString += $"[Sender] eventCode = {eventCode}";
        PrintLog.instance.LogString += $"[Sender] content = {content}";
    }

    public void EnterRoom()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void DisconnectRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = LoginManager.firebaseLoginManager.User.DisplayName;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 20 }, null);
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
