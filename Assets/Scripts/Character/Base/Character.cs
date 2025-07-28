using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
