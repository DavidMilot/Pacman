using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance;

    [SerializeField] GameObject[] _ghosts;
    int _totalSpawnedGhosts = 0;

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

    void Init()
    {
        for (int i = 0; i < _ghosts.Length; i++)
        {
            //_ghosts[i].GetComponent<Ghost>()
        }
        _totalSpawnedGhosts = 0;
    }


    void SpawnNextGhost()
    {
        if (_totalSpawnedGhosts < 4)
        {
            _ghosts[_totalSpawnedGhosts].GetComponent<Ghost>().EscapeGhost();
            _totalSpawnedGhosts++;
        }
    }
}
