using UnityEngine;
using UnityEngine.UI;
public class NotificationResult : MonoBehaviour
{
    public Image resultSprite;
    public Text resultName;
    public Text resultDescription;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowNotificationResult(Sprite sprite, string name, string description)
    {
        resultSprite.sprite = sprite;
        resultName.text = name;
        resultDescription.text = description;
    }
}
