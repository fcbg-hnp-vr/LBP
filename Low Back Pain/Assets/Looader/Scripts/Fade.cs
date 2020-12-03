///////////////////////////////////////////////////////////
//                                                        //
//  Script Fade (effet de transparence)
//                                                        //
//                                                        //
///////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {


    public Texture fadeOutTexture; //texture pour le overlay sur l'écran
    public float fadeSpeed = 0.4f; //la vitesse du fading

    private int drawDepth = -1000; 
    public float alpha = 1.0f; //alpha de la texture
    public int fadeDir = -1; // la direction du fade 


    public void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float DebutFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

}
