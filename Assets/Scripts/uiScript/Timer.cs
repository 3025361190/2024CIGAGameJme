/*
文件名：Timer.cs
编辑人：fortunate瑞
文件描述：Timer类，用于管理游戏中的计时器，显示剩余时间，并在时间结束时触发相应事件
绑定：
1. 在场景中添加timer物体
2. 绑定Timer脚本
3. 添加TextMeshProUGUI组件

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText; // 时间显示文本UI，代码中绑定
    public float maxTime = 100f; // 默认时间（秒）
    
    public float currentTime;
    private bool isPaused = false;       // 是否暂停
    private bool isRunning = true;      // 是否正在运行（计时器总开关）

    // awake is called when the script is loaded
    void Awake()
    {
        maxTime = GameManager.Instance.currentLevelConfig.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTime = maxTime;
        // 绑定timerText
        timerText = GetComponent<TextMeshProUGUI>();
        UpdateTimerDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning || isPaused) return;

        currentTime -= Time.deltaTime;
        
        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;
            GameManager.Instance.isTimeOut = true;
        }

        UpdateTimerDisplay();
    }

    // 更新计时器显示
    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            // int minutes = Mathf.FloorToInt(currentTime / 60);
            // int seconds = Mathf.FloorToInt(currentTime % 60);
            // timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            // 保留整数
            timerText.text = Mathf.CeilToInt(currentTime).ToString();
        }
    }

    // 暂停计时器
    public void PauseTimer()
    {
        isPaused = true;
    }

    // 继续计时器
    public void ResumeTimer()
    {
        isPaused = false;
    }

    // 重置计时器
    public void ResetTimer()
    {
        currentTime = maxTime;
        isRunning = true;
        isPaused = false;
        UpdateTimerDisplay();
    }

    // 设置新的时间
    public void SetTime(float newTime)
    {
        maxTime = newTime;
        currentTime = newTime;
        UpdateTimerDisplay();
    }
}
