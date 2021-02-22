using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DotManager : MonoBehaviour
{
    public static DotManager Instance;

    [SerializeField] UnityEvent OnCompleteEvent;

    [SerializeField] GameObject dot = null;
    [SerializeField] GameObject powerUp = null;
    [SerializeField] Transform cells = null;

    [SerializeField] List<Transform> powerUpTransforms;

    GameObject[] _dots;
    GameObject[] _powerUps;
    Transform[] _cells;

    int _totalDots = 0;
    int _currentDots = 0;
    public int CurrentDots
    {
        get { return _currentDots; }
    }
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

    /// <summary>
    /// Decrement from dot count and check if all dots are gone and if so, invoke OnComplete
    /// </summary>
    void UpdateDotCount()
    {
        _currentDots--;

        if (_currentDots == 0)
            OnCompleteEvent.Invoke();
    }

    /// <summary>
    /// Initialize all dots and powerup dots in the level and count the total of all dots plus powerUp dots
    /// </summary>
    void InitDots()
    {
        //Instantiate all the dots and powerups, keep a counter
        for (int i = 0; i < _cells.Length; i++)
        {
            //We don't want to count the cell space with the State 'Start' whcih does not have a dot on that space
            //That is used for the player
            if (_cells[i].GetComponent<Obstacle>().GameObstacle == Obstacle.Block.Empty)
            {
                _dots[i] = Instantiate(dot, _cells[i].position, Quaternion.identity);
                _dots[i].transform.SetParent(this.transform);
                _totalDots++;
            }
            else if(_cells[i].GetComponent<Obstacle>().GameObstacle == Obstacle.Block.PowerUp)
            {
                _dots[i] = Instantiate(powerUp, _cells[i].position, Quaternion.identity);
                _dots[i].transform.SetParent(this.transform);
                _totalDots++;
            }
        }
        _currentDots = _totalDots;

        for (int i = 0; i <= _totalDots; i++)
        {
            if (_dots[i] != null)
            {
                if (_dots[i].GetComponent<Dot>() != null)
                {
                    _dots[i].GetComponent<Dot>().OnPickup += HandlePickup;
                }
                else if (_dots[i].GetComponent<PowerUp>() != null)
                {
                    _dots[i].GetComponent<PowerUp>().OnPickup += HandlePickup;
                }
            }
        }

        /*
        for (int i = 0; i < _powerUps.Length; i++)
        {
            _powerUps[i].GetComponent<PowerUp>().OnPickup += HandlePickup;
        }*/
    }

    /// <summary>
    /// Reset all dots in the level and the power up dots
    /// </summary>
    public void ResetAllDots()
    {
        _currentDots = _totalDots;

        for (int i = 0; i <= _totalDots; i++)
        {
            if (_dots[i] != null)
            {
                _dots[i].SetActive(true);
            }
        }
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


