using System;
using UnityEngine;

namespace UI
{
    public class BGMPlayer : MonoBehaviour
    {
        [SerializeField] private BGMType clipType;
        
        private void Start()
        {
            AudioManager.Instance.PlayBGM(clipType);
        }
    }
}