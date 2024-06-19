using System;
using UnityEngine;

public class LineOfSightChecker : MonoBehaviour
{
    public event Action<Transform> OnGainSight;
    public event Action<Transform> OnLostSight;

    [SerializeField]
    private TargetProvider _targetProvider;
    [SerializeField]
    private LayerMask _targetedLayerMask;
    [SerializeField]
    private float _fieldOfView = 120f;
    [SerializeField]
    private float _visionRange = 10f;

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

                if (_targetInSight)
                    OnGainSight?.Invoke(_targetProvider.Target);
                else
                    OnLostSight?.Invoke(_targetProvider.Target);
            }
        }
    }

    private void Update()
    {
        if (!_targetProvider.HasTarget)
            TargetInSight = false;
        else
        {
            Ray ray = new()
            {
                origin = this.transform.position,
                direction = (_targetProvider.Target.position - this.transform.position).normalized
            };

            TargetInSight = CheckLineOfSight(_targetProvider.Target, ray);

            Debug.DrawRay(ray.origin, ray.direction * _visionRange, TargetInSight ? Color.green : Color.red);
        }
    }

    private bool CheckLineOfSight(Transform target, Ray ray)
    {
        float dotProduct = Vector3.Dot(this.transform.forward, ray.direction);

        if (dotProduct >= Mathf.Cos(Mathf.Deg2Rad * _fieldOfView / 2))
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, _visionRange))
            {
                LayerMask layerMask = 1 << hit.collider.gameObject.layer;
                if ((_targetedLayerMask & layerMask) != 0)
                    return true;
            }

        return false;
    }
}
