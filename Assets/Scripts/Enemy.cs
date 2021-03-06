﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Vector3[] _path;

    [SerializeField]
    private GameObject _playerToDetect;

    [SerializeField]
    private float _speed = 2f;

    [SerializeField]
    private float _runSpeed = 5f;

    [SerializeField]
    private LayerMask _obstacle;

    [SerializeField]
    private float _playerDetectRange;

    private float _checkCircleSize = .3f;

    private Vector3 _targetPosition;

    private bool _isPlayerDetected;

    private int _currentPathNodeIndex;

    private bool _isMovingForward;

    private bool _shouldStop;

    private bool _shouldFinishOffPath;

    private Animator _anim;

    private NavMeshAgent _agent;

    private bool _isAITakeOver;

    private Dictionary<Vector3, bool> exhasutedNodes = new Dictionary<Vector3, bool>();

    void Start()
    {

        _agent = GetComponent<NavMeshAgent>();

        _agent.updateRotation = false;

        _agent.updateUpAxis = false;

        transform.position = _path[0];

        _currentPathNodeIndex = 0;

        _targetPosition = _path[0];

        _isMovingForward = true;

        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (GameManager.INSTANCE.IsGameOver()) return;

        if (IsOnTopOfPlayer()) KillPlayer();

        if (_isAITakeOver)
        {
            Debug.Log("adads");
            _agent.SetDestination(_playerToDetect.transform.position);

            //Vector3 direction = _agent.destination - _agent.transform.position;

            //float xDir = 0f;
            //float yDir = 0f;

            //if (Mathf.Abs(direction.x) > 0.05f)
            //{
            //    xDir = direction.x;
            //    if (xDir < 0)
            //    {
            //        xDir = -1f;
            //    }
            //    else
            //    {
            //        xDir = 1f;
            //    }
            //}
            //else
            //{

            //    yDir = direction.y;
            //    if (yDir < 0)
            //    {
            //        yDir = -1f;
            //    }
            //    else
            //    {
            //        yDir = 1f;
            //    }
            //}

            //_agent.transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(xDir, yDir, 0f), _speed * Time.deltaTime);

            //Animate(new Vector3(x, y));

            return;
        }

        if (_shouldStop) return;

        //if (_isPlayerDetected)
        //{
        //    if (IsAtTargetPosition())
        //    {
        //        AssignTargetPositionToGetToPlayer();
        //    }
        //    else
        //    {
        //        Move();
        //    }
        //}
        //else
        //{
        if (IsAtTargetPosition())
        {
            AssignNewTargetNode();
        }
        else
        {
            Move();
        }

        IsPlayerInRange();
        //}
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
    }

    private bool IsTargetPositionFree(Vector2 _targetPosition)
    {
        if (!Physics2D.OverlapCircle(_targetPosition, _checkCircleSize, _obstacle))
        {
            return true;
        }

        return false;
    }

    private bool IsAtTargetPosition()
    {
        if (Vector3.Distance(transform.position, _targetPosition) <= .05f)
        {
            transform.position = _targetPosition;

            if (_shouldFinishOffPath && !_isPlayerDetected)
            {
                _isPlayerDetected = true;

                _shouldFinishOffPath = false;
            }
            return true;
        }

        return false;
    }

    private void AssignNewTargetNode()
    {
        if (_isMovingForward)
        {
            if (_currentPathNodeIndex + 1 == _path.Length)
            {
                _isMovingForward = false;

                _currentPathNodeIndex -= 1;
            }
            else
            {
                _currentPathNodeIndex += 1;
            }
        }
        else
        {
            if (_currentPathNodeIndex - 1 == -1)
            {
                _isMovingForward = true;

                _currentPathNodeIndex += 1;
            }
            else
            {
                _currentPathNodeIndex -= 1;
            }
        }

        _targetPosition = _path[_currentPathNodeIndex];

        Animate(_targetPosition);
    }

    private void Animate(Vector3 targetPosition)
    {
        if (targetPosition.x > transform.position.x)
        {
            FlipSprite(true);
            _anim.SetFloat("Horizontal Input", 1);
            _anim.SetFloat("Vertical Input", 0);

            return;
        }
        else if (targetPosition.x < transform.position.x)
        {
            FlipSprite(false);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            _anim.SetFloat("Horizontal Input", -1);
            _anim.SetFloat("Vertical Input", 0);

            return;
        }

        if (targetPosition.y > transform.position.y)
        {
            _anim.SetFloat("Vertical Input", 1);
            _anim.SetFloat("Horizontal Input", 0);

            return;
        }
        else if (targetPosition.y < transform.position.y)
        {
            _anim.SetFloat("Vertical Input", -1);
            _anim.SetFloat("Horizontal Input", 0);

            return;
        }
    }

    private bool IsPlayerInRange()
    {
        float distanceToPlayer = Vector3.Distance(_playerToDetect.transform.position, transform.position);

        if (distanceToPlayer <= _playerDetectRange)
        {
            float distanceBetweenTargetAndPlayerPosition = Vector3.Distance(_playerToDetect.transform.position, _targetPosition);

            if (distanceBetweenTargetAndPlayerPosition < distanceToPlayer)
            {
                _shouldFinishOffPath = true;

                Debug.Log("Player is in range");

                GameManager.INSTANCE.DisableControls();

                _speed = _runSpeed;

                _agent.speed = _runSpeed;

                _isAITakeOver = true;
            }
        }

        return false;
    }

    private bool IsOnTopOfPlayer()
    {
        if (Vector3.Distance(transform.position, _playerToDetect.transform.position) < 0.1f)
        {
            return true;
        }

        return false;
    }

    private void KillPlayer()
    {
        if (!_playerToDetect.GetComponent<Player>().IsDead())
        {
            _playerToDetect.GetComponent<Player>().Die();

        }
    }

    private void FlipSprite(bool shouldFlipRight = false)
    {
        if (shouldFlipRight)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    /*THIS METHOD HAS HAD LITTLE THINKING BEING PUT INTO IT*/
    /*
    private void AssignTargetPositionToGetToPlayer()
    {
        Vector3 ownPosition = transform.position;

        float xPos = Mathf.Round(ownPosition.x / 0.5f) * 0.5f;
        float yPos = Mathf.Round(ownPosition.y / 0.5f) * 0.5f;

        ownPosition = new Vector3(xPos, yPos);

        Vector3 playerPosition = _playerToDetect.transform.position;

        Vector3 targetPositon;

        //Debug.Log("Own Position "+ownPosition);
        //Debug.Log("Target Position "+_targetPosition);
        //Debug.Log("Player Position "+ playerPosition);

        if (Vector3.Distance(transform.position, _playerToDetect.transform.position) <= .05f)
        {
            _shouldStop = true;

            return;
        }

        if (playerPosition.x > ownPosition.x)
        {
            if (!exhasutedNodes.ContainsKey(ownPosition + Vector3.right))
            {
                if (IsTargetPositionFree(ownPosition + Vector3.right))
                {
                    targetPositon = ownPosition + Vector3.right;

                    _targetPosition = targetPositon;

                    exhasutedNodes.Add(targetPositon, true);

                    _anim.SetFloat("Horizontal Input", 1);
                    _anim.SetFloat("Vertical Input", 0);


                    return;
                }
                else
                {
                    if (!exhasutedNodes.ContainsKey(ownPosition + Vector3.up))
                    {
                        if (IsTargetPositionFree(ownPosition + Vector3.up))
                        {
                            targetPositon = ownPosition + Vector3.up;

                            _targetPosition = targetPositon;

                            exhasutedNodes.Add(targetPositon, true);

                            _anim.SetFloat("Horizontal Input", 0);
                            _anim.SetFloat("Vertical Input", 1);

                            return;
                        }
                    }
                    else
                    {
                        if (!exhasutedNodes.ContainsKey(ownPosition + Vector3.down))
                        {
                            if (IsTargetPositionFree(ownPosition + Vector3.down))
                            {
                                targetPositon = ownPosition + Vector3.down;

                                _targetPosition = targetPositon;

                                exhasutedNodes.Add(targetPositon, true);

                                _anim.SetFloat("Horizontal Input", 0);
                                _anim.SetFloat("Vertical Input", -1);

                                return;
                            }
                        }
                    }
                }
            }
        }
        else if (playerPosition.x < ownPosition.x)
        {
            if (!exhasutedNodes.ContainsKey(ownPosition + Vector3.left))
            {
                if (IsTargetPositionFree(ownPosition + Vector3.left))
                {
                    targetPositon = ownPosition + Vector3.left;

                    _targetPosition = targetPositon;

                    exhasutedNodes.Add(targetPositon, true);

                    _anim.SetFloat("Horizontal Input", -1);
                    _anim.SetFloat("Vertical Input", 0);

                    return;
                }
                else
                {
                    if (!exhasutedNodes.ContainsKey(ownPosition + Vector3.up))
                    {
                        if (IsTargetPositionFree(ownPosition + Vector3.up))
                        {
                            targetPositon = ownPosition + Vector3.up;

                            _targetPosition = targetPositon;

                            exhasutedNodes.Add(targetPositon, true);

                            _anim.SetFloat("Horizontal Input", 0);
                            _anim.SetFloat("Vertical Input", 1);

                            return;
                        }
                    }
                    else
                    {
                        if (!exhasutedNodes.ContainsKey(ownPosition + Vector3.down))
                        {
                            if (IsTargetPositionFree(ownPosition + Vector3.down))
                            {
                                targetPositon = ownPosition + Vector3.down;

                                _targetPosition = targetPositon;

                                exhasutedNodes.Add(targetPositon, true);

                                _anim.SetFloat("Horizontal Input", 0);
                                _anim.SetFloat("Vertical Input", -1);

                                return;
                            }
                        }
                    }
                }
            }
        }


        if (playerPosition.y > ownPosition.y)
        {
            if (!exhasutedNodes.ContainsKey(ownPosition + Vector3.up))
            {
                if (IsTargetPositionFree(ownPosition + Vector3.up))
                {
                    targetPositon = ownPosition + Vector3.up;

                    _targetPosition = targetPositon;

                    exhasutedNodes.Add(targetPositon, true);

                    _anim.SetFloat("Horizontal Input", 0);
                    _anim.SetFloat("Vertical Input", 1);

                    return;
                }
                else
                {
                    if (!exhasutedNodes.ContainsKey(ownPosition + Vector3.right))
                    {
                        if (IsTargetPositionFree(ownPosition + Vector3.right))
                        {
                            targetPositon = ownPosition + Vector3.right;

                            _targetPosition = targetPositon;

                            exhasutedNodes.Add(targetPositon, true);

                            _anim.SetFloat("Horizontal Input", 1);
                            _anim.SetFloat("Vertical Input", 0);

                            return;
                        }
                    }
                    else
                    {
                        if (!exhasutedNodes.ContainsKey(ownPosition + Vector3.left))
                        {
                            if (IsTargetPositionFree(ownPosition + Vector3.left))
                            {
                                targetPositon = ownPosition + Vector3.left;

                                _targetPosition = targetPositon;

                                exhasutedNodes.Add(targetPositon, true);

                                _anim.SetFloat("Horizontal Input", -1);
                                _anim.SetFloat("Vertical Input", 0);

                                return;
                            }
                        }
                    }
                }
            }
        }
        else if (playerPosition.y < ownPosition.y)
        {
            if (!exhasutedNodes.ContainsKey(ownPosition + Vector3.down))
            {
                if (IsTargetPositionFree(ownPosition + Vector3.down))
                {
                    targetPositon = ownPosition + Vector3.down;

                    _targetPosition = targetPositon;

                    exhasutedNodes.Add(targetPositon, true);

                    _anim.SetFloat("Horizontal Input", 0);
                    _anim.SetFloat("Vertical Input", -1);

                    return;
                }
                else
                {
                    if (!exhasutedNodes.ContainsKey(ownPosition + Vector3.right))
                    {
                        if (IsTargetPositionFree(ownPosition + Vector3.right))
                        {
                            targetPositon = ownPosition + Vector3.right;

                            _targetPosition = targetPositon;

                            exhasutedNodes.Add(targetPositon, true);

                            _anim.SetFloat("Horizontal Input", 1);
                            _anim.SetFloat("Vertical Input", 0);

                            return;
                        }
                    }
                    else
                    {
                        if (!exhasutedNodes.ContainsKey(ownPosition + Vector3.left))
                        {
                            if (IsTargetPositionFree(ownPosition + Vector3.left))
                            {
                                targetPositon = ownPosition + Vector3.left;

                                _targetPosition = targetPositon;

                                exhasutedNodes.Add(targetPositon, true);

                                _anim.SetFloat("Horizontal Input", -1);
                                _anim.SetFloat("Vertical Input", 0);

                                return;
                            }
                        }
                    }
                }
            }
        }
    }
    */
}
