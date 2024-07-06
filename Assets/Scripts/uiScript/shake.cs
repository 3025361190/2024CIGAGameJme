using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shake : MonoBehaviour
{
    public Text displayText;
    public Animator animator;
    public float switchInterval = 0.1f; // �л����ʱ��
    public int startNumber = 10; // ��ʼ��ʾ������
    public int endNumber = 100; // ������ʾ������

    private int currentNumber;

    void Start()
    {
        currentNumber = startNumber;
        // ��ʼ�л�����
        StartCoroutine(SwitchNumbers());
    }

    IEnumerator SwitchNumbers()
    {
        int count = 0;

        while ( currentNumber < endNumber)
        {
            displayText.text = currentNumber.ToString();
            yield return new WaitForSeconds(switchInterval);

        if (currentNumber < endNumber)
            {
                    currentNumber = currentNumber + ((endNumber - startNumber) / 4);
                }

        }
    }
}
