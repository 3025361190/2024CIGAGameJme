using System;
using System.Threading;
using System.Threading.Tasks;
using TapTap.TapAd;
using TapTap.TapAd.Internal;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public sealed class TapAdDemo : MonoBehaviour
{
    // banner 竖屏id
    private const int PORTRAIT_BANNER_ID = 1001201;
    // banner 横屏id
    private const int LANDSCAPE_BANNER_ID = 1001559;
    // rewardVideo 竖屏id
    private const int PORTRAIT_REWARD_ID = 1001253;
    // rewardVideo 横屏id
    private const int LANDSCAPE_REWARD_ID = 1001253;
    // splash 竖屏id
    private const int PORTRAIT_SPLASH_ID = 1001560;
    // splash 横屏id
    private const int LANDSCAPE_SPLASH_ID = 1001561;
    
    // interstitial 竖屏id
    private const int PORTRAIT_INTERSTITIAL_ID = 1000725;
    // interstitial 横屏id
    private const int LANDSCAPE_INTERSTITIAL_ID = 1001557;
    [SerializeField]
    private Button initButton;
    
    [SerializeField]
    private Button requestPermissionButton;
    
    [SerializeField]
    private Button uploadUserActionButton;
    
    [SerializeField]
    private Button loadRewardVideoVerticalButton;
    
    [SerializeField]
    private Button loadRewardVideoHorizontalButton;

    [SerializeField]
    private Button playRewardVideoButton;
    
    [SerializeField]
    private Button loadBannerVerticalButton;
    
    [SerializeField]
    private Button loadBannerHorizontalButton;
    [SerializeField]
    private InputField bannerBaselineInput;
    [SerializeField]
    private InputField bannerOffsetInput;
    
    [SerializeField]
    private Button playBannerButton;
    
    [SerializeField]
    private Button loadSplashVerticalButton;
    
    [SerializeField]
    private Button loadSplashHorizontalButton;
    
    [SerializeField]
    private Button playSplashButton;
    
    [SerializeField]
    private Button loadInterstitialVerticalButton;
    
    [SerializeField]
    private Button loadInterstitialHorizontalButton;
    
    [SerializeField]
    private Button playInterstitialButton;
    
    [SerializeField]
    private Text infoText;

    private TapRewardVideoAd _tapRewardAd;
    private TapBannerAd _tapBannerAd;
    private TapSplashAd _tapSplashAd;
    private TapInterstitialAd _tapInterstitialAd;
    
    // Unity 主线程ID:
    private static int mainThreadId;

    private void Awake()
    {
        mainThreadId = Thread.CurrentThread.ManagedThreadId;
        
        initButton.onClick.RemoveAllListeners();
        initButton.onClick.AddListener(Init);
        
        uploadUserActionButton.onClick.RemoveAllListeners();
        uploadUserActionButton.onClick.AddListener(OnClickUserActionButton);
        
        requestPermissionButton.onClick.RemoveAllListeners();
        requestPermissionButton.onClick.AddListener(RequestPermission);
        
        loadRewardVideoVerticalButton.onClick.RemoveAllListeners();
        loadRewardVideoVerticalButton.onClick.AddListener(()=> LoadRewardVideoAd(false));
        
        loadRewardVideoHorizontalButton.onClick.RemoveAllListeners();
        loadRewardVideoHorizontalButton.onClick.AddListener(()=> LoadRewardVideoAd(true));
        
        playRewardVideoButton.onClick.RemoveAllListeners();
        playRewardVideoButton.onClick.AddListener(PlayRewardVideoAd);
        
        loadBannerVerticalButton.onClick.RemoveAllListeners();
        loadBannerVerticalButton.onClick.AddListener(()=> LoadBannerAd(false));
        
        loadBannerHorizontalButton.onClick.RemoveAllListeners();
        loadBannerHorizontalButton.onClick.AddListener(()=> LoadBannerAd(true));
        
        playBannerButton.onClick.RemoveAllListeners();
        playBannerButton.onClick.AddListener(PlayBannerAd);
        
        loadSplashVerticalButton.onClick.RemoveAllListeners();
        loadSplashVerticalButton.onClick.AddListener(()=> LoadSplashAd(false));
        
        loadSplashHorizontalButton.onClick.RemoveAllListeners();
        loadSplashHorizontalButton.onClick.AddListener(()=> LoadSplashAd(true));
        
        playSplashButton.onClick.RemoveAllListeners();
        playSplashButton.onClick.AddListener(PlaySplashAd);
        
        loadInterstitialVerticalButton.onClick.RemoveAllListeners();
        loadInterstitialVerticalButton.onClick.AddListener(()=> LoadInterstitialAd(false));
        
        loadInterstitialHorizontalButton.onClick.RemoveAllListeners();
        loadInterstitialHorizontalButton.onClick.AddListener(()=> LoadInterstitialAd(true));
        
        playInterstitialButton.onClick.RemoveAllListeners();
        playInterstitialButton.onClick.AddListener(PlayInterstitialAd);
        
    }
    
    private void ShowText(string content)
    {
        content = string.Format($"[Unity:TapAd] {content} | Time: {DateTime.Now.ToString("g")}");
        infoText.text = content;
        Debug.LogFormat(content);
    }

    private void Init()
    {
        TapAdConfig config = null;
        ICustomController customController = null;
        
        config = new TapAdConfig.Builder()
            .MediaId(1000007)
            .MediaName("联盟测试-Android")
            .MediaKey("1AjDOjD0F3SDDmgTuBQHbCRULSizYPHV17viZObHvhDjf7Pq1rlarueOX1cYBucn")
            .EnableDebugLog(true)
            .MediaVersion("1")
            .Channel("taptap2")
            .TapClientId("0RiAlMny7jiz086FaU")
            .Build();
        customController = new FormalCustomControllerWrapper(this);
        TapAdSdk.Init(config, customController);
        ShowText("初始化完毕");
    }
    
    private void RequestPermission()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 需要先初始化!");
            return;
        }
        TapAdSdk.RequestPermissionIfNecessary();
        ShowText("请求权限");
    }

    private async void LoadRewardVideoAd(bool horizontal)
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 没有初始化!将自动初始化");
            Init();
            while (TapAdSdk.IsInited == false)
            {
                await Task.Yield();
            }
            ShowText("TapAd 初始化完毕");
        }
        if (_tapRewardAd != null)
        {
            _tapRewardAd.Dispose();
            _tapRewardAd = null;
        }
        int adId = horizontal ? LANDSCAPE_REWARD_ID : PORTRAIT_REWARD_ID;
        // create AdRequest
        var request = new TapAdRequest.Builder()
            .SpaceId(adId)
            .Extra1("{}")
            .RewardName("UnityRewardName")
            .RewardCount(9)
            .UserId("123")
            .Build();
        _tapRewardAd = new TapRewardVideoAd(request);
        _tapRewardAd.SetLoadListener(new RewardVideoAdLoadListener(this));
        _tapRewardAd.Load();
    }

    private void PlayRewardVideoAd()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 需要先初始化!");
            return;
        }
        if (_tapRewardAd != null)
        {
            _tapRewardAd.SetInteractionListener(new RewardVideoInteractionListener(this));
            _tapRewardAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] 未加载好视频,无法播放!");
        }
    }

    private async void LoadBannerAd(bool isHorizontal)
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 没有初始化!将自动初始化");
            Init();
            while (TapAdSdk.IsInited == false)
            {
                await Task.Yield();
            }
            ShowText("TapAd 初始化完毕");
        }
        if (_tapBannerAd != null)
        {
            _tapBannerAd.Dispose();
            _tapBannerAd = null;
        }
        
        int adId = isHorizontal ? LANDSCAPE_BANNER_ID : PORTRAIT_BANNER_ID;
        
        // create AdRequest
        var request = new TapAdRequest.Builder()
            .SpaceId(adId)
            .UserId("123")
            .Build();
        _tapBannerAd = new TapBannerAd(request);
        _tapBannerAd.SetLoadListener(new BannerAdLoadListener(this));
        _tapBannerAd.Load();
    }

    private void PlayBannerAd()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 需要先初始化!");
            return;
        }
        if (_tapBannerAd != null)
        {
            int baseLine = 0;
            int offset = 0;
            int.TryParse(bannerBaselineInput.text, out baseLine);
            if (baseLine != 0 && baseLine != 1)
            {
                Debug.LogErrorFormat("Banner Baseline 只能设置成 0 或者 1");
            }
            int.TryParse(bannerOffsetInput.text, out offset);
            _tapBannerAd.baseline = baseLine;
            _tapBannerAd.offset = offset;
            _tapBannerAd.SetInteractionListener(new BannerInteractionListener(this));
            _tapBannerAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] 未加载好视频,无法播放!");
        }
    }
    
    private async void LoadSplashAd(bool isHorizontal)
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 没有初始化!将自动初始化");
            Init();
            while (TapAdSdk.IsInited == false)
            {
                await Task.Yield();
            }
            ShowText("TapAd 初始化完毕");
        }
        if (_tapSplashAd != null)
        {
            _tapSplashAd.Dispose();
            _tapSplashAd = null;
        }

        int adId = isHorizontal ? LANDSCAPE_SPLASH_ID : PORTRAIT_SPLASH_ID;
        // create AdRequest
        var request = new TapAdRequest.Builder()
            .SpaceId(adId)
            .Extra1("{}")
            .UserId("123")
            .Build();
        _tapSplashAd = new TapSplashAd(request);
        _tapSplashAd.SetLoadListener(new SplashAdLoadListener(this));
        _tapSplashAd.Load();
    }

    private void PlaySplashAd()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 需要先初始化!");
            return;
        }
        if (_tapSplashAd != null)
        {
            _tapSplashAd.SetInteractionListener(new SplashInteractionListener(this));
            _tapSplashAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] 未加载好视频,无法播放!");
        }
    }
    
    private async void LoadInterstitialAd(bool isHorizontal)
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 没有初始化!将自动初始化");
            Init();
            while (TapAdSdk.IsInited == false)
            {
                await Task.Yield();
            }
            ShowText("TapAd 初始化完毕");
        }
        if (_tapInterstitialAd != null)
        {
            _tapInterstitialAd.Dispose();
            _tapInterstitialAd = null;
        }

        int adId = isHorizontal ? LANDSCAPE_INTERSTITIAL_ID : PORTRAIT_INTERSTITIAL_ID;
        // create AdRequest
        var request = new TapAdRequest.Builder()
            .SpaceId(adId)
            .Extra1("{}")
            .UserId("123")
            .Build();
        _tapInterstitialAd = new TapInterstitialAd(request);
        _tapInterstitialAd.SetLoadListener(new InterstitialAdLoadListener(this));
        _tapInterstitialAd.Load();
    }

    private void PlayInterstitialAd()
    {
        if (TapAdSdk.IsInited == false)
        {
            ShowText("TapAd 需要先初始化!");
            return;
        }
        if (_tapInterstitialAd != null)
        {
            _tapInterstitialAd.SetInteractionListener(new InterstitialAdInteractionListener(this));
            _tapInterstitialAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] 未加载好视频,无法播放!");
        }
    }
    
    private void OnClickUserActionButton()
    {
        var userActions = new UserAction[3];

        var jan1st1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        for (int i = 0 ; i < 3; i ++)
        {
            var tmp = new UserAction(actionType: i, actionTime: (long)(DateTime.UtcNow - jan1st1970).TotalMilliseconds,
                amount: i * 1000, winStatus: i % 2);
            userActions[i] = tmp;
        }
        TapAdSdk.UploadUserAction(userActions, new CustomUserAction(this));
    }
    
    public sealed class CustomUserAction : IUserAction
    {
        private readonly TapAdDemo example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public CustomUserAction(TapAdDemo context)
        {
            example = context;
        }

        public void OnSuccess()
        {
            example.ShowText($"上报成功");
        }

        public void OnError(int code, string message)
        {
            example.ShowText($"上报失败 code: {code} message: {message}");
        }
    }

    public sealed class FormalCustomControllerWrapper : ICustomController
    {
        private readonly TapAdDemo example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public FormalCustomControllerWrapper(TapAdDemo context)
        {
            example = context;
        }

        public bool CanUseLocation => false;

        public TapAdLocation GetTapAdLocation => null;
        
        public bool CanUsePhoneState => false;
        public string GetDevImei => "";
        public bool CanUseWifiState => false;
        public bool CanUseWriteExternal => false;
        public string GetDevOaid => null;
        public bool Alist => false;
        public bool CanUseAndroidId => false;
        public CustomUser ProvideCustomer() => null;
    }
    
    public sealed class RewardVideoAdLoadListener : IRewardVideoAdLoadListener
    {
        private readonly TapAdDemo example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public RewardVideoAdLoadListener(TapAdDemo context)
        {
            example = context;
        }

        public void OnError(int code, string message)
        {
            message = message ?? "NULL";
            Debug.LogErrorFormat($"加载激励视频错误! 错误 code: {code} message: {message}");
        }

        public void OnRewardVideoAdCached(TapRewardVideoAd ad)
        {
            example.ShowText($"{ad.AdType}素材 Cached 完毕! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Cached ad.IsReady");
        }
        
        public void OnRewardVideoAdLoad(TapRewardVideoAd ad)
        {
            example.ShowText($"{ad.AdType}素材 Load 完毕! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Load ad.IsReady");
        }
    }
    
    public sealed class RewardVideoInteractionListener : IRewardVideoInteractionListener
    {
        private readonly TapAdDemo example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public RewardVideoInteractionListener(TapAdDemo context)
        {
            example = context;
        }

        public void OnAdShow(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdShow");
        }

        public void OnAdClose(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdClose");
        }

        public void OnVideoComplete(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnVideoComplete");
        }

        public void OnVideoError(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnVideoError");
        }

        public void OnRewardVerify(TapRewardVideoAd ad, bool rewardVerify, int rewardAmount, string rewardName, int code, string msg)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnRewardVerify rewardVerify: {rewardVerify} rewardAmount: {rewardAmount} rewardName: {rewardName} code: {code} msg: {msg}");
        }

        public void OnSkippedVideo(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnSkippedVideo");
        }

        public void OnAdClick(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdClick");
            Debug.LogErrorFormat($"激励视频 点击");
        }

        public void OnAdValidShow(TapRewardVideoAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdValidShow");
        }
    }
    
    public sealed class BannerAdLoadListener : IBannerAdLoadListener
    {
        private readonly TapAdDemo example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public BannerAdLoadListener(TapAdDemo context)
        {
            example = context;
        }

        public void OnError(int code, string message)
        {
            message = message ?? "NULL";
            Debug.LogErrorFormat($"加载Banner错误! 错误 code: {code} message: {message}");
        }
        
        public void OnBannerAdLoad(TapBannerAd ad)
        {
            example.ShowText($"{ad.AdType}广告 Loaded 完毕! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Loaded ad.IsReady");
        }
    }
    
    public sealed class BannerInteractionListener : IBannerAdInteractionListener
    {
        private readonly TapAdDemo example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public BannerInteractionListener(TapAdDemo context)
        {
            example = context;
        }
        
        public void OnAdShow(TapBannerAd ad) {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdShow");
        }

        public void OnAdClose(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdClose");
        }

        public void OnAdClick(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdClick");
        }

        public void OnDownloadClick(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnDownloadClick");
        }

        public void OnAdValidShow(TapBannerAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdValidShow");
        }
    }
    
    public sealed class SplashAdLoadListener : ISplashAdLoadListener
    {
        private readonly TapAdDemo example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public SplashAdLoadListener(TapAdDemo context)
        {
            example = context;
        }

        public void OnError(int code, string message)
        {
            message = message ?? "NULL";
            Debug.LogErrorFormat($"加载Splash错误! 错误 code: {code} message: {message}");
        }
        
        public void OnSplashAdLoad(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}广告 OnSplashAdLoad! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Loaded ad.IsReady");
        }
    }
    
    public sealed class SplashInteractionListener : ISplashAdInteractionListener
    {
        private readonly TapAdDemo example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public SplashInteractionListener(TapAdDemo context)
        {
            example = context;
        }
        
        public void OnAdSkip(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdSkip");
            ad.Dispose();
        }

        public void OnAdTimeOver(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdTimeOver");
            ad.Dispose();
        }

        public void OnAdClick(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdClick");
        }

        public void OnAdShow(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdShow");
        }

        public void OnAdValidShow(TapSplashAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType} OnAdValidShow");
        }
    }

    public sealed class InterstitialAdLoadListener : IInterstitialAdLoadListener
    {
        private readonly TapAdDemo example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public InterstitialAdLoadListener(TapAdDemo context)
        {
            example = context;
        }

        public void OnError(int code, string message)
        {
            message = message ?? "NULL";
            Debug.LogErrorFormat($"加载Splash错误! 错误 code: {code} message: {message}");
        }

        public void OnInterstitialAdLoad(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}广告 OnInterstitialAdLoad! ad != null: {(ad != null).ToString()} Thread On MainThread: {Thread.CurrentThread.ManagedThreadId == mainThreadId}");
            Assert.IsTrue(ad.IsReady, "Loaded ad.IsReady");
        }
        
    }
    
    public sealed class InterstitialAdInteractionListener : IInterstitialAdInteractionListener
    {
        private readonly TapAdDemo example;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public InterstitialAdInteractionListener(TapAdDemo context)
        {
            example = context;
        }
        
        public void OnAdShow(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdShow");
        }

        public void OnAdClose(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdClose");
        }

        public void OnAdError(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdError");
        }

        public void OnAdValidShow(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdValidShow");
        }

        public void OnAdClick(TapInterstitialAd ad)
        {
            example.ShowText($"[Unity::AD] {ad.AdType}视频 OnAdClick");
        }
    }
}
