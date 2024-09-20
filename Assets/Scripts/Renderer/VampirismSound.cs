using UnityEngine;

[RequireComponent (typeof(AudioSource))]

public class VampirismSound : MonoBehaviour 
{
    [SerializeField] private Vampirism _vampirism;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource> ();
    }

    private void OnEnable()
    {
        _vampirism.SkillUsed += PlaySound;
    }

    private void OnDisable()
    {
        _vampirism.SkillUsed -= PlaySound;
    }

    private void PlaySound()
    {
        _audioSource.Play();
    }
}
