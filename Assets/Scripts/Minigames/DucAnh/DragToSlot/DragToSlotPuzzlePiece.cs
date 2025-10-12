using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DragToSlotPuzzlePiece : MonoBehaviour
{
    [SerializeField] private DragToSlotPuzzleSlot _slot;

    [SerializeField] private ParticleSystem _particleSystem;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _pickupClip;
    [SerializeField] private AudioClip _dropClip;

    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeAmount = 0.1f;
    [SerializeField] private float decreaseFactor = 1.0f;

    private bool _dragging;
    private bool _completed;
    private Vector2 _startPosition;
    private Vector2 _offset;

    public static Action onAddPoint;
    void Awake()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        if (_completed) return;
        if (!_dragging) return;

        transform.position = GetMousePos() - _offset;
    }

    private void OnMouseDown() {
        if (_completed) return;

        _dragging = true;
        _audioSource.PlayOneShot(_pickupClip);

        _offset = GetMousePos() - (Vector2)transform.position;
    }

    private void OnMouseUp() {
        if (_completed) return;

        if (Vector2.Distance(transform.position, _slot.transform.position) < 0.5f)
        {
            transform.position = _slot.transform.position;
            _particleSystem.Play();
            _slot.PlacedCorrectly();
            //FixSwordGameManager.Instance.AddPoint();
            onAddPoint?.Invoke();   
            _completed = true;
            this.enabled = false;
        } else {
            transform.position = _startPosition;
            Shake();
        }

        _dragging= false;
        _audioSource.PlayOneShot(_dropClip);
    }

    private Vector2 GetMousePos() {
        return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Shake() {
        StopAllCoroutines(); // Stop any existing shakes
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine() {
        float currentShakeAmount = shakeAmount;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration) {
            float x = Random.Range(-1f, 1f) * currentShakeAmount;
            float y = Random.Range(-1f, 1f) * currentShakeAmount;

            transform.position = _startPosition + new Vector2(x, y);

            currentShakeAmount = Mathf.Lerp(shakeAmount, 0, elapsed / shakeDuration); // Gradually decrease shake
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = _startPosition; // Reset to original position
    }
}
