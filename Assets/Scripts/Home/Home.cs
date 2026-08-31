using UnityEngine;

public class Home : MonoBehaviour
{
    void Start()
    {
        //AdControl.instance.HideBannerAd();
        MusicController.Music.BG_menu();
    }

    void Update()
    {
        // Exit game if click Escape key or back on mobile
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitOk();
        }
    }

    /// <summary>
    /// Exit game
    /// </summary>
    public void ExitOk()
    {
        Application.Quit();
    }

}
