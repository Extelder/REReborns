using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrupPickupable : MonoBehaviour, IPickupable
{
    [SerializeField] private EnemyRagdollHealth _ragdollHealth;
    [SerializeField] private Collider _pickupCollider;

    [field: SerializeField] public bool CanPickup { get; set; }

    private void OnEnable()
    {
        _ragdollHealth.Damaged += OnDamaged;
    }

    private void OnDamaged(float value)
    {
        CanPickup = true;
        _pickupCollider.enabled = true;
    }

    private void OnDisable()
    {
        _ragdollHealth.Damaged -= OnDamaged;
    }

    public void Pickuped()
    {
    }
}