using UnityEngine;

public class Looking : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _speed;
    [SerializeField] private float _positionZ;

    private void FixedUpdate()
    {
        Vector3 target = new Vector3(_player.transform.position.x, _player.transform.position.y, _positionZ);
        transform.position = Vector3.Lerp(transform.position, target, _speed * Time.fixedDeltaTime);
    }
}
