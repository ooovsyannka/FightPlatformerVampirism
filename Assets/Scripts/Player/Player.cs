using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMover), typeof(PlayerHealth), typeof(PlayerAmmunition))]

public class Player : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private PlayerCombat _combat;
    [SerializeField] private PlayerAnimation _animation;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Sounds _sound;
    [SerializeField] private Vampirism _vampirism;
    [SerializeField] private float _respawnDelay;

    private int _startHealth = 1; 
    private bool _isDie = false;
    private bool _isMove = false;

    private State _state;
    private Coroutine _respawnDelayCoroutine;
    private PlayerMover _mover;
    private PlayerHealth _health;
    private PlayerAmmunition _ammunition;
    private CapsuleCollider2D _collider;

    private void Awake()
    {
        _health = GetComponent<PlayerHealth>();
        _ammunition = GetComponent<PlayerAmmunition>();
        _mover = GetComponent<PlayerMover>();
        _collider = GetComponent<CapsuleCollider2D>();
        transform.position = _startPosition.position;
    }

    private void OnEnable()
    {
        _state = State.AnyState;
        _health.Died += Die;
        _health.Regenerate(_startHealth);
    }

    private void Update()
    {
        if (_isDie == false)
        {
            if (_inputReader.IsShoot)
            {
                _combat.Attack();
            }

            if (_inputReader.IsReload)
            {
                _combat.Reload();
            }

            if (_inputReader.UseSkill)
            {
                _vampirism.UseSkill();
            }

            _combat.LookAtTarget(_inputReader.MousePosition);
            _isMove = _inputReader.HorizontalInput != 0 || _inputReader.VerticalInput != 0;
        }

        _sound.PlaySound(_state);
        _animation.PlayAnimation(_isMove, _isDie, _mover.IsDash);
    }

    private void FixedUpdate()
    {
        if (_isDie == false)
        {
            _mover.Move(_inputReader.HorizontalInput, _inputReader.VerticalInput);

            if (_isMove)
            {
                _state = State.Move;

                if (_inputReader.IsDash)
                {
                    _mover.Dash(_inputReader.HorizontalInput, _inputReader.VerticalInput);
                }

                if (_mover.IsDash)
                {
                    _state = State.Dash;
                }
            }
            else
            {
                _state = State.AnyState;
            }
        }
    }

    private void Die()
    {
        _isDie = true;
        _state = State.Die;
        _collider.enabled = false;

        _vampirism.RecoverySkill();

        if (_respawnDelayCoroutine != null)
            StopCoroutine(_respawnDelayCoroutine);

        _respawnDelayCoroutine = StartCoroutine(RespawnDelay());
    }

    private IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(_respawnDelay);

        _health.Regenerate(_health.MaxValue);
        transform.position = _startPosition.position;
        _ammunition.ReplenishmentBulletsCount();
        _collider.enabled = true;

        _isDie = false;
    }
}