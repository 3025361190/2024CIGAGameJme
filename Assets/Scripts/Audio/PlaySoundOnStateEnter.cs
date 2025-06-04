using UnityEngine;

public class PlaySoundOnStateEnter : StateMachineBehaviour
{
    public AudioClip clip;
    private AudioSource audioSource;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (audioSource == null)
        {
            audioSource = animator.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                // 可选：自动添加AudioSource组件
                audioSource = animator.gameObject.AddComponent<AudioSource>();
            }
        }

        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}