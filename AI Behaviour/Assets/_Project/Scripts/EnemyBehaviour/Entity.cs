using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private Animator _animator;
    private Collider _collider;

    public Animator Animator => _animator;
    public Collider Collider => _collider;
}
