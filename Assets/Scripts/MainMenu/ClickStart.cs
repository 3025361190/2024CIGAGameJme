using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;  // 用于事件系统接口
using UnityEngine.UI;  // 用于UI组件

// 需要实现IPointerClickHandler接口来处理点击事件
public class ClickStart : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"开始按钮初始化: {gameObject.name}");
        
        // 检查是否在Canvas下
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas == null)
        {
            Debug.LogWarning("警告：开始按钮不在Canvas下，可能无法接收点击事件");
        }

        // 检查必要组件
        Image image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogWarning("警告：按钮缺少Image组件");
        }
        else
        {
            Debug.Log($"按钮raycastTarget状态: {image.raycastTarget}");
        }

        // 检查EventSystem
        if (FindObjectOfType<EventSystem>() == null)
        {
            Debug.LogError("场景中缺少EventSystem，UI事件将无法工作！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 处理点击事件
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"OnPointerClick被触发 - 点击位置: ({eventData.position.x}, {eventData.position.y})");
        
        // 检查GameManager是否存在
        if (GameManager.Instance != null)
        {
            Debug.Log("调用GameManager跳转到第一关");
            GameManager.Instance.jumpToLevel(1);
        }
        else
        {
            Debug.LogError("错误：GameManager实例不存在！");
        }
    }
}
