using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour, IMovement
{
    public enum GhostState
    {
        Home,
        Escape,
        Move,
        Flee,
        Dead
    }

    SpriteRenderer _ghostSpriteRenderer;
    Animator _ghostAnimator;
    Rigidbody2D _ghostRigidbody2D;

    GhostState _ghostState = GhostState.Home;
    public GhostState CurrentGhostState
    {
        get { return _ghostState; }
    }

    Enums.MoveDirection _ghostMoveDirection = Enums.MoveDirection.None;
    Enums.MoveDirection dir = Enums.MoveDirection.None;

    float _journeyLength, _startTime;
    float _distCovered, _fractionOfJourney;
    float _moveSpeed = 2.0f;
    float _drawDistance = 1.0f;
    float _randomizeMoveTimer = 1.0f;

    Vector3 _curPosition, _targetPosition;
    RaycastHit2D hit;

    void Start()
    {
        _ghostSpriteRenderer = GetComponent<SpriteRenderer>();
        _ghostAnimator= GetComponent<Animator>();
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
            Debug.Log(_fractionOfJourney);
            dir = Enums.MoveDirection.None;
            _curPosition = transform.position;

            hit = Physics2D.Raycast(transform.position, Player.Instance.GetMoveStepDistance(dir), _drawDistance);
            UnityEngine.Debug.DrawRay(transform.position, Player.Instance.GetMoveStepDistance(dir), Color.green);

            bool safeToMove = false;

            if (hit.collider != null)
            {
                if (_ghostState == GhostState.Move)
                {
                    Debug.Log("Move = " + Time.time);
                    //Hit something, try different direction
                    if (hit.collider.transform.name == "Wall" || hit.collider.transform.name == "Door" || hit.collider.transform.name == "House")
                    {
                        dir = RandomizeDirection();
                        safeToMove = true;
                    }
                }
                else if (_ghostState == GhostState.Home)
                {
                }
            }
            if (hit.collider == null || safeToMove)
            {
                NewDirection();
            }
        }
    }

    private void ChangeAnimation(Enums.MoveDirection moveDir)
    {
        _ghostAnimator.SetBool("Left", false);
        _ghostAnimator.SetBool("Right", false);
        _ghostAnimator.SetBool("Up", false);
        _ghostAnimator.SetBool("Down", false);
        switch (moveDir)
        {
            case Enums.MoveDirection.Up:
                _ghostAnimator.SetBool("Up", true);
                break;
            case Enums.MoveDirection.Right:
                _ghostAnimator.SetBool("Right", true);
                break;
            case Enums.MoveDirection.Left:
                _ghostAnimator.SetBool("Left", true);
                break;
            case Enums.MoveDirection.Down:
                _ghostAnimator.SetBool("Down", true);
                break;
        }
    }

    private Enums.MoveDirection RandomizeDirection()
    {
        return (Enums.MoveDirection)UnityEngine.Random.Range(1,5);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Game)
        {
            //Bounce around inside the House cells
            if (_ghostState == GhostState.Home)
            {
                /*
                hit = Physics2D.Raycast(transform.position, Player.Instance.GetMoveStepDistance(dir), _drawDistance);

                if (hit.collider != null)
                {
                    if (hit.collider.transform.name == "Wall")
                    {
                        if (dir == Enums.MoveDirection.Left)
                        {
                            dir = Enums.MoveDirection.Right;
                        }
                        else
                        {
                            dir = Enums.MoveDirection.Left;
                        }

                        if (safeToMove || hit.collider == null)
                        {
                            NewDirection();
                        }
                    }
                    else
                    {
                        safeToMove = true;
                    }
                }

                Move(Player.Instance.GetMoveStepDistance(_ghostMoveDirection) + _curPosition);*/
            }
            else if (_ghostState == GhostState.Escape)
            {
                dir = Enums.MoveDirection.Up;
                hit = Physics2D.Raycast(transform.position, Player.Instance.GetMoveStepDistance(dir), _drawDistance);

                if (hit.collider != null)
                {
                    if (hit.collider.transform.name == "Wall")
                    {
                        dir = RandomLeftRight();
                        NewDirection();
                        _ghostState = GhostState.Move;
                    }
                    /*
                    else if (hit.collider.transform.name == "Door")
                    {
                        safeToMove = true;
                    }*/
                }

                /*
                if (safeToMove || hit.collider == null)
                {
                    NewDirection();
                    _ghostState = GhostState.Move;
                }*/
                Move(Player.Instance.GetMoveStepDistance(_ghostMoveDirection) + _curPosition);
            }
            else if (_ghostState == GhostState.Move)
            {
                Move(Player.Instance.GetMoveStepDistance(_ghostMoveDirection) + _curPosition);
            }
        }
    }

    private void NewDirection()
    {
        _startTime = Time.time;
        _fractionOfJourney = 0;
        _curPosition = transform.position;
        _ghostMoveDirection = dir;

        ChangeAnimation(_ghostMoveDirection);

        _journeyLength = Vector3.Distance(_curPosition, _curPosition + Player.Instance.GetMoveStepDistance(_ghostMoveDirection));
    }

    private Enums.MoveDirection RandomLeftRight()
    {
        return (Enums.MoveDirection)UnityEngine.Random.Range(1, 3);
    }

    public void EscapeGhost()
    {
        _ghostState = GhostState.Escape;
        dir = Enums.MoveDirection.Up;
        NewDirection();
    }
}
