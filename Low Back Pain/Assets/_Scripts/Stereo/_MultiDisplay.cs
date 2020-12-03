using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MultiDisplay : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        /*int width, height, refreshRate;
        width = Screen.width;
        height = Screen.height;
        refreshRate = 120;*/

        for (int i = 1; i < Display.displays.Length; i++)
        {
           // Display.displays[i].Activate(width, height, refreshRate);
            Display.displays[i].Activate();
        }
    }
}
