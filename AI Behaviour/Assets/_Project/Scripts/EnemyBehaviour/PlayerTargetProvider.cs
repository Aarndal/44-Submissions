using System;
using UnityEngine;

public class PlayerTargetProvider : TargetProvider
{
    public event Action<bool> PlayerInSight;

    [SerializeField]
    private string _targetTag = "Player";

    private Collider _collider;


    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            Target = other.transform;
            PlayerInSight?.Invoke(HasTarget);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            Target = null;
            PlayerInSight?.Invoke(HasTarget);
        }
    }

    public override Transform GetTarget()
    {
        return Target;
    }
}
