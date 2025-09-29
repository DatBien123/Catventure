using System.Collections;
using UnityEngine;

public class UIHome : MonoBehaviour
{
    public Animator animator;
    public AudioManager audioManager;

    public float DelayTime = 1.2f;
    private void OnEnable()
    {
        animator.CrossFadeInFixedTime("Home Open", 0.0f);
        StartCoroutine(DelaySound());
    }

    IEnumerator DelaySound()
    {
        yield return new WaitForSeconds(DelayTime);

        if(audioManager != null)
        {
            audioManager.PlaySFX("Background Music");
        }
    }
}
