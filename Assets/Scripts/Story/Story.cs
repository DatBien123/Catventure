using UnityEngine;

public class Story : MonoBehaviour
{
    public SO_Story StoryData;

    public MeshRenderer MeshRenderer;


    public void SetupStory(SO_Story story)
    {
        StoryData = story;

        Material Material = MeshRenderer.material;

        // Gán texture mới vào Base Map
        Material.SetTexture("_BaseMap", story.Data.Icon);
        Material.SetTexture("_MetallicGlossMap", story.Data.Icon);
        Material.EnableKeyword("_METALLICGLOSSMAP"); // bật keyword để dùng map
    }

}
