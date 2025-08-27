using UnityEngine;
using UnityEngine.UI;
public class FloatingText : MonoBehaviour
{
    public Vector3 Offset = new Vector3(0, 2, 0);
    public Text text;
    private void Start()
    {
        transform.localPosition += Offset;
    }
    public void SetText(string context, Color color)
    {
        text.text = context;
        text.color = color;
    }
}
