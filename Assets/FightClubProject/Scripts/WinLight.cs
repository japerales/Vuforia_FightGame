using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLight : MonoBehaviour {


    public Image[] winLights;
    public Sprite winLightSprite;
    public int Player;

    public void setWin(int winNumber)
    {
        if (winNumber > 0 && winNumber <= winLights.Length)
        {
            winLights[winNumber-1].sprite = winLightSprite;
        }
    }


}
