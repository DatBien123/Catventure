using UnityEngine;

public class DragToSlotPuzzlePiece : MonoBehaviour
{
    [SerializeField] private DragToSlotPuzzleSlot _slot;

    [SerializeField] private ParticleSystem _particleSystem;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _pickupClip;
    [SerializeField] private AudioClip _dropClip;

    private bool _dragging;
    private bool _completed;
    private Vector2 _startPosition;
    private Vector2 _offset;

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
            FixSwordGameManager.Instance.AddPoint();

            _completed = true;
            this.enabled = false;
        } else {
            transform.position = _startPosition;
        }

        _dragging= false;
        _audioSource.PlayOneShot(_dropClip);
    }

    private Vector2 GetMousePos() {
        return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
