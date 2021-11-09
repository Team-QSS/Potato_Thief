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
    [SerializeField] private Text currentRoom;
    private bool DEBUG = true;
    
    private const int _maxRoomId = 1000000;
    private const int _minRoomId = 100000;
    private bool _isConnecting;
    
    public bool IsCreateRoom { get; set; }
    public bool IsPublicRoom { get; set; }
    public string RoomName { get; set; }

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

    protected override void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
    }

    private string GetRandomRoomCode()
    {
        Debug.Log("GetRandomRoomCode");
        var totalSecond = (long) (System.DateTime.Now.TimeOfDay.TotalSeconds * System.Math.Pow(10, 7)); // 유효 소숫점 (7자리) 부분을 없애주고 long형태로 타입변환
        long roomCode = 0;
        while (totalSecond != 0)
        {
            // 현재 시간을 _maxRoomId보다 작은 자릿수로 만들기 위한 연산
            roomCode += totalSecond % _maxRoomId;
            totalSecond /= _maxRoomId;
        }

        roomCode += Random.Range(_minRoomId, _maxRoomId);
        roomCode %= _maxRoomId;
        return roomCode.ToString();
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
            RoomName = GetRandomRoomCode();

            var roomOptions = new RoomOptions();
            roomOptions.IsVisible = IsPublicRoom;
            roomOptions.MaxPlayers = 2;

            PhotonNetwork.CreateRoom(RoomName, roomOptions, null);
        }
        else
        {
            if (IsPublicRoom)
            {
                PhotonNetwork.JoinRandomRoom(null, 2);
            }
            else
            {
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
}
