using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private Ammunition _amunition;
    [SerializeField] private Vampirism _vampirism;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Loot loot))
        {
            if (loot is EnemyBullet enemyBullet)
            {
                _health.TakeDamage(enemyBullet.Damage);
                enemyBullet.gameObject.SetActive(false);
            }
            else if (loot is Medkit medkit)
            {
                _health.Regenerate(medkit.CountToRecovery);
                medkit.gameObject.SetActive(false);
            }
            else if (loot is AmmunitionLoot ammunitionLoot)
            {
                _amunition.ReplenishmentBulletsCount(ammunitionLoot.CountToRecovery);
                ammunitionLoot.gameObject.SetActive(false);
            }
        }

        if (collision.TryGetComponent(out EnemyHealth enemyHealth))
        {
            _vampirism.AddEnemyInList(enemyHealth);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealth enemyHealth))
        {
            _vampirism.RemoveEnemyFromList(enemyHealth);
        }
    }
}
