using UnityEngine;

[DisallowMultipleComponent]
public abstract class Entity : MonoBehaviour
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

    protected virtual void OnEnable()
    {
        if (_animationEventBroadcaster != null)
            _animationEventBroadcaster.AnimationEventTriggered += OnAnimationEvenTriggered;
    }

    protected virtual void Start()
    {
        Animator.enabled = true;
    }

    protected virtual void OnDisable()
    {
        if (_animationEventBroadcaster != null)
            _animationEventBroadcaster.AnimationEventTriggered -= OnAnimationEvenTriggered;
    }

    protected virtual void OnAnimationEvenTriggered(AnimationEvent args) { }
}
