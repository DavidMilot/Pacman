using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IMovement
{
    enum PlayerState
    {
        Idle,
        Moving,
        Dead
    }
    public static Player Instance;

    SpriteRenderer _playerSpriteRenderer;
    Animator _playerAnimator;
    Rigidbody2D _playerRigidbody2D;
    float _powerUpTime;
    PlayerState _playerState = PlayerState.Idle;
    bool _playerPowerUpEnabled = false;

    Enums.MoveDirection _playerMoveDirection = Enums.MoveDirection.None;
    Vector3 _curPosition,_targetPosition;
    float _journeyLength, _startTime;
    float _distCovered, _fractionOfJourney;
    float _moveSpeed = 3.0f;
    Enums.MoveDirection dir = Enums.MoveDirection.None;

    float _drawDistance = 1.0f;

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


    void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Game)
        {
            if (_playerState == PlayerState.Idle)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    dir = Enums.MoveDirection.Up;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    dir = Enums.MoveDirection.Down;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    dir = Enums.MoveDirection.Left;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    dir = Enums.MoveDirection.Right;
                }

                RaycastHit2D hit = Physics2D.Raycast(transform.position, GetMoveStepDistance(dir), _drawDistance);
                UnityEngine.Debug.DrawRay(transform.position, GetMoveStepDistance(dir), Color.green);

                bool safeToMove = false;

                if (hit.collider != null)
                {
                    if (hit.collider.transform.name == "Wall" || hit.collider.transform.name == "Door" || hit.collider.transform.name == "House")
                    {
                        _playerState = PlayerState.Idle;
                    }
                    else
                    {
                        safeToMove = true; 
                    }
                }

                if (safeToMove || hit.collider == null)
                {
                    NewDirection();
                }

                //Debug.Log("Player state = " + _playerState);
            }
            else if(_playerState == PlayerState.Moving)
            {
                Move(GetMoveStepDistance(_playerMoveDirection) + _curPosition);
            }

            if (_playerPowerUpEnabled == true)
            {
                _powerUpTime -= Time.deltaTime;
                if (_powerUpTime < 0)
                {
                    AudioManager.Instance.StopSound(AudioManager.AudioSoundEffects.PowerPellet);
                    _playerPowerUpEnabled = false;
                }
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<Animator>();
        _fractionOfJourney = 1.0f;
    }
    

    private void ChangeAnimation(Enums.MoveDirection moveDir)
    {
        if (_playerState == PlayerState.Moving)
        {
            _playerAnimator.SetBool("Moving", true);
            switch (moveDir)
            {
                case Enums.MoveDirection.Up:
                    //_player
                    _playerSpriteRenderer.flipY = false;
                    break;
                case Enums.MoveDirection.Right:
                    _playerSpriteRenderer.flipX = false;
                    break;
                case Enums.MoveDirection.Left:
                    _playerSpriteRenderer.flipX = true;
                    break;
                case Enums.MoveDirection.Down:
                    _playerSpriteRenderer.flipY = true;
                    break;
            }
        }

        if (_playerState == PlayerState.Idle)
        {
            _playerAnimator.SetBool("Moving", false);
        }
    }

    public void Move(Vector3 newTargetPos)
    {
        _distCovered = (Time.time - _startTime) * _moveSpeed;

        // Fraction of journey completed equals current distance divided by total distance.
        _fractionOfJourney = _distCovered / _journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(_curPosition, newTargetPos, _fractionOfJourney);

        if (_fractionOfJourney >= 1.0f)
        {
            _curPosition = transform.position;

            if (Input.GetKey(KeyCode.W))
            {
                dir = Enums.MoveDirection.Up;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir = Enums.MoveDirection.Down;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                dir = Enums.MoveDirection.Left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir = Enums.MoveDirection.Right;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, GetMoveStepDistance(dir), _drawDistance);
            UnityEngine.Debug.DrawRay(transform.position, GetMoveStepDistance(dir), Color.green);

            bool safeToMove = false;

            if (hit.collider != null)
            {
                if (hit.collider.transform.name == "Wall" || hit.collider.transform.name == "Door" || hit.collider.transform.name == "House")
                {
                    _playerState = PlayerState.Idle;
                }
                else
                {
                    safeToMove = true;
                }
            }
            if (safeToMove || hit.collider == null)
            {
                NewDirection();
            }
        }
    }

    private void NewDirection()
    {
        _startTime = Time.time;
        _fractionOfJourney = 0;
        _curPosition = transform.position;
        _playerMoveDirection = dir;

        ChangeAnimation(_playerMoveDirection);

        _journeyLength = Vector3.Distance(_curPosition, _curPosition + GetMoveStepDistance(_playerMoveDirection));
        _playerState = PlayerState.Moving;
    }

    public Vector3 GetMoveStepDistance(Enums.MoveDirection direction)
    {
        Vector3 dir = Vector3.zero;

        switch (direction)
        {
            case Enums.MoveDirection.Up:
                dir = new Vector3(0, 1.0f, 0);
                break;
            case Enums.MoveDirection.Right:
                dir = new Vector3(1.0f, 0, 0);
                break;
            case Enums.MoveDirection.Left:
                dir = new Vector3(-1.0f, 0f, 0);
                break;
            case Enums.MoveDirection.Down:
                dir = new Vector3(0, -1.0f, 0);
                break;
            case Enums.MoveDirection.None:
                dir = new Vector3(0, 0, 0);
                break;
        }

        return dir;
    }

    public void ResetPlayer()
    {
        _powerUpTime = 0;
        _playerPowerUpEnabled = false;
        transform.position = new Vector3(1.5f, -1.5f, 0);
        _playerState = PlayerState.Idle;
        _playerAnimator.SetBool("Moving", false);
        _playerAnimator.SetBool("Dead", false);
    }

    public void PowerUpPlayer(float powerUpTime)
    {
        _playerPowerUpEnabled = true;
        _powerUpTime = powerUpTime;
        AudioManager.Instance.PlaySound(AudioManager.AudioSoundEffects.PowerPellet, true);
    }
}
