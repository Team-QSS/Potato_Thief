using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;
using InGame;
using UI;

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

    protected override void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
    }

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
        if(!PhotonNetwork.IsConnected || PhotonNetwork.CurrentRoom is null) return;
        
        Debug.Log("[Receiver] Receiver Data");
        var data = photonEvent.CustomData;
        switch ((CustomEventTypes)photonEvent.Code)
        {
            case CustomEventTypes.CheckMaster:
                GameManager.Instance.myIndex = 0;
                SendEvent((byte)CustomEventTypes.CheckClient, null, ReceiverGroup.Others);
                Debug.Log("[Receiver] CheckMaster");
                break;
            
            case CustomEventTypes.CheckClient:
                GameManager.Instance.myIndex = 1;
                Debug.Log("[Receiver] CheckClient");
                break;
            
            case CustomEventTypes.RequestReady:
                Debug.Log("[Receiver] RequestReady");
                RequestReadyEvent(data);
                break;

            case CustomEventTypes.AnswerReady:
                Debug.Log("[Receiver] AnswerReady");
                AnswerReadyEvent(data);
                break;

            case CustomEventTypes.ReadyOver:
                Debug.Log("[Receiver] ReadyOver");
                ReadyOverEvent();
                break;

            case CustomEventTypes.GameStart:
                Debug.Log("[Receiver] GameStart");
                GameStartEvent();
                break;
        }
    }
    
    private static void RequestReadyEvent(object data)
    {
        var answer = PlayerStatusCheck.Instance.isPlayerReady;
        PlayerStatusCheck.Instance.isOtherPlayerReady = (bool) data;
        EventSender.SendRaiseEvent(CustomEventTypes.AnswerReady, answer, ReceiverGroup.Others);
        Debug.Log($"[Sender] Answer Ready : ans = {answer.ToString()}, other = {((bool) data).ToString()}");
    }
    
    private static void AnswerReadyEvent(object data)
    {
        var otherReady = (bool) data;
        PlayerStatusCheck.Instance.isOtherPlayerReady = otherReady;
        var playerReady = PlayerStatusCheck.Instance.isPlayerReady;
                
        if (otherReady && playerReady)
        {
            EventSender.SendRaiseEvent(CustomEventTypes.ReadyOver, null, ReceiverGroup.MasterClient);
            Debug.Log("[Sender] Ready Over");
            return;
        }

        Debug.Log($"playerReady = {playerReady.ToString()}, otherPlayerReady = {otherReady.ToString()}");
    }
    
    private static void ReadyOverEvent()
    {
        EventSender.SendRaiseEvent(CustomEventTypes.GameStart, null, ReceiverGroup.All);
        // 게임 시작 전 준비과정
        Debug.Log("[Sender] GameStart");
    }
    
    private static void GameStartEvent()
    {
        // 게임 시작
        instance.SentMasterCheckEvent();
        SceneManagerEx.Instance.LoadScene(SceneType.InGame);
        Debug.Log("[Receiver] GameStart");
        
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

