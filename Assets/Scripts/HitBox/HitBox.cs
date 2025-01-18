using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private Health _health;

    public virtual void Hit(Rapier rapier)
    {
        _health.TakeDamage(rapier.Damage);
    }

    public virtual void Hit(DamageHitBoxOnCollisionAfterVelocity damageHitBoxOnCollisionAfterVelocity)
    {
        _health.TakeDamage(damageHitBoxOnCollisionAfterVelocity.Damage);
    }

    public EnemyRagdollHealth TryGetRagdollHealth() => (EnemyRagdollHealth) _health;
}