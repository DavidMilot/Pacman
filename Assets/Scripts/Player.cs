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

    SpriteRenderer _playerSpriteRenderer;
    Animator _playerAnimator;
    Rigidbody2D _playerRigidbody2D;

    PlayerState _playerState = PlayerState.Idle;
    Enums.MoveDirection _playerMoveDirection = Enums.MoveDirection.None;
    Vector3 _curPosition,_targetPosition;
    float _journeyLength, _startTime;
    float _distCovered, _fractionOfJourney;
    float _moveSpeed = 3.0f;
    Enums.MoveDirection dir = Enums.MoveDirection.None;

    float _drawDistance = 1.0f;

    void Update()
    {
        //CheckValidMoveInput();
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
                    //Debug.Log("Hit = " + hit.collider.transform.name);
                    _playerState = PlayerState.Idle;
                    //_fractionOfJourney = 1.0f;
                }
                else
                {
                    //Debug.Log("Hit GOod?= " + hit.collider.transform.name);
                    safeToMove = true; 
                }
            }

            if (safeToMove || hit.collider == null)
            {
                _startTime = Time.time;
                _fractionOfJourney = 0;
                _curPosition = transform.position;

                ChangeAnimation(_playerMoveDirection);

                _playerMoveDirection = dir;
                _journeyLength = Vector3.Distance(_curPosition, _curPosition + GetMoveStepDistance(_playerMoveDirection));
                _playerState = PlayerState.Moving;
            }

            Debug.Log("Player state = " + _playerState);
        }
        else if(_playerState == PlayerState.Moving)
        {
            MovePlayer(GetMoveStepDistance(_playerMoveDirection) + _curPosition);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<Animator>();
        _fractionOfJourney = 1.0f;
    }

    private bool CheckWallCollision()
    {
        UnityEngine.Debug.DrawRay(transform.position, GetMoveStepDistance(_playerMoveDirection), Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, GetMoveStepDistance(_playerMoveDirection), _drawDistance);

        if (hit.collider != null)
        {
            if (hit.collider.transform.name == "Wall" || 
                hit.collider.transform.name == "Door" || 
                hit.collider.transform.name == "House")
            {
                return true;
            }
        }
        return false;
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

    public void MovePlayer(Vector3 newTargetPos)
    {
        _distCovered = (Time.time - _startTime) * _moveSpeed;

        // Fraction of journey completed equals current distance divided by total distance.
        _fractionOfJourney = _distCovered / _journeyLength;

        /*
        Debug.Log("Current Postion = " + _curPosition);
        Debug.Log("newTarPos = " + newTargetPos);
        Debug.Log("Frac = " + _fractionOfJourney);
        */
        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(_curPosition, newTargetPos, _fractionOfJourney);

        if (_fractionOfJourney >= 1.0f)
        {
            _curPosition = transform.position;

            //CheckValidMoveInput();

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
                _startTime = Time.time;
                _fractionOfJourney = 0;
                _curPosition = transform.position;
                _playerMoveDirection = dir;
                ChangeAnimation(_playerMoveDirection);

                _journeyLength = Vector3.Distance(_curPosition, _curPosition + GetMoveStepDistance(_playerMoveDirection));
                _playerState = PlayerState.Moving;
            }
        }

        /*
        float elapsedTime = 0;
        _curPosition = transform.position;
        _targetPosition = _curPosition + direction;

        while (elapsedTime < _moveTime)
        {
            transform.position = Vector3.Lerp(_curPosition, _targetPosition, (elapsedTime / _moveTime));
            elapsedTime += Time.deltaTime;
        }

        transform.position = _targetPosition;*/
    }

    void CheckValidMoveInput()
    {
        //Check for input regardless of state of player, direction of input will be kept until the next direction is pressed
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

        if (dir != Enums.MoveDirection.None)
        {
            UnityEngine.Debug.DrawRay(transform.position, GetMoveStepDistance(dir), Color.green);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, GetMoveStepDistance(dir), _drawDistance);

            if (hit.collider != null)
            {
                if (hit.collider.transform.name == "Wall" || 
                    hit.collider.transform.name == "Door" || 
                    hit.collider.transform.name == "House")
                {
                    //Can't move in that direction
                }
                else
                {
                    if (dir != Enums.MoveDirection.None)
                    {
                        _playerMoveDirection = dir;
                    }
                }
            }
        }
    }

    Vector3 GetMoveStepDistance(Enums.MoveDirection direction)
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
}
