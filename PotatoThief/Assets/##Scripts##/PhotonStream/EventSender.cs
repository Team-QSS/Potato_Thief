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
        var raiseEventOptions = new RaiseEventOptions()
        {
            Receivers = sendTarget
        };
        var sendOptions = new SendOptions()
        {
            Reliability = true
        };
        
        PhotonNetwork.RaiseEvent(eventCode, contents, raiseEventOptions, sendOptions);
    }
}