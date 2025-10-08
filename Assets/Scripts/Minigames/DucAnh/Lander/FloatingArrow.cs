using UnityEngine;

public class FloatingArrow : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 1f; // Speed of the floating motion
    [SerializeField] private float floatHeight = 0.5f; // Height of the floating motion
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}
