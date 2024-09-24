using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class VampirismDetector : MonoBehaviour
{
   [SerializeField] private VampirismTarget _vampirismTarget; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealth enemyHealth))
        {
            _vampirismTarget.AddEnemyHealthInList(enemyHealth);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealth enemyHealth))
        {
            _vampirismTarget.RemoveEnemyHealthInList(enemyHealth);
        }
    }
}
