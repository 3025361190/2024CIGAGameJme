using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shake : MonoBehaviour
{
    public Text displayText;
    public Animator animator;
    public float switchInterval = 0.1f; // 切换间隔时间
    public int startNumber = 10; // 开始显示的数字
    public int endNumber = 100; // 最终显示的数字

    private int currentNumber;

    void Start()
    {
        currentNumber = startNumber;
        // 开始切换数字
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
