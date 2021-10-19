using UnityEngine;
using UnityEngine.UI;

public class PrintLog : Singleton<PrintLog>
{
    [SerializeField]
    private Text _logText;

    public string LogString { 
        get => _logText.text;
        set => _logText.text = $"{value}\n";
    }
}
