using UnityEngine;

public class EffectSprite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ShowEffect(float lifeTime)
    {
        // Sau lifetime giây thì tự hủy
        Destroy(gameObject, lifeTime);

    }
    public void ShowShortEffect()
    {

    }
    public void OnEffectEnd()
    {
        Destroy(gameObject);
    }
}
