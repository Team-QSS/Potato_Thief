using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private GameObject[] leftHearts;
        [SerializeField] private GameObject[] rightHearts;
        [SerializeField] private Text limitTimeText;
        [SerializeField] private Text statusText;

        private const string LimitTimeTextFormat = "{0} 초";

        public void UpdateLeftHeart(int number)
        {
            foreach (var heart in leftHearts)
            {
                heart.SetActive(false);
            }

            for (int i = 0; i < number; i++)
            {
                leftHearts[i].SetActive(true);
            }
        }
        
        public void UpdateRightHearts(int number)
        {
            foreach (var heart in rightHearts)
            {
                heart.SetActive(false);
            }

            for (int i = 0; i < number; i++)
            {
                rightHearts[i].SetActive(true);
            }
        }

        public void UpdateLimitTimeText(float time)
        {
            limitTimeText.text = string.Format(LimitTimeTextFormat, Mathf.Round(time));
        }

        public void UpdateStatusText(string message)
        {
            statusText.text += message;
        }
    }
}