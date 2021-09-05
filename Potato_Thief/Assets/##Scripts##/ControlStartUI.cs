using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlStartUI : MonoBehaviour
{
    public GameObject StartUI;
    
    [SerializeField] private Image backGround;
    [SerializeField] private GameObject cover;
    [SerializeField] private Text clickMessage;

    bool isClicked;
    bool isCallNextScene;
    bool isAlphaPlus;
    float changeValue;

    private void Start()
    {
        changeValue = 0.05f;
        isAlphaPlus = false;
        isClicked = false;
        isCallNextScene = false;
    }

    private void FixedUpdate()
    {
        if (!isClicked)
            ChangeTextColor();
    }
    private void ChangeTextColor()
    {
        if (isAlphaPlus) 
            FadeIn();
        else 
            FadeOut();
    }

    private void FadeOut()
    {
        Color color = clickMessage.color;
        if (color.a > 0.05f)
            color.a -= changeValue;
        else
        {
            color.a = 0f;
            isAlphaPlus = true;
        }
        clickMessage.color = color;
    }

    private void FadeIn()
    {
        Color color = clickMessage.color;
        if (color.a < 0.95f)
            color.a += changeValue;
        else
        {
            color.a = 1f;
            isAlphaPlus = false;
        }
        clickMessage.color = color;
    }

    public void IsButtonDown()
    {
        if (!isCallNextScene)
        {
            isClicked = true;

            for (float i = clickMessage.color.a; i > 0.05f; i -= 0.05f)
            {
                Color c = clickMessage.color;
                c.a = i;
                clickMessage.color = c;
            }
            Color c2 = clickMessage.color;
            c2.a = 0;
            clickMessage.color = c2;

            isCallNextScene = true;
            Invoke(nameof(LoadNextScene), 1f);
        }
    }

    private void LoadNextScene()
    {
        // Scene Load
        // Application.Quit();
    }
}
