using UnityEngine;

public class AutoDestroyAfterAnimation : MonoBehaviour
{
    private Animator animator; // Animator���������
    private Renderer renderer; // ��Ⱦ�����������

    void Start()
    {
        animator = GetComponent<Animator>(); // ��ȡAnimator���
        renderer = GetComponent<Renderer>(); // ��ȡRenderer���

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

        // ����ı���ɫ
        ChangeColorRandomly();
    }

    void DestroyAfterAnimation()
    {
        Destroy(gameObject); // ����Ԥ�������
    }

    void ChangeColorRandomly()
    {
        if (renderer != null && renderer.material != null)
        {
            // �������ǳɫ
            float h = Random.Range(0f, 1f); // ���ɫ��
            float s = Random.Range(0.3f, 0.7f); // ������Ͷȣ��ϵ͵�ֵ����ӽ���ɫ��
            float v = Random.Range(0.7f, 1f); // ������ȣ��ϸߵ�ֵ����ӽ���ɫ��

            // ת��HSV��RGB
            Color randomColor = Color.HSVToRGB(h, s, v);

            // ������Ⱦ�����ʵ���ɫ
            renderer.material.color = randomColor;
        }
        else
        {
            Debug.LogWarning("Renderer or material not found. Cannot change color.");
        }
    }

}
