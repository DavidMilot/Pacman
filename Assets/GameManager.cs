using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState
    {
        Start,
        Intro,
        Clear,
        Game
    }

    [SerializeField] TextMeshProUGUI _startText;

    GameState _gameState;
    float _musicTimer = 0;
    int _lastDotCount = 0;
    float _ghostSpeedMul = 1.0f;

    public float GhostSpeedMul
    {
        set
        {
            _ghostSpeedMul = value;
        }
        get
        {
            return _ghostSpeedMul;
        }
    }
    public GameState CurrentGameState
    {
        get{ return _gameState; }
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
        ScoreManager.Instance.ToggleScore();
        _gameState = GameState.Start;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_gameState)
        {
            case GameState.Start:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    AudioManager.Instance.PlaySound(AudioManager.AudioSoundEffects.GameStart,false);
                    _gameState = GameState.Intro;
                }
                break;
            case GameState.Intro:
                if (_musicTimer < AudioManager.Instance.GetSoundClipLength(AudioManager.AudioSoundEffects.GameStart))
                {
                    _musicTimer += Time.deltaTime;
                }
                else
                {
                    _musicTimer = 0;
                    AudioManager.Instance.PlaySiren(AudioManager.AudioSoundEffects.Siren1);
                    _startText.enabled = false;
                    ScoreManager.Instance.ToggleScore();
                    _gameState = GameState.Game;
                }
                break;
            case GameState.Clear:
                if (_musicTimer < AudioManager.Instance.GetSoundClipLength(AudioManager.AudioSoundEffects.GameStart))
                {
                    _musicTimer += Time.deltaTime;
                }
                else
                {
                    _musicTimer = 0;
                    ResetGame();
                }
                break;
            case GameState.Game:

                //use lastDotcount to avoid repeatingly calling PlaySiren function when player is on the same dot count
                if (_lastDotCount != DotManager.Instance.CurrentDots)
                {
                    if (DotManager.Instance.CurrentDots == 150)
                    {
                        AudioManager.Instance.PlaySiren(AudioManager.AudioSoundEffects.Siren2);
                        GhostSpeedMul = 1.1f;
                    }
                    else if (DotManager.Instance.CurrentDots == 100)
                    {
                        AudioManager.Instance.PlaySiren(AudioManager.AudioSoundEffects.Siren3);
                        GhostSpeedMul = 1.15f;
                    }
                    else if (DotManager.Instance.CurrentDots == 50)
                    {
                        AudioManager.Instance.PlaySiren(AudioManager.AudioSoundEffects.Siren4);
                        GhostSpeedMul = 1.2f;
                    }
                    else if (DotManager.Instance.CurrentDots == 25)
                    {
                        AudioManager.Instance.PlaySiren(AudioManager.AudioSoundEffects.Siren5);
                        GhostSpeedMul = 1.25f;
                    }
                    else
                    {
                        GhostSpeedMul = 1.0f;
                    }
                }

                _lastDotCount = DotManager.Instance.CurrentDots;
                break;
        }
    }

    public void StageComplete()
    {
        _gameState = GameState.Clear;
        AudioManager.Instance.StopAllSounds();
        AudioManager.Instance.PlaySound(AudioManager.AudioSoundEffects.Intermission, false);
        _musicTimer = 0;
    }

    public void ResetGame()
    {
        ScoreManager.Instance.ToggleScore();
        DotManager.Instance.ResetAllDots();
        AudioManager.Instance.StopAllSirens();
        Player.Instance.ResetPlayer();
        _startText.enabled = true;
        _gameState = GameState.Start;
        _musicTimer = 0;
    }
}
