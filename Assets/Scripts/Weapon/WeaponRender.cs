using UnityEngine;

public class WeaponRender : MonoBehaviour
{
    private SpriteRenderer _weaponSprite;
    private float _currentAngle;

    private void Start()
    {
        _weaponSprite = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (_currentAngle > 0)
        {
            _weaponSprite.flipX = true;
        }
        else
        {
            _weaponSprite.flipX = false;
        }
    }

    public void GetCurrentAngle(float currentAngle)
    {
        _currentAngle = currentAngle;
    }
}