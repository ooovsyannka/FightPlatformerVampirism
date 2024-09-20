using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private int _maxEnemyCount;
    [SerializeField] private List<Transform> _wayPoints;
    [SerializeField] private Enemy _enemyPrefab;

    private Queue<Enemy> _pool;

    private void Start()
    {
        FillEnemyPool();
    }

    public bool TryGetEnemy(out Enemy enemy)
    {
        enemy = null;

        if (_pool.Count != 0)
        {
            enemy = _pool.Dequeue();
            enemy.gameObject.SetActive(true);

            return true;
        }

        return false;
    }

    public void ReturnEnemyInPool(Enemy enemy)
    {
        _pool.Enqueue(enemy);
    }

    private void FillEnemyPool()
    {
        _pool = new Queue<Enemy>();

        Enemy currentEnemy;

        for (int i = 0; i < _maxEnemyCount; i++)
        {
            currentEnemy = Instantiate(_enemyPrefab);
            currentEnemy.transform.SetParent(transform);
            currentEnemy.GetEnemyPool(this);
            currentEnemy.gameObject.TryGetComponent(out EnemyMover enemyMover);
            enemyMover.GetWayPoints(_wayPoints);
            currentEnemy.gameObject.SetActive(false);
            _pool.Enqueue(currentEnemy);
        }
    }
}