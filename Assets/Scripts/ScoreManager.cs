using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Events;
using System.Linq;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] TextMeshProUGUI _textScore;
    int _totalPoints = 0;
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

    public void UpdatePoints(int points)
    {
        _totalPoints += points;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        _textScore.text = _totalPoints.ToString();
    }
}
