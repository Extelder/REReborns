using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHitBoxOnCollisionAfterVelocity : MonoBehaviour
{
    [SerializeField] private VelocityDestructableObject _velocityDestructableObject;

    [field: SerializeField] public float Damage { get; private set; }
    

    private void OnEnable()
    {
        _velocityDestructableObject.Destructed += OnDestructed;
    }

    private void OnDestructed(Collision other)
    {
        if (other.collider.TryGetComponent<HitBox>(out HitBox hitBox))
        {
            hitBox.Hit(this);
        }
    }

    private void OnDisable()
    {
        _velocityDestructableObject.Destructed -= OnDestructed;
    }
}