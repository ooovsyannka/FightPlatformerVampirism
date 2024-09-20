using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public abstract class Bullet : Loot
{
    [SerializeField] private int _force;
    [SerializeField] private int _critDamage;

    private float _timer = 1;
    private Rigidbody2D _rigidbody;
    private Transform _direction;
    private Transform _parent;

    [field: SerializeField] public int Damage { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _rigidbody.velocity = Vector2.zero;

        if (_direction != null)
        {
            _rigidbody.AddForce(_direction.up * _force, ForceMode2D.Impulse);
            StartCoroutine(TimeToDie());
        }
    }

    public int ChanceCrit()
    {
        float chance = Random.Range(0, 1f);
        float desiredChance = 0.2f;

        if (chance <= desiredChance)
        {
            return _critDamage;
        }

        return Damage;
    }

    public void SetDirection(Transform direction) => _direction = direction;

    public void GetParent(Transform parent)
    {
        _parent = parent;
        transform.parent = _parent;
    }

    private IEnumerator TimeToDie()
    {
        yield return new WaitForSeconds(_timer);

        transform.parent = _parent;
        gameObject.SetActive(false);
    }
}
