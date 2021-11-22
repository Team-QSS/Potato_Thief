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

    public GameEndType gameEndType = GameEndType.None;
    public long playTime;
    
    public void LoadScene(SceneType type)
    {
        SceneManager.LoadScene((int) type);
    }
    
    public void LoadScene(SceneType type, GameEndType gameEndType)
    {
        this.gameEndType = gameEndType;
        SceneManager.LoadScene((int) type);
    }
}
