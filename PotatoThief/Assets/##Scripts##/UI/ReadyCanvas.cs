using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyCanvas : MonoBehaviour
{
    [SerializeField] private int playerCount;
    
    public void OnClickBackButton()
    {
        // 대충 포톤 스크립트
        SceneManagerEx.Instance.LoadScene(SceneType.Lobby);
    }

    public void OnClickStartButton()
    {
        if (playerCount == 2) return;
        
        // 대충 포톤 스크립트
        SceneManagerEx.Instance.LoadScene(SceneType.InGame);
    }
    
    
    
}
