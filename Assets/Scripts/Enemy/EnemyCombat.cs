using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private WeaponRender _weaponRender;

    private int _distanceToAttack = 24;

    public bool IsCombat { get; private set; }

    public Player Player { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            IsCombat = true;
            Player = player;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            IsCombat = false;
            Player = null;
        }
    }

    public void TryAttackPlayer()
    {
        if (transform.position.IsEnoughClose(Player.transform.position, _distanceToAttack))
            _weapon.TryShoot();

        LookAtTarget();
    }

    private void LookAtTarget()
    {
        Vector3 lookDirection = Player.transform.position - transform.position;
        float angle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, -angle);
        _weaponRender.GetCurrentAngle(transform.rotation.z);
    }
}