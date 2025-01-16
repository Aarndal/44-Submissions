using UnityEngine;

public class FakePlatform : MonoBehaviour, IAmToggleable
{
    [SerializeField]
    private Collider _triggerCollider;

    [field: SerializeField]
    public bool IsActive { get; set; }
    [field: SerializeField]
    public float DeactivationTime { get; protected set; }


    private void Start()
    {
        _triggerCollider.isTrigger = true;
    }

    public void SetToggleState(bool toggle)
    {
        _triggerCollider.enabled = toggle;
        this.IsActive = toggle;
    }
}
