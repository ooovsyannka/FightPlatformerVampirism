using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashDelay;

    private bool _canDash = true;
    private Rigidbody2D _rigidbody;
    private Coroutine _dash;
    private Coroutine _dashCooldown;

    public bool IsDash { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(float horizontalDirection, float verticalDirection)
    {
        transform.Translate(new Vector2(_moveSpeed * horizontalDirection,
            _moveSpeed * verticalDirection) * Time.fixedDeltaTime,
            Space.World);

        TurnByDirection(horizontalDirection);
    }

    public void Dash(float directionX, float directionY)
    {
        if (_canDash)
        {
            if (_dash != null)
                StopCoroutine(_dash);

            _dash = StartCoroutine(DashDuration(directionX, directionY));
        }
    }

    private void TurnByDirection(float direction)
    {
        float maxAngle = 180;

        if (direction < 0)
            transform.rotation = Quaternion.Euler(0, maxAngle, 0);

        if (direction > 0)
            transform.rotation = Quaternion.identity;
    }

    private IEnumerator DashDuration(float directionX, float directionY)
    {
        _rigidbody.AddForce(new Vector2(directionX, directionY) * _dashForce, ForceMode2D.Impulse);
        _canDash = false;
        IsDash = true;

        yield return new WaitForSeconds(_dashDuration);

        IsDash = false;
        _rigidbody.velocity = Vector2.zero;

        if (_dashCooldown != null)
            StopCoroutine(_dashCooldown);

        _dashCooldown = StartCoroutine(DashCooldown());
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(_dashDelay);

        _canDash = true;
    }
}
