using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class VampirismDetector : MonoBehaviour
{
    private List<EnemyHealth> _enemyHealths = new List<EnemyHealth>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealth enemyHealth))
        {
            _enemyHealths.Add(enemyHealth);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealth enemyHealth))
        {
            _enemyHealths.Remove(enemyHealth);
        }
    }

    public EnemyHealth GetNearestEnemy()
    {
        EnemyHealth enemyHealth = null;

        if (_enemyHealths.Count > 0)
        {
            enemyHealth = _enemyHealths.OrderBy(enemyHealth =>
            transform.position.SqrDistance(enemyHealth.transform.position)).FirstOrDefault();
        }

        return enemyHealth;
    }
}
