using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _reloadDelayTime;
    [SerializeField] private float _shootDelayTime;
    [SerializeField] private Ammunition _ammunition;
    [SerializeField] private WeaponAnimation _weaponAnimation;
    [SerializeField] private Sounds _weaponSounds;

    private bool _isReload;
    private bool _isShoot;
    private bool _canReload;
    private bool _canShoot;
    private Coroutine _shootDelayCoroutine;
    private Coroutine _reloadDelay;
    private Transform _shootPosition;
    private State _weaponState;

    private void Start()
    {
        _isReload = false;
        _isShoot = false;
        _canReload = true;
        _canShoot = true;
        _shootPosition = transform;
        _weaponState = State.AnyState;
    }

    private void Update()
    {
        _weaponAnimation.PlayAnimation(_isShoot, _isReload);
        _weaponSounds.PlaySound(_weaponState);
    }

    public void TryShoot()
    {
        if (_canShoot == false)
            return;

        if (_ammunition.TryGetBullet(out Bullet currentBullet))
        {
            currentBullet.SetDirection(_shootPosition);
            currentBullet.transform.position = _shootPosition.position;
            currentBullet.gameObject.SetActive(true);

            if (_shootDelayCoroutine != null)
                StopCoroutine(_shootDelayCoroutine);

            _shootDelayCoroutine = StartCoroutine(ShootDelay());
        }
        else
        {
            TryReload();
        }
    }

    public void TryReload()
    {
        if (_canReload)
        {
            if (_reloadDelay != null)
                StopCoroutine(_reloadDelay);

            _reloadDelay = StartCoroutine(ReloadDelay());
        }
    }

    private IEnumerator ShootDelay()
    {
        _canShoot = false;
        _isShoot = true;
        _weaponState = State.Shoot;

        yield return new WaitForSeconds(_shootDelayTime);

        _isShoot = false;
        _canShoot = true;
        _weaponState = State.AnyState;
    }

    private IEnumerator ReloadDelay()
    {
        _canReload = false;
        _isReload = true;
        _weaponState = State.Reload;

        yield return new WaitForSeconds(_reloadDelayTime);

        _canReload = true;
        _isReload = false;
        _weaponState = State.AnyState;
        _ammunition.TryReload();
    }
}
