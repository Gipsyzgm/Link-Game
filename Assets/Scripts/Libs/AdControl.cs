using UnityEngine;
using TTSDK;
using TTSDK.UNBridgeLib.LitJson;
using UnityEngine.Serialization;

public class AdControl : MonoBehaviour
{
    public static AdControl instance;     // instance of this class
    void Awake()
    {
        if (instance == null)
        {
            // Makes the object target not be destroyed automatically when loading a new scene
            DontDestroyOnLoad(gameObject); 
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        TT.InitSDK();
    }
    
    private TTRewardedVideoAd _rvideoAd;
    private TTBannerStyle m_style = new TTBannerStyle();
    private TTBannerAd _bannerAd;
    public int rewardType = 0;
    /// <summary>
    /// 时间间隔
    /// </summary>
    public int adInterval = 5;
    
    public string bannerAdID = "an14ib5i6ueijn10kf";
    public string videoAdID = "e2hn7qqic34pda4hik";
    public void CreatVideoAd()
    {
        string videoAdId = videoAdID;
        var param = new CreateRewardedVideoAdParam { AdUnitId = videoAdId };
        _rvideoAd = TT.CreateRewardedVideoAd(param);
        _rvideoAd.OnClose += OnVideoAdClose;
        _rvideoAd.OnError +=OnVideoAdError;
        _rvideoAd.OnLoad +=OnVideoAdLoaded;
    }

    public void LoadVideoAd()
    {
        _rvideoAd.Load();
    }
    public void ShowVideoAd(int rewardType = 0)
    {
        _rvideoAd.Show();
    }
    void OnVideoAdClose(bool ended, int count)
    {
        Debug.Log($"激励视频关闭 ended: {ended}, count: {count}");
        if (ended)
        {
            Debug.Log($"激励视频奖励类型为{rewardType}");
            if (rewardType == 0)
            {
                if (DataLoader.Data)
                { 
                    DataLoader.Data.VideoAdBtn.SetActive(false);
                    DataLoader.Data.UnlockNewLevel();
                    Invoke(nameof(DoSomething), adInterval);

                   
                }
            }
        }
        else
        {
            Debug.Log($"激励视频奖励不可用");
        }
     
    }
    void DoSomething()
    {
       
        if (DataLoader.Data)
        { 
            Debug.Log("Invoke延时触发");
            DataLoader.Data.VideoAdBtn.SetActive(true);
        }
        else
        {
            Debug.Log("找不到不触发");
        }
        
    }
    
    void OnVideoAdError(int iErrCode, string errMsg)
    {
        Debug.Log($"激励视频错误 errorCode: {iErrCode}");
    }
    
    void OnVideoAdLoaded()
    {
        Debug.Log($"激励视频加载成功");
    }
    
    public void CreatBannerAd()
    {
        m_style.top = 10;
        m_style.left = 10;
        m_style.width = 320;

        if (_bannerAd != null && _bannerAd.IsInvalid())
        {
            _bannerAd.Destroy();
            _bannerAd = null;
        }
        if (_bannerAd == null)
        {
            var param = new CreateBannerAdParam
            {
                BannerAdId = bannerAdID,
                Style = m_style,
                AdIntervals = 60
            };
            _bannerAd = TT.CreateBannerAd(param);
            _bannerAd.OnError += OnBannerAdError;
            _bannerAd.OnClose += OnBannerClose;
            _bannerAd.OnResize += OnBannerResize;
            _bannerAd.OnLoad += OnBannerLoaded;
        }
    }
    
    
    void OnBannerAdError(int iErrCode, string errMsg)
    {
        Debug.LogError("错误 ： " + iErrCode + "  " + errMsg);
    }

    private void OnBannerLoaded()
    {
        _bannerAd?.Show();
    }

    private void OnBannerResize(int width, int height)
    {
        Debug.Log($"OnBannerResize - width:{width} height:{height}");
    }

    private void OnBannerClose()
    {
        Debug.Log("banner广告关闭");
    }
    //展示
    public void ShowBannerAd()
    {
        _bannerAd?.Show();
    }

    //隐藏
    public void HideBannerAd()
    {
        _bannerAd?.Hide();
    }
    //修改尺寸
    private void ResizeBannerAd()
    {
        m_style.top = int.Parse("10");
        m_style.left = int.Parse("10");
        m_style.width = int.Parse("320");
        _bannerAd?.ReSize(m_style);
    }
    //销毁广告实例
    private void DestroyBannerAd()
    {
        _bannerAd?.Destroy();
    }
    void Start()
    {
        CreatVideoAd();
        CreatBannerAd();
        TT.CheckScene(TTSideBar.SceneEnum.SideBar, b =>
        {
            Debug.Log("check scene success，"+b );
        }, () =>
        {
            Debug.Log("check scene complete");
        }, (errCode, errMsg) =>
        {
            Debug.Log($"check scene error, errCode:{errCode}, errMsg:{errMsg}");
        });
    }
    /// <summary>
    /// 打开侧边栏
    /// </summary>
    public void OpenSideSlider()
    {
        Debug.LogError("点击 navigate to scene");
       var data = new JsonData
       {
           ["scene"] = "sidebar",
       };
       TT.NavigateToScene(data, () =>
       {
           Debug.Log("navigate to scene success");
       }, () =>
       {
           Debug.Log("navigate to scene complete");
       }, (errCode,errMsg) =>
       {
           Debug.Log($"navigate to scene error, errCode:{errCode}, errMsg:{errMsg}");
       });
    }
}
