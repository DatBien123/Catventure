using UnityEngine;
using UnityEngine.Events;

public class HandHeldItem : MonoBehaviour
{
    public UnityEvent onBeWeared;
    public UnityEvent onBeTookOff;

    private void OnEnable()
    {
        onBeWeared?.Invoke();
    }

    private void OnDisable()
    {
        onBeTookOff?.Invoke();
    }
}
