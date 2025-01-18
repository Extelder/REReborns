using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityDestructableObject : MonoBehaviour
{
    [SerializeField] private float _velocityForDestruct;
    [SerializeField] private Pool _destroyEffectPool;

    private Rigidbody _rigidbody;

    public event Action<Collision> Collided;
    public event Action<Collision> Destructed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        float velocity = _rigidbody.velocity.magnitude;
        Debug.Log(other.gameObject);
        Debug.Log(other.gameObject.name);

        Collided?.Invoke(other);

        if (velocity >= _velocityForDestruct)
        {
            _destroyEffectPool.GetFreeElement(transform.position);
            Destructed?.Invoke(other);
            Destroy(gameObject);
        }
    }
}