using UnityEngine;

[DisallowMultipleComponent]
public class Entity : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    protected AnimationEventBroadcaster _animationEventBroadcaster;

    protected Animator _animator;

    public Animator Animator => _animator;

    protected virtual void Awake()
    {
        _animator = Animator != null ? Animator : GetComponentInChildren<Animator>();
    }
}
