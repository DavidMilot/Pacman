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
    bool _playMunchSound1 = true;

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
        Siren5,
        Intermission
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
                break;
            }
        }
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            _audioSources[i].Stop();
            _audioSources[i].loop = false;
        }
    }

    public void StopSound(AudioSoundEffects effects)
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            if (_audioSources[i].isPlaying == true && _audioSources[i].clip == _clips[(int)effects])
            {
                _audioSources[i].Stop();
                break;
            }
        }
    }

    /// <summary>
    /// Play munch sfx and have it switch between the two munch sounds automatically
    /// </summary>
    public void PlayMunchSoundSFX()
    {
        if (_playMunchSound1)
        {
            PlaySound(AudioSoundEffects.Munch1, false);
        }
        else
        {
            PlaySound(AudioSoundEffects.Munch2, false);
        }
        _playMunchSound1 = !_playMunchSound1;
    }

    /// <summary>
    /// Retrive the sound clip length in seconds
    /// </summary>
    /// <param name="effect">SFX to choose</param>
    /// <returns>sound clip length in seconds</returns>
    public float GetSoundClipLength(AudioSoundEffects effect)
    {
        return _clips[(int)effect].length;
    }

    /// <summary>
    /// Stop all siren sounds
    /// </summary>
    public void StopAllSirens()
    {
        for (int i = 0; i < _audioSources.Count; i++)
        {
            if (_audioSources[i].isPlaying == true && _audioSources[i].clip == _clips[(int)AudioSoundEffects.Siren1] 
                || _audioSources[i].isPlaying == true && _audioSources[i].clip == _clips[(int)AudioSoundEffects.Siren2]
                || _audioSources[i].isPlaying == true && _audioSources[i].clip == _clips[(int)AudioSoundEffects.Siren3]
                || _audioSources[i].isPlaying == true && _audioSources[i].clip == _clips[(int)AudioSoundEffects.Siren4]
                || _audioSources[i].isPlaying == true && _audioSources[i].clip == _clips[(int)AudioSoundEffects.Siren5])
            {
                _audioSources[i].Stop();
                break;
            }
        }
    }

    /// <summary>
    /// Stop current siren playing and play siren effect
    /// </summary>
    /// <param name="sirenEffect">Play desired siren effect</param>
    public void PlaySiren(AudioSoundEffects sirenEffect)
    {
        //Make sure sirenEffect is a sirenEffect 
        if (sirenEffect != AudioSoundEffects.Siren1
            && sirenEffect != AudioSoundEffects.Siren2
            && sirenEffect != AudioSoundEffects.Siren3
            && sirenEffect != AudioSoundEffects.Siren4
            && sirenEffect != AudioSoundEffects.Siren5)
        {
            Debug.LogAssertion("Effect not a siren SFX.");
        }
        else
        {
            //Stop what ever siren is playing
            StopAllSirens();

            //Make sure there are no other sirens playing before checking 
            for (int i = 0; i < _audioSources.Count; i++)
            {
                if (_audioSources[i].isPlaying == false)
                {
                    _audioSources[i].loop = true;
                    _audioSources[i].clip = _clips[(int)sirenEffect];
                    _audioSources[i].Play();
                    break;
                }
            }
        }
    }
}
