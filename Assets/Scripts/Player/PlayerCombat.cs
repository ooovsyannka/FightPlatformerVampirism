using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private WeaponRender _currentAngle;

    public void Attack()
    {
        _weapon.TryShoot();
    }

    public void LookAtTarget(Vector3 direction)
    {
        Vector3 lookDirection = direction - transform.position;
        float angle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, -angle);

        _currentAngle.GetCurrentAngle(transform.rotation.z);
    }

    public void Reload()
    {
        _weapon.TryReload();
    }
}