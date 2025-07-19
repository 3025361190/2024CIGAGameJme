using System;
using System.Threading;
using System.Threading.Tasks;
using TapTap.TapAd;
using TapTap.TapAd.Internal;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class AdInitializer : MonoBehaviour
{
    private Text infoText;
    void Start()
    {
        // 初始化广告SDK
        InitTapAdSdk();
    }
    private void ShowText(string content)
    {
        content = string.Format($"[Unity:TapAd] {content} | Time: {DateTime.Now.ToString("g")}");
        infoText.text = content;
        Debug.LogFormat(content);
    }
    private void InitTapAdSdk()
    {
     /*   // 创建广告配置
        TapAdConfig config = new TapAdConfig.Builder()
                .MediaId(1012520)             // 必选参数，为 TapADN 注册的媒体 ID
                .MediaName("吞吞爆爆火锅料")         // 必选参数，为 TapADN 注册的媒体名称
                .MediaKey("802RgVbwrAH2FdRdUlEnjXyfDD1kXUDHKlSSMxQ5ZWuCtTI3vD4UBXtUF0JAl8Vg")           // 必选参数，媒体密钥，可以在 TapADN 后台查看（用于传输数据的解密）
                .MediaVersion("1")                  // 必选参数，默认值 "1"
                .Channel("taptap")             // 必选参数，渠道（如果在 TapTap 上架填写 "taptap", 其它渠道上架填写商店拼音小写字母，eg. 小米 -> "xiaomi"）
                .TapClientId("755065")    // 可选参数，TapTap 开发者中心的游戏 Client ID 
                .EnableDebugLog(false)              // 可选参数，是否打开原生 debug 调试信息输出：true 打开、false 关闭。默认 false 关闭
                .ShakeEnabled(false)                // 可选参数，是否开启摇一摇: true 打开、false 关闭。
                .Build();*/
        TapAdConfig config = null;
        ICustomController customController = null;

        config = new TapAdConfig.Builder()
                  .MediaId(1012520)             // 必选参数，为 TapADN 注册的媒体 ID
                  .MediaName("吞吞爆爆火锅料")         // 必选参数，为 TapADN 注册的媒体名称
                  .MediaKey("802RgVbwrAH2FdRdUlEnjXyfDD1kXUDHKlSSMxQ5ZWuCtTI3vD4UBXtUF0JAl8Vg")           // 必选参数，媒体密钥，可以在 TapADN 后台查看（用于传输数据的解密）
                  .MediaVersion("1")                  // 必选参数，默认值 "1"
                  .Channel("taptap")             // 必选参数，渠道（如果在 TapTap 上架填写 "taptap", 其它渠道上架填写商店拼音小写字母，eg. 小米 -> "xiaomi"）
                  .TapClientId("755065")    // 可选参数，TapTap 开发者中心的游戏 Client ID 
                  .EnableDebugLog(false)              // 可选参数，是否打开原生 debug 调试信息输出：true 打开、false 关闭。默认 false 关闭
                  .ShakeEnabled(false)                // 可选参数，是否开启摇一摇: true 打开、false 关闭。
                  .Build();
     //   customController = new FormalCustomControllerWrapper(this);
        TapAdSdk.Init(config, customController);
        ShowText("初始化完毕");
    }

    private void OnAdSdkInited()
    {
        Debug.Log("广告SDK初始化成功，可以开始加载广告");
        // 在这里可以调用加载广告的方法
    }
}