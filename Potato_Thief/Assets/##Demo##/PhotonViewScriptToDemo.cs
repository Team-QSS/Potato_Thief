using System.Collections;
using System.Collections.Generic;
using KJG;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PhotonViewScriptToDemo : MonoBehaviourPunCallbacks
{
    public GameObject playerTemplate;
    public GameObject joystick;
    private PhotonView _photonView;
    
    void Start()
    {
        _photonView = GetComponent<PhotonView>();

        if (_photonView.IsMine)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 참가 완료");
        PhotonNetwork.JoinOrCreateRoom("Pop", new RoomOptions {MaxPlayers = 2}, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("방 참가 완료");
        if (_photonView.IsMine)
        {
            var gameObject =  PhotonNetwork.Instantiate(playerTemplate.name, new Vector3(Random.Range(-5f, 5f), -2f), Quaternion.identity);
            gameObject.GetComponent<PlayerControl>().joystick = joystick;
        }
    }
}