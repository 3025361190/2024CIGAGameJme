using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static huanxue;

public class kouxue : MonoBehaviour
{
    private huanxue healthBar;
    public GameObject quanquan; // 声明公共变量以便在Unity Inspector中进行绑定

    void Start()
    {
        healthBar = GetComponent<huanxue>(); // 获取附加到同一对象上的HealthBar组件
    }

    void Update()
    {
        // 在 Update 方法中更新 HealthBar，模拟时间流逝
      //  healthBar.Update(Time.deltaTime);
    }

    public void ButtonClickTakeDamage()
    {
        // 当按钮被点击时，扣除一点血量
        healthBar.TakeDamage(1);
    }
}

