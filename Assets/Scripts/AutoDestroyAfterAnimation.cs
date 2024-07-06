using UnityEngine;

public class AutoDestroyAfterAnimation : MonoBehaviour
{
    private Animator animator; // Animator组件的引用
    private Renderer renderer; // 渲染器组件的引用

    void Start()
    {
        animator = GetComponent<Animator>(); // 获取Animator组件
        renderer = GetComponent<Renderer>(); // 获取Renderer组件

        if (animator == null)
        {
            Debug.LogError("No Animator component found on this GameObject.");
            Destroy(gameObject); // 如果没有Animator组件，则直接销毁该对象
            return;
        }

        // 订阅动画状态机的动画播放完成事件
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float clipLength = stateInfo.length; // 获取当前动画片段的长度

        Invoke("DestroyAfterAnimation", clipLength); // 延迟销毁

        // 随机改变颜色
        ChangeColorRandomly();
    }

    void DestroyAfterAnimation()
    {
        Destroy(gameObject); // 销毁预制体对象
    }

    void ChangeColorRandomly()
    {
        if (renderer != null && renderer.material != null)
        {
            // 生成随机浅色
            float h = Random.Range(0f, 1f); // 随机色调
            float s = Random.Range(0.3f, 0.7f); // 随机饱和度（较低的值会更接近灰色）
            float v = Random.Range(0.7f, 1f); // 随机亮度（较高的值会更接近白色）

            // 转换HSV到RGB
            Color randomColor = Color.HSVToRGB(h, s, v);

            // 设置渲染器材质的颜色
            renderer.material.color = randomColor;
        }
        else
        {
            Debug.LogWarning("Renderer or material not found. Cannot change color.");
        }
    }

}
