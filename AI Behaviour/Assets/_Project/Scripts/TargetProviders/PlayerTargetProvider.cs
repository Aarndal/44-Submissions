using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SphereCollider))]
public class PlayerTargetProvider : TargetProvider
{
    [SerializeField]
    private string _targetTag = "Player";
    [SerializeField]
    private float _checkRadius = 10f;

    private SphereCollider _collider;

    public float CheckRadius => _checkRadius;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        _collider.isTrigger = true;
        _collider.radius = _checkRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
            Target = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_targetTag))
            Target = null;
    }
}
