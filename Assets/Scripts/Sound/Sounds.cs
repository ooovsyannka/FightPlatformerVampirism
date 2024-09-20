using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Sounds : MonoBehaviour
{
    [System.Serializable]
    public class AudioClipHolder
    {
        public State State;
        public AudioClip Sound;
    }

    [SerializeField] private List<AudioClipHolder> _audioClips = new List<AudioClipHolder>();

    private AudioSource _audioSource;
    private Dictionary<State, AudioClip> _soundHolder;

    private void Start()
    {
        _soundHolder = new Dictionary<State, AudioClip>();

        _audioSource = GetComponent<AudioSource>();

        foreach (AudioClipHolder audioClip in _audioClips)
        {
            _soundHolder.Add(audioClip.State, audioClip.Sound);
        }
    }

    public void PlaySound(State state)
    {
        if (_soundHolder.ContainsKey(state))
        {
            _audioSource.clip = _soundHolder[state];

            if (_audioSource.isPlaying)
                return;

            _audioSource.Play();
        }
        else
        {
            _audioSource.Stop();
        }
    }

    public void SetValueVolume(float value = 0)
    {
        _audioSource.volume = value;
    }
}
