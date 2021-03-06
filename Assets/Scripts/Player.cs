﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Audio;

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

    [SerializeField]
    private LayerMask _interactableLayer;

    private float _horizontalInput; // movement in x axis

    private float _verticalInput; // movement in y axis

    private float _checkCircleSize = .4f;

    private float _originalSpeed;

    private bool _isOnDeathFloor;

    private bool _isDead;

    private Vector3 _targetPosition;

    private Animator _anim;

    private bool _isInteracting;

    public void Start()
    {
        _anim = GetComponentInChildren<Animator>();

        transform.position = _startingPoint;

        _targetPosition = _startingPoint;

        _originalSpeed = _speed;
    }

    public void Update()
    {
        if (GameManager.INSTANCE.IsGameOver()) return;

        CheckForInteractable();

        if (IsAtTargetPosition())
        {
            if (_isOnDeathFloor && !_isDead)
            {
                transform.position = _targetPosition;

                Die();

                GameManager.INSTANCE.EndGame();

                _isDead = true;
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

            if (IsTargetPositionFree(_targetPosition) && (_horizontalInput != 0 || _verticalInput != 0)) MovementTracker.INSTANCE.EnableMovementHasOccured();

        }
    }

    private bool IsTargetPositionFree(Vector2 targetPosition)
    {
        if (!Physics2D.OverlapCircle(targetPosition, _checkCircleSize, _obstacle) && !Physics2D.OverlapCircle(targetPosition, _checkCircleSize, _interactableLayer))
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
        if (Physics2D.OverlapCircle(targetPosition, 0.3f, _deathLayer))
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

    private void CheckForInteractable()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(_horizontalInput, _verticalInput), 1f, _interactableLayer);

        Debug.DrawRay(transform.position, new Vector2(_horizontalInput, _verticalInput) * 1f, Color.green);

        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponentInParent<Interactable>().Interact();

            _isInteracting = true;
        }
        else
        {
            _isInteracting = false;
        }
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

    public bool IsInteracting()
    {
        return this._isInteracting;
    }

    public void Dead()
    {
        GameManager.INSTANCE.ReloadScene();
    }

    public void Die()
    {
        _isDead = true;

        _anim.SetTrigger("Death");

        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_04, false, 0f);
    }

    public bool IsDead()
    {
        return _isDead;
    }
}
