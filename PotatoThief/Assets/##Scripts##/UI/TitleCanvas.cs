using System;
using Login;
using UnityEngine;

namespace UI
{
    public class TitleCanvas : MonoBehaviour
    {
        public void OnClickStartButton()
        {
            LoginManager.Instance.StartButtonDown();
        }
    }
}
