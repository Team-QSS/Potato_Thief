using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager canvasManager = null;
    // Start is called before the first frame update
    void Start()
    {
        if (canvasManager == null)
            canvasManager = this;
    }

    public void LobbySetActiveOn()
    {

    }
}
