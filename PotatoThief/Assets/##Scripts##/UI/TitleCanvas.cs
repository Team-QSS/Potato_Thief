using UnityEngine;

namespace UI
{
    public class TitleCanvas : MonoBehaviour
    {
        public void OnClickStartButton()
        {
            SceneManagerEx.Instance.LoadScene(SceneType.Lobby);
        }
    }
}
