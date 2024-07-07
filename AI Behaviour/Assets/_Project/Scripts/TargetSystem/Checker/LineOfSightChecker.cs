using System;
using System.Collections;
using UnityEngine;

public class LineOfSightChecker : MonoBehaviour
{
    public event Action<Transform> OnGainSight;
    public event Action<Transform> OnLostSight;

    [Header("References")]
    [SerializeField]
    private TargetProvider _targetProvider;

    [Header("Variables")]
    [SerializeField]
    private LayerMask _targetedLayerMask;
    [SerializeField]
    private float _fieldOfView = 120f;
    [SerializeField]
    private float _visionRange = 10f;

    private bool _targetInSight = false;

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

    private void Awake()
    {
        if (_targetProvider == null)
            throw new ArgumentNullException("Target Provider is not set.");
    }

    private void Update()
    {
        if (!_targetProvider.HasTarget)
            TargetInSight = false;
        else
        {
            Ray ray = new()
            {
                origin = transform.position,
                direction = (_targetProvider.Target.position - this.transform.position).normalized,
            };

            TargetInSight = CheckLineOfSight(ray);

            Debug.DrawRay(ray.origin, ray.direction * _visionRange, TargetInSight ? Color.green : Color.red);
        }

        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        yield return new WaitForSeconds(1);

        if (TargetInSight)
            this.transform.rotation = Quaternion.LookRotation((_targetProvider.Target.position - this.transform.position).normalized, this.transform.up);

        if (!TargetInSight)
            this.transform.rotation = Quaternion.LookRotation(this.transform.parent.transform.forward, this.transform.up);
    }

    private bool CheckLineOfSight(Ray ray)
    {
        float dotProduct = Vector3.Dot(this.transform.forward, ray.direction);

        if (dotProduct >= Mathf.Cos(Mathf.Deg2Rad * _fieldOfView / 2))
        {
            int toHitLayerMask = 1 << this.gameObject.layer | 1 << 2; // 2 is build-in "Ignore Raycast" layer
            toHitLayerMask = ~toHitLayerMask; // gets reversed layerMasks

            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, _visionRange, toHitLayerMask, QueryTriggerInteraction.Ignore))
            {
                LayerMask layerMask = 1 << hit.collider.gameObject.layer;

                if ((_targetedLayerMask & layerMask) != 0)
                    return true;
            }
        }

        return false;
    }
}
