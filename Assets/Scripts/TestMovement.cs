using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
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

    private float _horizontalInput; // movement in x axis

    private float _verticalInput; // movement in y axis

    private float _checkCircleSize = .4f;

    private Vector3 _targetPosition;

    public void Start()
    {
        transform.position = _startingPoint;

        _targetPosition = _startingPoint;
    }

    public void Update()
    {

        if (IsAtTargetPosition())
        {
            if (GameManager.INSTANCE.IsControlsDisabled()) return;

            InputListener();
        }
        else
        {
            if (IsTargetPositionFree(_targetPosition)) Move();
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
        }
    }

    private bool IsTargetPositionFree(Vector2 _targetPosition)
    {
        if (!Physics2D.OverlapCircle(_targetPosition, _checkCircleSize, _obstacle))
        {
            return true;
        }
        else
        {
            ResetTargetPosition();

            return false;
        }
    }

    private bool IsAtTargetPosition()
    {
        if (Vector3.Distance(transform.position, _targetPosition) <= .05f)
        {
            transform.position = _targetPosition;

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
}
