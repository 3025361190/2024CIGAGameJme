using UnityEngine;

public class ClickAnimationTrigger : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // µ±×ó¼üµã»÷Ê±
        {
            animator.SetTrigger("Switch");
        }
    }

}
