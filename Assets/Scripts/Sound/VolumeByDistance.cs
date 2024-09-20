using UnityEngine;

[RequireComponent(typeof(Sounds))]

public class VolumeByDistance : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 30;

    private Sounds _sounds;

    private void Start()
    {
        _sounds = GetComponent<Sounds>();
    }

    public void SetVolumeValue(Player player)
    {
        if (player != null)
        {
            float maxValue = 1;
            float distance = Vector2.Distance(transform.position, player.transform.position);
            float volume = Mathf.Clamp(maxValue - distance / _maxDistance, 0, maxValue);

            _sounds.SetValueVolume(volume);
        }
        else
        {
            _sounds.SetValueVolume(0);
        }
    }
}