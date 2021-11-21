using System;
using UniRx;
using UnityEngine;

namespace UI
{
    public class ReadyCanvas : MonoBehaviour
    {
        [SerializeField] private int playerCount = 2;
    
        public void OnClickBackButton()
        {
            // 대충 포톤 스크립트
            RoomManager.instance.DisconnectRoom();
            SceneManagerEx.Instance.LoadScene(SceneType.Lobby);
        }

        public void OnClickStartButton()
        {
            if (playerCount != 2) return;
            
            SceneManagerEx.Instance.LoadScene(SceneType.InGame);
        }

        private void Start()
        {
            RoomManager.instance.playersStatus.Subscribe((status) =>
            {
                switch (status)
                {
                    case PlayersStatus.OtherJoined:
                        break;
                    case PlayersStatus.OtherDisconnected:
                        break;
                    case PlayersStatus.Joined:
                        break;
                    case PlayersStatus.Disconnected:
                        break;
                    default:
                        break;
                }
            });
        }
    }
}
