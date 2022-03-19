using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Login;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum PlayersStatus
{
    Joined,
    Disconnected,
    OtherJoined,
    OtherDisconnected
}

public class RoomManager : SingletonPhotonCallbacks<RoomManager>
{
    // [SerializeField] private Text currentRoom;
    [SerializeField] private bool _isDebugMode = false;
    [SerializeField] private bool _isShowRoomList;
    
    private const int _maxRoomId = 1000000;
    private const int _minRoomId = 100000;
    public bool IsConnecting { get; set; }
    public bool IsCreateRoom { get; set; }
    public bool IsPublicRoom { get; set; }
    public string RoomName { get; set; }
    public ReactiveProperty<string> ConnectStatus = new ReactiveProperty<string>();
    
    public Subject<PlayersStatus> playersStatus = new Subject<PlayersStatus>();
    // public Subject<string> RoomConnectionStatus = new Subject<string>();

    protected override void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
    }
    private void InitializedMatchingData(bool isPublicRoom, bool isConnecting, bool isCreateRoom)
    {
        IsPublicRoom = isPublicRoom;
        this.IsConnecting = isConnecting;
        IsCreateRoom = isCreateRoom;
    }
    
    public void ShowPublicRoomList()
    {
        Debug.Log("Show Public Room List");
        _isShowRoomList = true;
        // 기능 구현 필요
        _isShowRoomList = false;
    }

    private string GetRandomRoomCode()
    {
        return Random.Range(_minRoomId, _maxRoomId).ToString();
    }

    public void CreatRoom(bool isPublicRoom)
    {
        if(IsConnecting) return;
        InitializedMatchingData(isPublicRoom, true, true);

        Debug.Log($"[Creat Room] Creat {(isPublicRoom ? "Public" : "Private")} Room");
        PhotonNetwork.ConnectUsingSettings();
    }       

    public void EnterPublicRoom()
    {
        if(IsConnecting) return;
        InitializedMatchingData(true, true, false);

        Debug.Log("[Enter Room] Enter Public Room");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void EnterRoomId(string roomId)
    {
        if(IsConnecting) return;
        RoomName = roomId;
        InitializedMatchingData(false, true, false);

        Debug.Log("::Private Mode:: Enter Private Room");
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public void DisconnectRoom()
    {
        Debug.Log("[Disconnect Room]");
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        var isAndroidPlatform = Application.platform == RuntimePlatform.Android;
        // 로그인이 안되어있는 경우에는 임의의 닉네임으로 지정
        PhotonNetwork.LocalPlayer.NickName = isAndroidPlatform
            ? LoginManager.firebaseLoginManager.User.DisplayName
            : $"USER{Random.Range(10000, 100000).ToString()}";
        // 방 생성시 수행할 코드
        ConnectStatus.Value = "Connecting to room";
        
        if (IsCreateRoom)
        {
            RoomName = GetRandomRoomCode();
            var roomOptions = new RoomOptions();
            roomOptions.IsVisible = IsPublicRoom;
            roomOptions.MaxPlayers = 2;
            Debug.Log($"Room Code : {RoomName}");
            PhotonNetwork.CreateRoom(RoomName, roomOptions);
            return;
        }
        // 방 입장시 수행할 코드
        if (IsPublicRoom)
        {
            PhotonNetwork.JoinRandomRoom(null, 2); // 공개방 입장
            return;
        }
        // 비공개방 입장
        PhotonNetwork.JoinRoom(RoomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // 방 생성 실패시 callback
        ConnectStatus.Value = "Failed to create room";
        IsConnecting = false;
        DisconnectRoom();
        Debug.Log($"{returnCode.ToString()} : {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 랜덤 방 입장 실패시 callback
        ConnectStatus.Value = "Failed to join room";
        IsConnecting = false;
        DisconnectRoom();
        Debug.Log($"{returnCode.ToString()} : {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // 방 입장 실패시 callback
        ConnectStatus.Value = "Failed to join room";
        IsConnecting = false;
        DisconnectRoom();
        Debug.Log($"{returnCode.ToString()} : {message}");
    }
    
    public override void OnJoinedRoom()
    {
        // 방 입장 성공시 callback
        IsConnecting = false;
        Debug.Log("[OnJoinedRoom] : Join Success");
        SceneManagerEx.Instance.LoadScene(SceneType.Ready);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 연결 종료시 callback
        PlayerStatusCheck.Instance.isPlayerReady = false;
        InitializedMatchingData(false, false, false);
        DisconnectRoom();
        Debug.Log("[OnDisconnected] : Disconnect Success");
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerStatusCheck.Instance.isPlayerLeave = true;
        Debug.Log($"{otherPlayer.NickName} left the room");
        PlayerStatusCheck.Instance.isOtherPlayerReady = false;
        playersStatus.OnNext(PlayersStatus.OtherDisconnected);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} joined the room");
        playersStatus.OnNext(PlayersStatus.OtherJoined);
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("[Receive Callback] On Room List Update");
        // 미구현
        // 방 리스트 변경 시 callback
        if (_isShowRoomList) return;
        
        UpdateRoomList(roomList);
    }
    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        Debug.Log("Update Room List");
    }

}
