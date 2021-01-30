using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _step = 1;

    [SerializeField]
    private float _speed = 2f;

    [SerializeField]
    private Vector2 _startingPoint;

    [SerializeField]
    private bool _isInverse;

    [SerializeField]
    private LayerMask _obstacle;

    [SerializeField]
    private LayerMask _iceLayer;

    [SerializeField]
    private LayerMask _deathLayer;

    private float _horizontalInput; // movement in x axis

    private float _verticalInput; // movement in y axis

    private float _checkCircleSize = .4f;

    private float _originalSpeed;

    private bool _isOnDeathFloor;

    private Vector3 _targetPosition;

    private Animator _anim;

    public void Start()
    {
        _anim = GetComponent<Animator>();

        transform.position = _startingPoint;

        _targetPosition = _startingPoint;

        _originalSpeed = _speed;
    }

    public void Update()
    {
        if (GameManager.INSTANCE.IsGameOver()) return;

        if (IsAtTargetPosition())
        {
            if (_isOnDeathFloor)
            {
                transform.position = _targetPosition;

                _anim.SetTrigger("Death");

                GameManager.INSTANCE.EndGame();
            }

            if (GameManager.INSTANCE.IsControlsDisabled()) return;

            InputListener();
        }
        else
        {

            CheckIfTargetPositionIsDeath(_targetPosition);

            if (IsTargetPositionIce(_targetPosition))
            {
                Move();

                return;
            }

            if (IsTargetPositionFree(_targetPosition))
            {
                Move();

                return;
            }
        }
    }

    private void InputListener()
    {
        if (Input.anyKeyDown)
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");

            _verticalInput = Input.GetAxisRaw("Vertical");


            if (_horizontalInput != 0)
            {
                if (_isInverse) _horizontalInput *= -1f;

                _verticalInput = 0f;

            }
            else if (_verticalInput != 0)
            {
                if (_isInverse) _verticalInput *= -1f;

                _horizontalInput = 0f;
            }

            _targetPosition = transform.position + new Vector3(_horizontalInput, _verticalInput) * _step;

            if (IsTargetPositionFree(_targetPosition)) MovementTracker.INSTANCE.EnableMovementHasOccured();

        }
    }

    private bool IsTargetPositionFree(Vector2 targetPosition)
    {
        if (!Physics2D.OverlapCircle(targetPosition, _checkCircleSize, _obstacle))
        {
            return true;
        }
        else
        {
            ResetTargetPosition();

            return false;
        }
    }

    private bool IsTargetPositionIce(Vector2 targetPosition)
    {
        if (Physics2D.OverlapCircle(targetPosition, _checkCircleSize, _iceLayer))
        {
            _speed = 4f;

            _targetPosition += new Vector3(_horizontalInput, _verticalInput);


            return true;
        }

        return false;
    }

    private void CheckIfTargetPositionIsDeath(Vector2 targetPosition)
    {
        if (Physics2D.OverlapCircle(targetPosition, _checkCircleSize, _deathLayer))
        {
            _isOnDeathFloor = true;
        }
    }

    private bool IsAtTargetPosition()
    {
        if (_isOnDeathFloor)
        {
            if (Vector3.Distance(transform.position, _targetPosition) <= .2f)
            {
                return true;
            }

            return false;
        }

        if (Vector3.Distance(transform.position, _targetPosition) <= .05f)
        {
            transform.position = _targetPosition;

            ResetSpeed();

            return true;
        }

        return false;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
    }

    private void ResetTargetPosition()
    {
        _targetPosition = transform.position;
    }

    public void SetSpeed(float newSpeed)
    {
        this._speed = newSpeed;
    }

    public void ResetSpeed()
    {
        this._speed = _originalSpeed;
    }
}
