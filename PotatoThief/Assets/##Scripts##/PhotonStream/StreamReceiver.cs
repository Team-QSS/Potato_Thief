using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;

public class StreamReceiver : MonoBehaviour
{
    public byte EventCode { get; set; }
    public int Data { get; set; }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log("[Receiver] Receive Event");

        EventCode = photonEvent.Code;
        Data = (int)photonEvent.CustomData;

        Debug.Log($"[Receiver] EventCode : {EventCode}, Data : {Data}");
    }
}

