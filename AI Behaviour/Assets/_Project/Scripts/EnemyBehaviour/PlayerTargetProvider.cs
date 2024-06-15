using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerTargetProvider : TargetProvider
{
    public event Action<bool> PlayerInSight;

    [SerializeField]
    private string _targetTag = "Player";
    [SerializeField]
    private float _visionRange = 60;

    private SphereCollider _collider;
    private bool _targetInSight;

    public bool TargetInSight
    {
        get => _targetInSight;
        private set
        {
            if (_targetInSight != value)
            {
                _targetInSight = value;
                Debug.Log("Target in Sight: " + TargetInSight);
                PlayerInSight?.Invoke(_targetInSight);
            }
        }
    }

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        _collider.isTrigger = true;
    }

    private void Update()
    {
        if (HasTarget)
        {
            InVisionRangeCheck();
        }
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

    public override Transform GetTarget() => Target;

    private void InVisionRangeCheck()
    {
        //_collider.bounds.extents.z
        if (Physics.Raycast(this.transform.position, Target.position, _collider.radius))
        {
            float angle = Vector3.Angle(this.transform.forward, Target.position - this.transform.position);

            if (angle <= _visionRange)
                TargetInSight = true;
            else
                TargetInSight = false;
        }
        else
            TargetInSight = false;
    }
}
