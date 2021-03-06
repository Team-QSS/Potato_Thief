using UnityEngine;

namespace UI
{
    public class SettingsPanel : MonoBehaviour
    {
        public void OnBGMSliderValueChanged(float value)
        {
            AudioManager.Instance.BGMVolume = value;
        }

        public void OnSFXSliderValueChanged(float value)
        {
            AudioManager.Instance.SFXVolume = value;
        }

        public void OnClickExitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); //어플리케이션 종료
#endif
        }
    }
}