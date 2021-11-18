using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomIcon : MonoBehaviour
{
    [SerializeField] private Text roomName;
    public string RoomID
    {
        get { return RoomID; }
        set
        {
            RoomID = value;
            roomName.text = value;
        }
    }
    public void EnterSelectRoom()
    {
        RoomManager.instance.EnterRoomId(RoomID);
    }
}
