using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private EnemyPool _objectPooler;
    [SerializeField] private LootPool _lootPooler;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private List<Transform> _spawnerPositions;

    private void Start()
    {
        StartCoroutine(SpawnNewEnemy());
    }

    private IEnumerator SpawnNewEnemy()
    {
        bool isWork = true;

        while (isWork)
        {
            yield return new WaitForSeconds(_spawnDelay);

            SpawnEnemy();
        }
    }

    private Transform GetRandomSpaner()
    {
        return _spawnerPositions[UnityEngine.Random.Range(0, _spawnerPositions.Count)];
    }

    private void SpawnEnemy()
    {
        if (_objectPooler.TryGetEnemy(out Enemy enemy))
        {
            enemy.transform.position = GetRandomSpaner().position;

            if (CanBetrayLoot())
            {
                enemy.GetLoot(BetrayLoot());
            }

            enemy.gameObject.SetActive(true);
        }
    }

    private bool CanBetrayLoot()
    {
        float maxChance = 1;
        float randomNumber = UnityEngine.Random.Range(0, maxChance);
        float maxChanceToBetray = 0.35f;

        if (randomNumber <= maxChanceToBetray)
            return true;

        return false;
    }

    private Loot BetrayLoot()
    {
        Loot desiredLoot;
        int randomTypeLoot = UnityEngine.Random.Range(0, Enum.GetValues(typeof(TypeLoot)).Length);
        _lootPooler.TryGetDesiredLoot(out desiredLoot, (TypeLoot)randomTypeLoot);

        return desiredLoot;
    }
}