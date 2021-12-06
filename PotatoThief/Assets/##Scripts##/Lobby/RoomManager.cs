using System;
using System.Collections.Generic;
using Login;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UnityEngine;
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
    // [SerializeField] private bool isDebugMode = false;
    
    private const int MaxRoomId = 1000000;
    private const int MinRoomId = 0;
    private const int MaxPlayers = 2;
    
    [SerializeField] private bool _isShowRoomList;

    // Room Connecting Info
    private bool _isConnecting;
    public enum RoomConnect { Create, Random, Custom, }
    public RoomConnect RoomConnectType;
    public bool IsPublicRoom { get; set; }
    public string RoomId { get; set; }
    
    public Subject<PlayersStatus> playersStatus = new Subject<PlayersStatus>();
    // public Subject<string> RoomConnectionStatus = new Subject<string>();

    private void StartConnecting(RoomConnect roomConnectType, bool isPublicRoom, string roomId)
    {
        _isConnecting = true;

        RoomConnectType = roomConnectType;
        
        IsPublicRoom = isPublicRoom;
        RoomId = roomId;
    }

    private void EndConnecting()
    {
        _isConnecting = false;
    }
    
    private static string GetRandomCode6()
    {
        return Random.Range(MinRoomId, MaxRoomId).ToString("D6");
    }

    #region Public Interface
    public void ShowPublicRoomList()
    {
        Debug.Log("Show Public Room List");
        _isShowRoomList = true;
        // 기능 구현 필요
        _isShowRoomList = false;
    }

    public void EnterRandomRoom()
    {
        if(_isConnecting) return;
        StartConnecting(RoomConnect.Random, false, "");

        Debug.Log("[Enter Room] Enter Public Room");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void EnterCustomRoom(string roomId)
    {
        if(_isConnecting) return;
        StartConnecting(RoomConnect.Custom, false, roomId);

        Debug.Log("::Private Mode:: Enter Private Room");
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public void CreateRoom(bool isPublicRoom)
    {
        if(_isConnecting) return;
        StartConnecting(RoomConnect.Create, isPublicRoom, "");
        
        Debug.Log("[Create Room] Enter Random Room");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void DisconnectRoom()
    {
        Debug.Log("[Disconnect Room]");
        PhotonNetwork.Disconnect();
    }
    #endregion

    #region CallBack
    public override void OnConnectedToMaster()
    {
        // 디버그 돌릴경우에는 Firebase없이 로컬에서 테스트 하기 위함
        var isAndroidPlatform = Application.platform == RuntimePlatform.Android;
        var userName = isAndroidPlatform
            ? LoginManager.firebaseLoginManager.User.DisplayName
            : $"USER{GetRandomCode6()}";
        
        PhotonNetwork.LocalPlayer.NickName = userName;

        switch (RoomConnectType)
        {
            case RoomConnect.Random:
                PhotonNetwork.JoinRandomRoom(null, MaxPlayers);
                break;
            
            case RoomConnect.Custom:
                PhotonNetwork.JoinRoom(RoomId);
                break;
            
            case RoomConnect.Create:
                var roomId = GetRandomCode6();
                var roomOptions = new RoomOptions
                {
                    IsVisible = IsPublicRoom,
                    MaxPlayers = MaxPlayers
                };
                PhotonNetwork.CreateRoom(roomId, roomOptions);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // 방 생성 실패시 callback
        EndConnecting();
        DisconnectRoom();
        Debug.Log($"{returnCode} : {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 랜덤 방 입장 실패시 callback
        EndConnecting();
        DisconnectRoom();
        Debug.Log($"{returnCode.ToString()} : {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // 방 입장 실패시 callback
        EndConnecting();
        DisconnectRoom();
        Debug.Log($"{returnCode.ToString()} : {message}");
    }
    
    public override void OnJoinedRoom()
    {
        // 방 입장 성공시 callback
        EndConnecting();
        Debug.Log("[OnJoinedRoom] : Join Success");
        PhotonNetwork.Instantiate("StreamObject", Vector3.zero, Quaternion.identity);
        SceneManagerEx.Instance.LoadScene(SceneType.Ready);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 연결 종료시 callback
        PlayerStatusCheck.Instance.isPlayerReady = false;
        EndConnecting();
        DisconnectRoom();
        Debug.Log("[OnDisconnected] : Disconnect Success");
    }
    #endregion

    #region RoomList
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
    #endregion
}
