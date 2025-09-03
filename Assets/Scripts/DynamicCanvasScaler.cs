using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class DynamicCanvasScaler : MonoBehaviour
{
    public Vector2 referenceResolution = new Vector2(1600, 900);
    public CanvasScaler scaler;
    float currentAspect;
    float referenceAspect;
    void Awake()
    {
        scaler = GetComponent<CanvasScaler>();
        UpdateMatch();
    }

    void OnRectTransformDimensionsChange()
    {
        UpdateMatch();
    }

    void UpdateMatch()
    {
        currentAspect = (float)Screen.width / Screen.height;
        referenceAspect = referenceResolution.x / referenceResolution.y;

        scaler.matchWidthOrHeight = (currentAspect > referenceAspect) ? 1 : 0;
    }
}
