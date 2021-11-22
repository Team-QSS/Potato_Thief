using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;
using InGame;

public enum CustomEventTypes
{
    Default,
    CheckMaster,
    CheckClient,
    RequestReady,
    AnswerReady,
    ReadyOver,
    GameStart
}
public class StreamReceiver : SingletonPhotonCallbacks<StreamReceiver>, IOnEventCallback
{
    public byte EventCode { get; set; }
    public int Data { get; set; }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        EventCode = photonEvent.Code;

        switch ((CustomEventTypes)EventCode)
        {
            case CustomEventTypes.CheckMaster:
                GameManager.Instance.myIndex = 0;
                SendEvent((byte)CustomEventTypes.CheckMaster, null, ReceiverGroup.Others);
                break;
            
            case CustomEventTypes.CheckClient:
                GameManager.Instance.myIndex = 1;
                break;
            case CustomEventTypes.RequestReady: break;
            case CustomEventTypes.AnswerReady: break;
            case CustomEventTypes.ReadyOver: break;
            case CustomEventTypes.GameStart: break;
        }
    }
    
    public void SendEvent(byte code, object data, ReceiverGroup target)
    {
        var eventCode = code; // 커스텀 이벤트 번호 설정
        object content = data;
        
        RaiseEventOptions raiseEventOptions // 누구에게 보낼지 보낼 대상을 지정
            = new RaiseEventOptions {Receivers = target};
        SendOptions sendOptions = new SendOptions {Reliability = true}; // 전송 방식을 지정 (UDP, TCP 등)
        
        PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, sendOptions);
    }

    public void SentMasterCheckEvent()
    {
        RaiseEventOptions raiseEventOptions // 누구에게 보낼지 보낼 대상을 지정
            = new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient};
        SendOptions sendOptions = new SendOptions {Reliability = true}; // 전송 방식을 지정 (UDP, TCP 등)
        
        PhotonNetwork.RaiseEvent((byte)CustomEventTypes.CheckMaster, null, raiseEventOptions, sendOptions);
    }
}

