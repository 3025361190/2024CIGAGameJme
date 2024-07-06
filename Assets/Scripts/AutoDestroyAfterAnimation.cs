using UnityEngine;

public class AutoDestroyAfterAnimation : MonoBehaviour
{
    private Animator animator; // Animator���������

    void Start()
    {
        animator = GetComponent<Animator>(); // ��ȡAnimator���
        if (animator == null)
        {
            Debug.LogError("No Animator component found on this GameObject.");
            Destroy(gameObject); // ���û��Animator�������ֱ�����ٸö���
            return;
        }

        // ���Ķ���״̬���Ķ�����������¼�
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float clipLength = stateInfo.length; // ��ȡ��ǰ����Ƭ�εĳ���

        Invoke("DestroyAfterAnimation", clipLength); // �ӳ�����
    }

    void DestroyAfterAnimation()
    {
        Destroy(gameObject); // ����Ԥ�������
    }
}
