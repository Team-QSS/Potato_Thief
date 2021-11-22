using System;
using DG.Tweening;
using InGame;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AccountCanvas : MonoBehaviour
    {
        [SerializeField] private Text endingText;
        
        public void OnClickRoomButton()
        {
            SceneManagerEx.Instance.LoadScene(SceneType.Ready);
        }

        public void OnClickLobbyButton()
        {
            RoomManager.instance.DisconnectRoom();
            SceneManagerEx.Instance.LoadScene(SceneType.Lobby);
        }

        private void Start()
        {
            var playtime = (long) SceneManagerEx.Instance.SceneLoadInfo;
            var time = 5;
            switch (SceneManagerEx.Instance.gameEndType)
            {
                case GameEndType.TimeOver:
                    endingText.DOText($"시간 초과\n플레이 타임 : {playtime}초", time);
                    break;
                case GameEndType.DeadOver:
                    endingText.DOText($"전멸\n플레이 타임 : {playtime}초", time);
                    break;
                case GameEndType.Clear:
                    endingText.DOText($"완주\n플레이 타임 : {playtime}초", time);
                    Invoke(nameof(AddScoreAtLeaderBoard), 2);
                    break;
                default:
                    endingText.DOText("어쩌구...", time);
                    throw new ArgumentOutOfRangeException(nameof(SceneManagerEx.Instance.gameEndType), SceneManagerEx.Instance.gameEndType, null);
            }
        }

        private void AddScoreAtLeaderBoard()
        {
            LeaderBoardExample.AddLeaderBoardData((long) SceneManagerEx.Instance.SceneLoadInfo);
        }
    }
}
