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
        private const int maxHP = 5;
        private int HP;
        [SerializeField] private float maxLimitTime;
        private float limitTime;
        
        [SerializeField] private Text text;
        private bool[] canPlayerMove;

        private void Start()
        {
            canPlayerMove = new[] {true, true};
            HP = maxHP;
            limitTime = maxLimitTime;

            this.UpdateAsObservable().Subscribe(_ =>
            {
                limitTime -= Time.deltaTime;
                text.text = $"남은시간 : {Mathf.Round(limitTime)} 초";
                if (limitTime <= 0) { GameTimeOver(); }
            });
        }

        // 아이템 획득
        public void GetItem(ItemType item)
        {
            switch (item)
            {
                case ItemType.None:
                    break;
                case ItemType.Potato:
                    Healed();
                    break;
                case ItemType.GoldenPotato:
                    Recover();
                    break;
            }
        }

        // 장애물과 부딪힘
        public void HitByObstacle(ObstacleType obstacle)
        {
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
            if (canPlayerMove.Any(who => who == false) || limitTime <= 0)
                return;
            
            GameClear();
        }

        // 회복
        private void Healed(int value = 1)
        {
            HP = Mathf.Min(HP + value, maxHP);
        }
        
        // 피격
        private void Damaged(int value = 1)
        {
            HP -= value;
            if (HP <= 0) { Retire(); }
        }
        
        // 전투불능
        private void Retire(int who = 0)
        {
            canPlayerMove[who] = false;
            if (canPlayerMove.Any(who => who == false)) { GameDeadOver(); }
        }

        // 재기
        private void Recover()
        {
            for (int i = 0; i < canPlayerMove.Length; i++) { canPlayerMove[i] = true; }
        }

        // 시간 초과
        private void GameTimeOver() { }
        
        // 전멸
        private void GameDeadOver() { }

        // 클리어
        private void GameClear() { }
    }
}