using System.Threading;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public abstract class Loot : MonoBehaviour
{
    public abstract TypeLoot GetTypeLoot();
}