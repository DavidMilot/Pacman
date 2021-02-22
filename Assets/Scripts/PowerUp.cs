using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUp : MonoBehaviour, ICollectable
{
    public event Action<PowerUp> OnPickup;
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
        //Give player power up
        gameObject.SetActive(false);
    }
}
