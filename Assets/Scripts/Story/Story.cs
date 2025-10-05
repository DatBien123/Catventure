using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Story : MonoBehaviour
{
    public StoryInstance StoryData;

    public MeshRenderer MeshRenderer;

    public UnityEvent OnShowUp;
    public UnityEvent OffShowUp;

    Coroutine C_ShopUp;
    public void SetupStory(StoryInstance story)
    {
        StoryData = story;

        Material Material = MeshRenderer.material;

        // Gán texture mới vào Base Map
        Material.SetTexture("_BaseMap", story.StoryData.Data.Icon);
        Material.SetTexture("_MetallicGlossMap", story.StoryData.Data.Icon);
        Material.EnableKeyword("_METALLICGLOSSMAP"); // bật keyword để dùng map
    }

    public void StartShowUp(float waitTime)
    {
        if(C_ShopUp != null) StopCoroutine(C_ShopUp);
        C_ShopUp = StartCoroutine(ShowUp(waitTime));
    }

    IEnumerator ShowUp(float waitTime)
    {

        OnShowUp?.Invoke();
        float elapsedTime = 0.0f;

        while (elapsedTime < waitTime)
        {

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        OffShowUp?.Invoke();
    }
}
