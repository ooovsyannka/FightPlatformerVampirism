using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Medkit : Loot
{
    [field: SerializeField] public int CountToRecovery { get; private set; }

    public override TypeLoot GetTypeLoot()
    {
        return TypeLoot.Medkit;
    }
}