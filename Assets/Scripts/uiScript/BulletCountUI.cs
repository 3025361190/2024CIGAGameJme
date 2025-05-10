using UnityEngine;
using UnityEngine.UI;

public class BulletCountUI : MonoBehaviour
{
    public Text bulletCountText;  // 引用 UI Text 组件
    public GameObject targetGameObject;  // 引用包含 PlayerController 组件的目标 GameObject
    private PlayerController PlayerController;  // 引用 PlayerController 脚本

    void Start()
    {
        // 获取目标 GameObject 上的 PlayerController 组件
        if (targetGameObject != null)
        {
            PlayerController = targetGameObject.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Target GameObject not assigned!");
        }

        // 获取 UI Text 组件
        bulletCountText = GetComponent<Text>();
    }

    void Update()
    {
        // 每帧更新显示的子弹数量
        if (PlayerController != null && bulletCountText != null)
        {
            // 从 ShootBullet 脚本的 GetBulletMount 函数获取子弹数量
            int bulletCount = PlayerController.GetRemainingBullets();

            // 更新 UI Text 显示的文本内容
            bulletCountText.text = bulletCount.ToString();
        }
    }
}
