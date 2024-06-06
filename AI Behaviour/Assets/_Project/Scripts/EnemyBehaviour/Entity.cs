using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Animator _animator;

    public Animator Animator => _animator;

    private void Awake()
    {
        _animator = Animator != null ? Animator : GetComponentInChildren<Animator>();
    }
}
