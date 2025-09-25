using UnityEngine;

public class UIHome : MonoBehaviour
{
    public Animator animator;
    private void OnEnable()
    {
        animator.CrossFadeInFixedTime("Home Open", 0.0f);
    }
}
