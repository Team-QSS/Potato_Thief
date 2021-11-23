using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public static class EventSender
{
    public static void SendRaiseEvent(CustomEventTypes eventCode, object contents, ReceiverGroup sendTarget)
    {
        SendRaiseEvent((byte) eventCode, contents, sendTarget);
    }

    public static void SendRaiseEvent(byte eventCode, object contents, ReceiverGroup sendTarget)
    {
        Debug.Log("[Event Sender] SendRaiseEvent");
        if (!PhotonNetwork.IsConnected || PhotonNetwork.CurrentRoom is null) return;
        
        var raiseEventOptions = new RaiseEventOptions()
        {
            Receivers = sendTarget
        };
        var sendOptions = new SendOptions()
        {
            Reliability = true
        };
        Debug.Log($"[Send Event] code : {eventCode.ToString()}");
        PhotonNetwork.RaiseEvent(eventCode, contents, raiseEventOptions, sendOptions);
    }
}