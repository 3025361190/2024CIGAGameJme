using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeShootingModeToYaogan : MonoBehaviour, IPointerClickHandler
{
    void Start()
    {
        // 检查是否在Canvas下
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas == null)
        {
            Debug.LogWarning("警告：切换射击模式按钮不在Canvas下，可能无法接收点击事件");
        }

        // 检查EventSystem
        if (FindObjectOfType<EventSystem>() == null)
        {
            Debug.LogError("场景中缺少EventSystem，UI事件将无法工作！");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeShootingModeToJoystick();
        }
        else
        {
            Debug.LogError("错误：GameManager实例不存在！");
        }
    }
}
