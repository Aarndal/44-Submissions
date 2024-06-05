using System;
using UnityEngine;

public abstract class Entity : NavMeshMovement
{
    private Animator _animator;
    private Collider _collider;

    public Animator Animator => _animator;
    public Collider Collider => _collider;


    public static event Action<bool> Irgendwas;
}
