using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMover), typeof(EnemyHealth))]

public class Enemy : MonoBehaviour
{
    [SerializeField] private VolumeByDistance _volumeByDistance;
    [SerializeField] private EnemyCombat _combat;
    [SerializeField] private EnemyAnimation _animation;
    [SerializeField] private Sounds _sounds;

    private float _timeDieDelay = 1.5f;
    private float _recoveryDelay = 0.1f;
    private bool _isDie = false;
    private EnemyMover _enemyMover;
    private EnemyHealth _enemyHealth;
    private Loot _loot;
    private CapsuleCollider2D _collider;
    private EnemyPool _pool;

    private void Awake()
    {
        _enemyMover = GetComponent<EnemyMover>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _collider = transform.GetComponent<CapsuleCollider2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(RecovyreDelay());
        _isDie = false;
        _enemyHealth.Died += Die;
        _collider.enabled = true;
    }

    private void OnDisable()
    {
        _enemyHealth.Died -= Die;
    }

    private void Update()
    {
        if (_isDie == false)
        {
            bool isCombat = _combat.IsCombat;

            if (isCombat)
            {
                _enemyMover.SetTarget(_combat.Player.transform.position);
                _combat.TryAttackPlayer();
            }

            _enemyMover.SetCombateState(isCombat);
        }

        _volumeByDistance.SetVolumeValue(_combat.Player);
        _sounds.PlaySound(_enemyMover.EnemyState);
        _animation.PlayAnimation(_enemyMover.IsMove, _isDie);
    }

    public void GetEnemyPool(EnemyPool enemyPool)
    {
        _pool = enemyPool;
    }

    public void GetLoot(Loot loot) => _loot = loot;

    private void Die()
    {
        if (_isDie == false)
        {
            _isDie = true;

            StartCoroutine(DieDelay());
        }
    }
    private void DropLoot()
    {
        if (_loot == null)
            return;

        _loot.gameObject.SetActive(true);
        _loot.transform.position = transform.position;
        _loot = null;
    }

    private IEnumerator RecovyreDelay()
    {
        yield return new WaitForSeconds(_recoveryDelay);

        _enemyHealth.Regenerate(_enemyHealth.MaxValue);
    }

    private IEnumerator DieDelay()
    {
        _enemyMover.StopMove();
      
        transform.GetComponent<CapsuleCollider2D>().enabled = false;

        yield return new WaitForSeconds(_timeDieDelay);

        //_pool.ReturnEnemyInPool(this);
        DropLoot();

        gameObject.SetActive(false);
    }
}
