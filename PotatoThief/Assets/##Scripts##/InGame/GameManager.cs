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
        
        private bool[] canPlayerMove;

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
            });
        }

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

        public void TouchAtClearPoint()
        {
            UIManager.Instance.UpdateStatusText("클리어 지점에 도달\n");
            if (canPlayerMove.Any(who => who == false) || limitTime <= 0)
                return;
            
            GameClear();
        }

        // 회복
        private void Heal(int value = 1)
        {
            HP = Mathf.Min(HP + value, maxHP);
            UIManager.Instance.UpdateStatusText($"회복 : {HP}\n");
        }
        
        // 피격
        private void Damaged(int value = 1)
        {
            HP -= value;
            UIManager.Instance.UpdateLeftHeart(HP);
            UIManager.Instance.UpdateStatusText($"피격 : {HP}\n");
            if (HP <= 0) { Retire(); }
        }
        
        // 이동불능
        private void Retire(int who = 0)
        {
            UIManager.Instance.UpdateStatusText($"이동 불능 : {who}\n");
            canPlayerMove[who] = false;
            if (canPlayerMove.Any(who => who == false)) { GameDeadOver(); }
        }

        // 재기
        private void Recover()
        {
            UIManager.Instance.UpdateStatusText($"재기\n");
            for (int i = 0; i < canPlayerMove.Length; i++) { canPlayerMove[i] = true; }
        }

        // 시간 초과
        private void GameTimeOver()
        {
            UIManager.Instance.UpdateStatusText("시간 초과\n");
            isGameEnd = true;
        }
        
        // 전멸
        private void GameDeadOver()
        {
            UIManager.Instance.UpdateStatusText("전멸\n");
            isGameEnd = true;
        }

        // 클리어
        private void GameClear()
        {
            UIManager.Instance.UpdateStatusText("클리어\n");
            isGameEnd = true;
        }
    }
}