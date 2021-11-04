using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _creatRoomPopupScreen;
    void Start()
    {
        _creatRoomPopupScreen.SetActive(false);
    }
}
