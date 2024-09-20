using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private EnemyHealth _health;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerBullet playerBullet))
        {
            _health.TakeDamage(playerBullet.Damage);
            playerBullet.gameObject.SetActive(false);
        }
    }
}