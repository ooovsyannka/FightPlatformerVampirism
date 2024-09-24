using System;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [field:SerializeField] public int MaxValue { get; private set; }
    
    public float CurrentValue { get; private set; }

    public event Action Died;
    public event Action<float> ValueChanged;

    public void TakeDamage(float damage)
    {
        if (CurrentValue == 0)
            return;

        if (damage < 0)
            damage *= -1;

        CurrentValue = Math.Clamp(CurrentValue - damage, 0, MaxValue);

        ValueChanged?.Invoke(CurrentValue);

        if (CurrentValue == 0)
        {
            Died?.Invoke();
        }
    }

    public void Regenerate(float desiredCount)
    {
        if ((CurrentValue + desiredCount) < MaxValue)
        {
            CurrentValue += desiredCount;
        }
        else
        {
            CurrentValue = MaxValue;
        }

        ValueChanged?.Invoke(CurrentValue);
    }
}
