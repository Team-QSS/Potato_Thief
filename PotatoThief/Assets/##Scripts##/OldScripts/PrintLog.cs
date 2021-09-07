using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintLog : Singleton<PrintLog>
{
    public static string str = "Nothing";
    public Text t;

    private void Update()
    {
        t.text = str;
    }
}
