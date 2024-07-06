using UnityEngine;

public class AutoDestroyAfterAnimation : MonoBehaviour
{
    private Animator animator; // Animator组件的引用

    void Start()
    {
        animator = GetComponent<Animator>(); // 获取Animator组件
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
    }

    void DestroyAfterAnimation()
    {
        Destroy(gameObject); // 销毁预制体对象
    }
}
