using InGame;
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

    public object SceneLoadInfo;
    public GameEndType gameEndType = GameEndType.None;

    public void LoadScene(SceneType type)
    {
        SceneManager.LoadScene((int) type);
    }
    
    public void LoadScene(SceneType type, GameEndType gameEndType, object info)
    {
        SceneLoadInfo = info;
        this.gameEndType = gameEndType;
        SceneManager.LoadScene((int) type);
    }
}
