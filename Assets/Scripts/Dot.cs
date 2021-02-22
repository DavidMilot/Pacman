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

    /// <summary>
    /// When player collects the dot, increment score, play munch sound and turn off game object
    /// </summary>
    public void Collect()
    {
        ScoreManager.Instance.UpdatePoints(_points);
        AudioManager.Instance.PlayMunchSoundSFX();
        gameObject.SetActive(false);
    }
}
