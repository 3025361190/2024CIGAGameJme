using UnityEngine;

public class AutoDestroyAfterAnimation : MonoBehaviour
{
    private Animator animator; // Animator组件引用
    private new Renderer renderer; // 渲染器组件引用

    void Start()
    {
        animator = GetComponent<Animator>(); // 获取Animator组件
        renderer = GetComponent<Renderer>(); // 获取Renderer组件

        if (animator == null)
        {
            Debug.LogError("No Animator component found on this GameObject.");
            Destroy(gameObject); // 如果没有Animator组件，直接销毁这个游戏对象
            return;
        }

        // 获取当前动画状态的动画信息
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float clipLength = stateInfo.length; // 获取前一帧动画片段的长度

        Invoke("DestroyAfterAnimation", clipLength); // 延迟调用

        // 改变颜色
        ChangeColorRandomly();
    }

    void DestroyAfterAnimation()
    {
        Destroy(gameObject); // 预先销毁这个游戏对象
    }

    void ChangeColorRandomly()
    {
        if (renderer != null && renderer.material != null)
        {
            // 随机生成颜色
            float h = Random.Range(0f, 1f); // 色相
            float s = Random.Range(0.3f, 0.7f); // 饱和度，取值越低越接近灰色
            float v = Random.Range(0.7f, 1f); // 亮度，取值越高越接近白色

            // 将HSV转换为RGB
            Color randomColor = Color.HSVToRGB(h, s, v);

            // 将渲染器材质的颜色改为随机颜色
            renderer.material.color = randomColor;
        }
        else
        {
            Debug.LogWarning("Renderer or material not found. Cannot change color.");
        }
    }

}
