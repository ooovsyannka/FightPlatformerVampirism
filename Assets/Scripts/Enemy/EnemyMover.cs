using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _idleTime;
    [SerializeField] private float _speedMove;
    [SerializeField] private float _distanceToPlayer;
    

    private float _minDistanceToTarget = 0.3f;
    private float _currentDistance;
    private int _currentPointIndex = 0;
    private bool _isCombat;
    private Vector2 _target;
    private Coroutine _moveToCoroutine;
    private Coroutine _changeTargetCoroutine;
    private List<Transform> _wayPoints;

    public bool IsMove { get; private set; }
    public State EnemyState { get; private set; }

    private void OnEnable()
    {
        if (_wayPoints != null)
            _target = _wayPoints[GetRandomIndexWayPoint()].position;

        _moveToCoroutine = StartCoroutine(MoveTo());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isCombat == false && collision.gameObject.TryGetComponent(out Enemy _))
        {
            int newPoint = GetRandomIndexWayPoint();

            while (newPoint == _currentPointIndex)
            {
                newPoint = GetRandomIndexWayPoint();
            }

            _currentPointIndex = newPoint;
            _target = _wayPoints[_currentPointIndex].position;
        }
    }

    public void StopMove()
    {
        EnemyState = State.Die;
        if (_moveToCoroutine != null)
            StopCoroutine(_moveToCoroutine);

        if (_changeTargetCoroutine != null)
            StopCoroutine(_changeTargetCoroutine);
    }

    public void SetTarget(Vector2 target)
    {
        if (_isCombat)
        {
            _target = target;
            ChangeDistanceToTarget(_distanceToPlayer);
        }
        else
        {
            _target = _wayPoints[_currentPointIndex].position;
            ChangeDistanceToTarget(_minDistanceToTarget);
        }
    }

    public void SetCombateState(bool isCombat) => _isCombat = isCombat;

    public void GetWayPoints(List<Transform> wayPoints)
    {
        _wayPoints = new List<Transform>();
        _wayPoints = wayPoints;
    }

    private void ChangeDistanceToTarget(float distance) => _currentDistance = distance;

    private int GetRandomIndexWayPoint() => Random.Range(0, _wayPoints.Count);

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target, _speedMove * Time.deltaTime);

        TurnByDirection(_target.x);
    }

    private void TurnByDirection(float targetPositionX)
    {
        float maxAngle = 180;

        transform.rotation = targetPositionX < transform.position.x ? Quaternion.Euler(0, maxAngle, 0) : Quaternion.identity;
    }

    private IEnumerator MoveTo()
    {
        IsMove = true;
        EnemyState = State.Move;

        while (transform.position.IsEnoughClose( _target,_currentDistance) == false)
        {
            Move();

            yield return null;
        }

        IsMove = false;

        if (_changeTargetCoroutine != null)
            StopCoroutine(_changeTargetCoroutine);

        _changeTargetCoroutine = StartCoroutine(ChangeTarget());
    }

    private IEnumerator ChangeTarget()
    {
        EnemyState = State.AnyState;

        yield return new WaitForSeconds(_idleTime);

        _currentPointIndex = GetRandomIndexWayPoint();
        _target = _wayPoints[_currentPointIndex].position;


        if (_moveToCoroutine != null)
            StopCoroutine(_moveToCoroutine);

        _moveToCoroutine = StartCoroutine(MoveTo());
    }
}
