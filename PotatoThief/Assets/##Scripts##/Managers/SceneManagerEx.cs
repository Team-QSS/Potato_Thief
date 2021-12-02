using InGame;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    Lobby,
    Ready,
    InGame,
    Account
}

public class SceneManagerEx : Singleton<SceneManagerEx>
{
    public SceneType CurrentSceneType 
        => (SceneType)SceneManager.GetActiveScene().buildIndex;

    public GameEndType gameEndType = GameEndType.None;
    public long playTime;
    
    public void LoadScene(SceneType type)
    {
        PhotonNetwork.LoadLevel((int) type);
    }
    
    public void LoadScene(SceneType type, GameEndType gameEndType)
    {
        this.gameEndType = gameEndType;
        PhotonNetwork.LoadLevel((int) type);
    }
}
