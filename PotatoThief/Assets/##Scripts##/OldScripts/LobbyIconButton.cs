using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyIconButton : MonoBehaviour
{
     public Sprite[] mapThumbnails;
    [SerializeField] Text mapText;

    public void InitializeObject(int mapImageIndex, string mapName)
    {
        gameObject.GetComponent<Image>().sprite = mapThumbnails[mapImageIndex];
        mapText.text = mapName;
    }

    public void IsButtonClicked()
    {
        print("Connect Room");
    }
}
