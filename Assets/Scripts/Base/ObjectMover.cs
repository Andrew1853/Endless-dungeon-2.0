using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 _currentDir;
    Vector3 _targetPos;
    float _roundMovementDesiredDistance = 1f;
    [SerializeField] float breakDistance = .01f;
    public bool ReachDestination { get => reachDestination; }
    bool reachDestination = true;
    public Vector2Int CurrentPosInt { get => Vector2Int.FloorToInt(transform.position); }
    public Vector2Int TargetPosInt { get => Vector2Int.FloorToInt(_targetPos); }
    public bool IsActive { get => _isActive; set => _isActive = value; }

    public Vector3 prevPos;

    bool _isActive = false;
    bool _roundMovement = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 offset = rb.position - centerPoint;
        _angle = Mathf.Atan2(offset.y, offset.x);
    }
    [SerializeField] float _angle;
    [SerializeField] float _angularSpeed;
    [SerializeField] Vector2 centerPoint;
    public float radius = 5f;
    public float positionCorrectionSpeed = 2f;

    float _currentDistance;
    Vector2 _desiredPosition;
    Vector2 _direction;
    private void Update()
    {
        if (_isActive == false)
        {
            return;
        }

        if (_roundMovement == false)
        {
            if (Vector2.Distance(transform.position, _targetPos) < breakDistance)
            {
                reachDestination = true;
                rb.velocity = Vector2.zero;
                _isActive = false;
                return;
            }
            else
            {
                _currentDir = (_targetPos - transform.position).normalized;

            }
        }
        else
        {
            _desiredPosition = (Vector2)_targetPos + new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle)) * radius;

            _currentDistance = Vector2.Distance(transform.position, _targetPos);
        }

        Vector2Int prevPosInt = Vector2Int.FloorToInt(prevPos);
        if (prevPosInt != CurrentPosInt)
        {
            GameManager.ChunkManager.MoveObject(prevPosInt, CurrentPosInt, gameObject);
        }
        prevPos = transform.position;
    }
    private void FixedUpdate()
    {
        if (IsActive)
        {
            if (_roundMovement)
            {
                HandleMoveAroundPos();
            }
            else
            {
                rb.velocity = _currentDir;
            }
        }
    }
    public void MoveToPos(Vector2Int pos)
    {
        _roundMovement = false;

        _isActive = true;
        reachDestination = false;
        _targetPos = new Vector3(pos.x, pos.y);
    }
    public void MoveAroundPosition(Vector2Int pos, float desiredDistance, bool rightHandDir)
    {
        IsActive = true;

        _roundMovement = true;

        if (rightHandDir)
            _angularSpeed = 1;
        else
            _angularSpeed = -1;
        radius = desiredDistance;
        _targetPos = (Vector2)pos;
    }
    public void Stop()
    {
        _roundMovement = false;
        IsActive = false;
        rb.velocity = Vector2.zero;
    }
    void HandleMoveAroundPos()
    {
        if (Mathf.Abs(_currentDistance - radius) > 0.01f)
        {
            _direction = (_desiredPosition - rb.position).normalized;

            rb.MovePosition(Vector2.MoveTowards(rb.position, _desiredPosition, Time.fixedDeltaTime));
        }
        else
        {
            _angle += _angularSpeed * Time.fixedDeltaTime;

            rb.MovePosition(_desiredPosition);
        }
    }
    public bool Collide()
    {
        List<GameObject> o = GameManager.ChunkManager.grid.cells[CurrentPosInt].objects;
        foreach (var item in o)
        {
            if (item != gameObject)
            {
                return true;
            }
        }
        return false;
    }
}
