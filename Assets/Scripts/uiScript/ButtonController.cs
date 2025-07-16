using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button targetButton; // 需要调用的目标按钮

    public void OnButtonClick()
    {
        // 直接调用目标按钮的点击事件
        targetButton.onClick.Invoke();
    }
}