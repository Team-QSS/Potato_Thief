using System;
using System.Collections;
using System.Collections.Generic;
using Login;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class RoomManager : SingletonPhotonCallbacks<RoomManager>
{
    [SerializeField] private Transform roomIconTransform;
    [SerializeField] private GameObject RoomIconPrefab;
    [SerializeField] private Text currentRoom;
    [SerializeField] private bool DEBUG = true;
    [SerializeField] private GameObject streamObject;
    
    private const int _maxRoomId = 1000000;
    private const int _minRoomId = 100000;
    private List<GameObject> _roomIcons;
    private List<RoomInfo> _roomlist;
    private bool _isConnecting;
    [SerializeField] private bool _isShowRoomList;

    public bool IsCreateRoom { get; set; }
    public bool IsPublicRoom { get; set; }
    public string RoomName { get; set; }
    
    protected override void Awake()
    {
        _roomIcons = new List<GameObject>();
        _roomlist = new List<RoomInfo>();
        dontDestroyOnLoad = true;
        base.Awake();
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.CurrentRoom != null)
            {
                currentRoom.text = "ID : " + PhotonNetwork.CurrentRoom.Name;
            }
            else
            {
                currentRoom.text = "Connecting";
            }
        }
        else
        {
            currentRoom.text = "Disconnect";
        }
    }

    public void ShowPublicRoomList()
    {
        Debug.Log("Show Public Room List");
        _isShowRoomList = true;
        int roomCount = _roomIcons.Count;
        for (int i = 0; i < roomCount; i++)
        {
            Destroy(_roomIcons[i]); 
        }

        _roomIcons = new List<GameObject>();

        roomCount = _roomlist.Count;
        for (int i = 0; i < roomCount; i++)
        {
            var roomIcon = Instantiate(RoomIconPrefab, roomIconTransform);
            _roomIcons.Add(roomIcon);
            RoomIcon iconScript = roomIcon.GetComponent<RoomIcon>();
            iconScript.RoomID = _roomlist[i].Name;
        }
        
        _isShowRoomList = false;
    }

    private string GetRandomRoomCode()
    {
        return Random.Range(_minRoomId, _maxRoomId).ToString();
    }

    public void CreatRoom(bool isPublicRoom)
    {
        if(_isConnecting) return;
        
        IsPublicRoom = isPublicRoom;
        _isConnecting = true;
        IsCreateRoom = true;
        Debug.Log("[Creat Room] Enter Random Room");
        PhotonNetwork.ConnectUsingSettings();
    }       

    public void EnterPublicRoom()
    {
        if(_isConnecting) return;
        
        _isConnecting = true;
        IsPublicRoom = true;
        IsCreateRoom = false;
        Debug.Log("[Enter Room] Enter Public Room");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void EnterRoomId(string roomId)
    {
        if(_isConnecting) return;

        RoomName = roomId;
        _isConnecting = true;
        IsPublicRoom = false;
        IsCreateRoom = false;
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
        if (DEBUG)
        {
            userName = "_DEBUGER";
            PhotonNetwork.LocalPlayer.NickName = userName;
        }
        else
        {
            userName = LoginManager.firebaseLoginManager.User.DisplayName;
            PhotonNetwork.LocalPlayer.NickName = userName;
        }

        if (IsCreateRoom)
        {
            // 방 생성시 수행할 코드
            RoomName = GetRandomRoomCode();

            var roomOptions = new RoomOptions();
            roomOptions.IsVisible = IsPublicRoom;
            roomOptions.MaxPlayers = 2;

            PhotonNetwork.CreateRoom(RoomName, roomOptions, null);
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
        Debug.Log($"{returnCode} : {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // 방 입장 실패시 callback
        _isConnecting = false;
        DisconnectRoom();
        Debug.Log($"{returnCode} : {message}");
    }
    
    public override void OnJoinedRoom()
    {
        // 방 입장 성공시 callback
        _isConnecting = false;
        Debug.Log("[OnJoinedRoom] : Join Success");
        PhotonNetwork.Instantiate("StreamObject", Vector3.zero, Quaternion.identity);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 연결 종료시 callback
        _isConnecting = false;
        IsPublicRoom = false;
        IsCreateRoom = false;
        DisconnectRoom();
        Debug.Log("[OnDisconnected] : Disconnect Success");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("[Receive Callback] On Room List Update");
        // 방 리스트 변경 시 callback
        if (_isShowRoomList) return;
        
        UpdateRoomList(roomList);
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        Debug.Log("Update Room List");
    }
}
