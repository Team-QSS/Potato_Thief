using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUiManager : MonoBehaviour
{
    public GameObject lobbyIconPrefab;
    public GameObject chatTestPrefab;

    public Transform lobbyContent;
    public Transform chatContent;

    public GameObject InstantiateLobbyIcon()
    {
        return Instantiate(lobbyIconPrefab, lobbyContent);
    }
    public GameObject InstantiateChatText()
    {
        return Instantiate(chatTestPrefab, chatContent);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
