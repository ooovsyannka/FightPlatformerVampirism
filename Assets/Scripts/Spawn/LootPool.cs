using System.Collections.Generic;
using UnityEngine;

public class LootPool : MonoBehaviour
{
    [System.Serializable]
    public class Pools
    {
        public int MaxSize;
        public Loot Loot;
    }

    [SerializeField] private List<Pools> _lootPrefabs;

    private Dictionary<TypeLoot, Queue<Loot>> _lootPools;

    private void Start()
    {
        FillLootPool();
    }

    private void FillLootPool()
    {
        _lootPools = new Dictionary<TypeLoot, Queue<Loot>>();

        foreach (Pools lootPool in _lootPrefabs)
        {
            Queue<Loot> newLootPool = new Queue<Loot>();
            Loot currentLoot = null;

            for (int i = 0; i < lootPool.MaxSize; i++)
            {
                currentLoot = Instantiate(lootPool.Loot);
                currentLoot.transform.SetParent(transform);
                currentLoot.gameObject.SetActive(false);
                newLootPool.Enqueue(currentLoot);
            }

            _lootPools.Add(currentLoot.GetTypeLoot(), newLootPool);
        }
    }


    public bool TryGetDesiredLoot(out Loot loot, TypeLoot desiredType)
    {
        loot = null;
         
        if (_lootPools.ContainsKey(desiredType))
        {
            if (_lootPools[desiredType].Count > 0)
            {
                loot = _lootPools[desiredType].Dequeue();

                if (loot.gameObject.activeInHierarchy)
                    loot.gameObject.SetActive(true);

                _lootPools[desiredType].Enqueue(loot);

                return true;
            }
        }

        return false;
    }
}