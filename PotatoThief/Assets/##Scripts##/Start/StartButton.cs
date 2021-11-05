using System.Collections;
using System.Collections.Generic;
using Login;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void ButtonDown()
    {
        LoginManager.instance.StartButtonDown();
    }
}
