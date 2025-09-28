using System.Collections;
using UnityEngine;

public class UIIntro : MonoBehaviour
{
    public GameObject LoadingUI;

    Coroutine C_StartIntro;

    public float delayTime = 3.0f;

    void Start()
    {
        StartIntro();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartIntro()
    {
        if(C_StartIntro != null)StopCoroutine(C_StartIntro);
        C_StartIntro = StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(delayTime);
        this.gameObject.SetActive(false);
        LoadingUI.gameObject.SetActive(true);
    }
}
