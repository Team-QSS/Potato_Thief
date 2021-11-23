using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using Login;

public class StreamManager : SingletonPhotonCallbacks<StreamManager>
{
    public bool isRoomEntered = false;

    protected override void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
    }

    private void Start()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    public void SendEvent()
    {
        if (!isRoomEntered)
        {
            return; // Room Entered First
        }
        
        byte eventCode = 1; // 커스텀 이벤트 번호 설정
        object[] content = {"a", "b", "c"}; // 선택할 오브젝트의 ID와 위치 등의 상태를 object 배열로 전송
        
        RaiseEventOptions raiseEventOptions // 누구에게 보낼지 보낼 대상을 지정
            = new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient};
        SendOptions sendOptions = new SendOptions {Reliability = true}; // 전송 방식을 지정 (UDP, TCP 등)
        
        PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, sendOptions);
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
        var userName = LoginManager.firebaseLoginManager.User.DisplayName;
        PhotonNetwork.LocalPlayer.NickName = userName;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions {MaxPlayers = 20}, null);
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