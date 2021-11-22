using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace InGame
{
    public struct Player
    {
        public int hp;
        public bool canMove;
        public bool isRetired;
        public bool isFinished;
    }
    public enum GameEndType
    {
        None,
        TimeOver,
        DeadOver,
        Clear
    }
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private float maxLimitTime;
        private float limitTime;
        private bool isGameEnd;

        private const int maxHP = 5;
        public Player[] Players;
        public int myIndex;

        private void Start()
        {
            Players = new Player[2];
            Players[0].hp = Players[1].hp = maxHP;
            Players[0].canMove = Players[1].canMove = true;
            Players[0].isRetired = Players[1].isRetired = false; 
            Players[0].isFinished = Players[1].isFinished = false; 
            
            limitTime = maxLimitTime;
            
            this.UpdateAsObservable().Where(_ => !isGameEnd).Subscribe(_ =>
            {
                limitTime -= Time.deltaTime;
                UIManager.Instance.UpdateLimitTimeText(limitTime);
                if (limitTime <= 0) { GameEnd(GameEndType.TimeOver); }
            }).AddTo(gameObject);
        }

        #region GameEvent (GetItem, HitByObstacle, TouchAtClearPoint) -> 공유해야 함
        
        // 아이템 획득
        public void GetItem(ItemType item, int who)
        {
            UIManager.Instance.UpdateStatusText($"아이템 획득 : {item}\n");
            switch (item)
            {
                case ItemType.None:
                    break;
                case ItemType.Potato:
                    Heal(who);
                    break;
                case ItemType.GoldenPotato:
                    Recover();
                    break;
            }
        }

        // 장애물과 부딪힘
        public void HitByObstacle(ObstacleType obstacle, int who)
        {
            UIManager.Instance.UpdateStatusText($"장애물과 충돌 : {obstacle}\n");
            switch (obstacle)
            {
                case ObstacleType.None:
                    break;
                case ObstacleType.CutWire:
                    Damaged(who,3);
                    break;
                case ObstacleType.Manhole:
                    Damaged(who,2);
                    break;
                case ObstacleType.Banana:
                    Damaged(who, 1);
                    break;
            }
        }

        // 클리어 지점에 닿음
        public void TouchAtClearPoint(int who)
        {
            UIManager.Instance.UpdateStatusText("클리어 지점에 도달\n");
            Players[who].isFinished = true;

            if (Players.Any(player => player.isFinished == false) || limitTime <= 0)
                return;
            
            GameEnd(GameEndType.Clear);
        }
        
        #endregion

        #region HP (Heal, Damaged, Retire, Recover)
        
        // 회복
        private void Heal(int who, int value = 1)
        {
            Players[who].hp = Mathf.Min(Players[who].hp + value, maxHP);
            UIManager.Instance.UpdateStatusText($"회복. HP : {Players[who].hp}\n");
        }
        
        // 피격
        private void Damaged(int who, int value = 1)
        {
            Players[who].hp -= value;
            UIManager.Instance.UpdateLeftHeart(Players[who].hp);
            UIManager.Instance.UpdateStatusText($"피격. HP : {Players[who].hp}\n");
            if (Players[who].hp <= 0) { Retire(who); }
        }
        
        // 이동불능
        private void Retire(int who)
        {
            UIManager.Instance.UpdateStatusText($"이동 불능. HP : {who}\n");
            Players[who].canMove = false;
            Players[who].isRetired = true;
            if (Players.All(player => player.isRetired)) { GameEnd(GameEndType.DeadOver); }
        }

        // 부활
        private void Recover()
        {
            UIManager.Instance.UpdateStatusText("부활\n");
            Heal(maxHP);
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].canMove = true;
                Players[i].isRetired = false;
            }
        }
        
        #endregion

        #region GameEnd (TimeOver, DeadOver, Clear)

        private void GameEnd(GameEndType gameEndType)
        {
            switch (gameEndType)
            {
                case GameEndType.TimeOver:
                    UIManager.Instance.UpdateStatusText("시간 초과\n");
                    break;
                case GameEndType.DeadOver:
                    UIManager.Instance.UpdateStatusText("전멸\n");
                    break;
                case GameEndType.Clear:
                    UIManager.Instance.UpdateStatusText("클리어\n");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameEndType), gameEndType, null);
            }

            isGameEnd = true;
            for (int i = 0; i < Players.Length; i++) { Players[i].canMove = false; }
            SceneManagerEx.Instance.LoadScene(SceneType.Account, gameEndType, maxLimitTime - limitTime);
        }
        #endregion

    }
}