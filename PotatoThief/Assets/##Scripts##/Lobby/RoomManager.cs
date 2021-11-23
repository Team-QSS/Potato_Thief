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
    
    private const int _maxRoomId = 1000000;
    private const int _minRoomId = 100000;
    
    private bool _isConnecting;
    [SerializeField] private bool _isShowRoomList;

    public bool IsCreateRoom { get; set; }
    public bool IsPublicRoom { get; set; }
    public string RoomName { get; set; }
    
    
    public Subject<PlayersStatus> playersStatus = new Subject<PlayersStatus>();
    // public Subject<string> RoomConnectionStatus = new Subject<string>();

    protected override void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
    }
    
    /*
    private void Update()
    {
        currentRoom.text = PhotonNetwork.IsConnected switch
        {
            true when PhotonNetwork.CurrentRoom != null => $"ID : {PhotonNetwork.CurrentRoom.Name}",
            true => "Connecting",
            _ => "Disconnect"
        };
        currentRoom.text = $"{currentRoom.text} \nPlayer ready = {PlayerStatusCheck.Instance.isPlayerReady.ToString()}";
        currentRoom.text = $"{currentRoom.text} \nOther Player ready = {PlayerStatusCheck.Instance.isOtherPlayerReady.ToString()}";
    }
    */

    private void InitializedMatchingData(bool isPublicRoom, bool isConnecting, bool isCreateRoom)
    {
        IsPublicRoom = isPublicRoom;
        _isConnecting = isConnecting;
        IsCreateRoom = isCreateRoom;
    }
    
    
    public void ShowPublicRoomList()
    {
        UnityEngine.Debug.Log("Show Public Room List");
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
        if(_isConnecting) return;
        InitializedMatchingData(isPublicRoom, true, true);
        
        Debug.Log("[Creat Room] Enter Random Room");
        PhotonNetwork.ConnectUsingSettings();
    }       

    public void EnterPublicRoom()
    {
        if(_isConnecting) return;
        InitializedMatchingData(true, true, false);

        Debug.Log("[Enter Room] Enter Public Room");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void EnterRoomId(string roomId)
    {
        if(_isConnecting) return;
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
        string userName;
        
        // 디버그 돌릴경우에는 Firebase없이 로컬에서 테스트 하기 위함
        userName = _isDebugMode ? "_DEBUGER" : LoginManager.firebaseLoginManager.User.DisplayName;
        
        PhotonNetwork.LocalPlayer.NickName = userName;


        if (IsCreateRoom)
        {
            // 방 생성시 수행할 코드
            RoomName = GetRandomRoomCode();

            var roomOptions = new RoomOptions();
            roomOptions.IsVisible = IsPublicRoom;
            roomOptions.MaxPlayers = 2;

            PhotonNetwork.CreateRoom(RoomName, roomOptions);
        }
        else
        {
            // 방 입장시 수행할 코드
            if (IsPublicRoom)
            {
                // 공개방 입장
                PhotonNetwork.JoinRandomRoom(null, 2);
            }
            else
            {
                // 비공개방 입장
                PhotonNetwork.JoinRoom(RoomName);
            }
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // 방 생성 실패시 callback
        _isConnecting = false;
        DisconnectRoom();
        Debug.Log($"{returnCode} : {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 랜덤 방 입장 실패시 callback
        _isConnecting = false;
        DisconnectRoom();
        Debug.Log($"{returnCode.ToString()} : {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // 방 입장 실패시 callback
        _isConnecting = false;
        DisconnectRoom();
        Debug.Log($"{returnCode.ToString()} : {message}");
    }
    
    public override void OnJoinedRoom()
    {
        // 방 입장 성공시 callback
        _isConnecting = false;
        Debug.Log("[OnJoinedRoom] : Join Success");
        PhotonNetwork.Instantiate("StreamObject", Vector3.zero, Quaternion.identity);
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

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        Debug.Log("Update Room List");
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
}
