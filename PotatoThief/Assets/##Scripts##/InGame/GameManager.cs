using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private float maxLimitTime;
        private float limitTime;
        private bool isGameEnd;

        private const int maxHP = 5;
        private int HP;

        public bool[] canPlayerMove;

        private void Start()
        {
            canPlayerMove = new[] {true, true};
            HP = maxHP;
            limitTime = maxLimitTime;

            this.UpdateAsObservable().Subscribe(_ =>
            {
                if (!isGameEnd)
                {
                    limitTime -= Time.deltaTime;
                    UIManager.Instance.UpdateLimitTimeText(limitTime);
                    if (limitTime <= 0) { GameTimeOver(); }
                }
            }).AddTo(gameObject);
        }

        #region GameEvent (GetItem, HitByObstacle, TouchAtClearPoint)
        
        // 아이템 획득
        public void GetItem(ItemType item)
        {
            UIManager.Instance.UpdateStatusText($"아이템 획득 : {item}\n");
            switch (item)
            {
                case ItemType.None:
                    break;
                case ItemType.Potato:
                    Heal();
                    break;
                case ItemType.GoldenPotato:
                    Recover();
                    break;
            }
        }

        // 장애물과 부딪힘
        public void HitByObstacle(ObstacleType obstacle)
        {
            UIManager.Instance.UpdateStatusText($"장애물과 충돌 : {obstacle}\n");
            switch (obstacle)
            {
                case ObstacleType.None:
                    break;
                case ObstacleType.CutWire:
                    Damaged(3);
                    break;
                case ObstacleType.Manhole:
                    Damaged(2);
                    break;
                case ObstacleType.Banana:
                    Damaged(1);
                    break;
            }
        }

        // 클리어 지점에 닿음
        public void TouchAtClearPoint()
        {
            UIManager.Instance.UpdateStatusText("클리어 지점에 도달\n");
            if (canPlayerMove.Any(who => who == false) || limitTime <= 0)
                return;
            
            GameClear();
        }
        
        #endregion

        #region HP (Heal, Damaged, Retire, Recover)
        
        // 회복
        private void Heal(int value = 1)
        {
            HP = Mathf.Min(HP + value, maxHP);
            UIManager.Instance.UpdateStatusText($"회복. HP : {HP}\n");
        }
        
        // 피격
        private void Damaged(int value = 1)
        {
            HP -= value;
            UIManager.Instance.UpdateLeftHeart(HP);
            UIManager.Instance.UpdateStatusText($"피격. HP : {HP}\n");
            if (HP <= 0) { Retire(); }
        }
        
        // 이동불능
        private void Retire(int who = 0)
        {
            UIManager.Instance.UpdateStatusText($"이동 불능. HP : {who}\n");
            canPlayerMove[who] = false;
            if (canPlayerMove.All(who => who == false)) { GameDeadOver(); }
        }

        // 부활
        private void Recover()
        {
            UIManager.Instance.UpdateStatusText("부활\n");
            Heal(maxHP);
            for (int i = 0; i < canPlayerMove.Length; i++) { canPlayerMove[i] = true; }
        }
        
        #endregion

        #region GameEnd (TimeOver, DeadOver, Clear)
        
        private void GameEnd()
        {
            isGameEnd = true;
            for (int i = 0; i < canPlayerMove.Length; i++) { canPlayerMove[i] = false; }
        }

        // 시간 초과
        private void GameTimeOver()
        {
            UIManager.Instance.UpdateStatusText("시간 초과\n");
            GameEnd();
        }
        
        // 전멸
        private void GameDeadOver()
        {
            UIManager.Instance.UpdateStatusText("전멸\n");
            GameEnd();
        }

        // 클리어
        private void GameClear()
        {
            UIManager.Instance.UpdateStatusText("클리어\n");
            GameEnd();
        }
        
        #endregion

    }
}