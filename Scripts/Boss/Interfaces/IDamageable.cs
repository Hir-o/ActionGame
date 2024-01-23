using System;
using UnityEngine;

public interface IDamageable
{
    public event Action OnDie;
    public event Action<int, int> OnHealthChanged;
    
    public void TakeDamage(int damageAmount);
    public void Die();

    public Vector3 RocketIncomingPosition { get; }
}
