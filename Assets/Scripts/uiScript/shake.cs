using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shake : MonoBehaviour
{
    public Text displayText;
    public Animator animator;
    public float switchInterval = 0.1f; 
    public int startNumber = 10; 
    public int endNumber = 100; 

    private int currentNumber;

    void Start()
    {
        currentNumber = startNumber;
        // ¿ªÊ¼ÇÐ»»Êý×Ö
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
