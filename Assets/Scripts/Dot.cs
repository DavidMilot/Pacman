using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Dot : MonoBehaviour, ICollectable
{
    public event Action<Dot> OnPickup;
    [SerializeField] int _points = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            OnPickup?.Invoke(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Collect()
    {
        ScoreManager.Instance.UpdatePoints(_points);
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            AudioManager.Instance.PlaySound(AudioManager.AudioSoundEffects.Munch1, false);
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.AudioSoundEffects.Munch2, false);
        }
        gameObject.SetActive(false);
    }
}
