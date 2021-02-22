using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUp : MonoBehaviour, ICollectable
{
    public event Action<PowerUp> OnPickup;
    [SerializeField] int _points = 10;
    [SerializeField] int _powerUpTime = 6;

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
    /// When player collects the dot, increment score, give player power up and turn off game object
    /// </summary>
    public void Collect()
    {
        ScoreManager.Instance.UpdatePoints(_points);
        Player.Instance.PowerUpPlayer(_powerUpTime);
        gameObject.SetActive(false);
    }
}
