using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;

public class StreamReceiver : MonoBehaviour, IOnEventCallback
{
    public byte EventCode { get; set; }
    public int Data { get; set; }

    void Photon.Realtime.IOnEventCallback.OnEvent(EventData photonEvent)
    {
        EventCode = photonEvent.Code;
        Data = (int)photonEvent.CustomData;

        Debug.Log($"EventCode : {EventCode}, Data : {Data}");
        PrintLog.instance.LogString = $"EventCode : {EventCode}, Data : {Data}";
    }
}
