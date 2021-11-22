using System;
using DG.Tweening;
using Photon.Pun;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ReadyCanvas : MonoBehaviour
    {
        [SerializeField] private bool canStart;
        [SerializeField] private Image otherPlayerImage;
    
        public void OnClickBackButton()
        {
            // 대충 포톤 스크립트
            RoomManager.instance.DisconnectRoom();
            SceneManagerEx.Instance.LoadScene(SceneType.Lobby);
        }

        public void OnClickStartButton()
        {
            if (!canStart) return;
            StreamReceiver.instance.SentMasterCheckEvent();
            SceneManagerEx.Instance.LoadScene(SceneType.InGame);
        }

        private void Start()
        {
            canStart = false;
            otherPlayerImage.DOFade(0.5f, 0.01f);
            RoomManager.instance.playersStatus.Subscribe((status) =>
            {
                switch (status)
                {
                    case PlayersStatus.OtherJoined:
                        otherPlayerImage.DOFade(1f, 0.1f);
                        canStart = true;
                        break;
                    case PlayersStatus.OtherDisconnected:
                        otherPlayerImage.DOFade(0.5f, 1f);
                        canStart = false;
                        break;
                    case PlayersStatus.Joined:
                        canStart = PhotonNetwork.CountOfPlayersInRooms == 2; // photon rooom player num
                        break;
                    case PlayersStatus.Disconnected:
                        break;
                }
            });
        }
    }
}
