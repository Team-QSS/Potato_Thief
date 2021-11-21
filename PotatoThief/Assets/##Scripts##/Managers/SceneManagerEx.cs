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


    public void LoadScene(SceneType type)
    {
        SceneManager.LoadScene((int) type);
    }
}
