using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip[] _clips;
    public AudioSource audioSource;

    List<AudioSource> _audioSources = new List<AudioSource>();

    //Sound list of enums

    public enum AudioSoundEffects
    {
        Munch1,
        Munch2,
        EatGhost,
        EatFruit,
        PowerPellet,
        Death1,
        Death2,
        GameStart,
        Retreating,
        Siren1,
        Siren2,
        Siren3,
        Siren4,
        Siren5
    }

    void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(Instance);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            _audioSources.Add(Instantiate(audioSource));
            _audioSources[i].transform.SetParent(this.transform);
        }
    }

    public void PlaySound(AudioSoundEffects effect, bool loop)
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            if (_audioSources[i].isPlaying == false)
            {
                _audioSources[i].loop = loop;
                _audioSources[i].clip = _clips[(int)effect];
                _audioSources[i].Play();
            }
        }
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            _audioSources[i].Stop();
        }
    }

    public void StopSound(AudioSoundEffects effects)
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            _audioSources[i].Stop();
        }
    }

    public AudioClip GetAudioClip(AudioSoundEffects effect)
    {
        return null;
    }
}
