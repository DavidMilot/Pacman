using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DotManager : MonoBehaviour
{
    public static DotManager Instance;

    [SerializeField] UnityEvent OnCompleteEvent;

    [SerializeField] GameObject dot;
    [SerializeField] GameObject powerUp;
    [SerializeField] Transform cells;

    GameObject[] _dots;
    GameObject[] _powerUps;
    Transform[] _cells;

    int _totalDots = 0;

    void OnEnable()
    {/*
        for (int i = 0; i < _dots.Length; i++)
        {
            _dots[i].GetComponent<Dot>().OnPickup += HandlePickup;
        }

        for (int i = 0; i < _powerUps.Length; i++)
        {
            _powerUps[i].GetComponent<PowerUp>().OnPickup += HandlePickup;
        }*/
    }

    void HandlePickup(Dot dot)
    {
        dot.Collect();
        UpdateDotCount();
    }

    void HandlePickup(PowerUp powerUp)
    {
        powerUp.Collect();
        UpdateDotCount();
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

    void Start()
    {
        _cells = cells.GetComponentsInChildren<Transform>();
        _dots = new GameObject[_cells.Length];
        InitDots();
    }

    void UpdateDotCount()
    {
        _totalDots--;

        if (_totalDots == 0)
            OnCompleteEvent.Invoke();
    }

    void InitDots()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            _dots[i] = Instantiate(dot, _cells[i].position, Quaternion.identity);
            _dots[i].transform.SetParent(this.transform);
        }
        _totalDots = _dots.Length;

        
        for (int i = 0; i < _dots.Length; i++)
        {
            if (_dots[i].GetComponent<Dot>() != null)
            {
                _dots[i].GetComponent<Dot>().OnPickup += HandlePickup;
            }
        }

        /*
        for (int i = 0; i < _powerUps.Length; i++)
        {
            _powerUps[i].GetComponent<PowerUp>().OnPickup += HandlePickup;
        }*/
    }

    /*
    public Dot[] GetListDots()
    {
        Dot[] dots = new Dot[_dots.Length];

        for (int i = 0; i < _dots.Length; i++)
        {
            dots[i] = _dots[i].GetComponent<Dot>();
        }

        return dots;
    }*/
}


