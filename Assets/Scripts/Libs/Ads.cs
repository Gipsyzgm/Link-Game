using UnityEngine;
using System.Collections;
public class Ads : MonoBehaviour
{

    string ModeName;        
    void Start()
    {
        if (PLayerInfo.MODE == 1)
            ModeName = "ARCADE ";
        else
            ModeName = "CLASSIC ";
        MusicController.Music.BG_play();
        // show banner
        //AdControl.instance.ShowBannerAd();

        // request Google Analytics
        //AdmobGA.load.GA.LogScreen(ModeName + "Level: " + PLayerInfo.MapPlayer.Level);
    }

}
