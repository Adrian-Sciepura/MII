using Pathfinding;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LandPlayerFollowerMovementSystem : WalkFromRightToLeft
{
    protected Transform _target;
    protected Path _path;
    protected Seeker _seeker;
    protected int _currentWaypoint = 0;
    protected bool _reachedEndOfPath = false;
    protected Coroutine _updateCoroutine = null;

    protected override void Awake()
    {
        base.Awake();
        _seeker = this.AddComponent<Seeker>();
    }

    protected override void Start()
    {
        _target = LevelManager.PlayerEntity.transform;
        _updateCoroutine = StartCoroutine(UpdatePath());
    }
        
    protected override void Update()
    {
        if (_path == null)
            return;

        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndOfPath = true;
            return;
        }
        else
        {
            _reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rigidBody.position).normalized;

        SetVelocity(_movementData.speed * direction.x, _rigidBody.velocity.y);

        float distance = Vector2.Distance(_rigidBody.position, _path.vectorPath[_currentWaypoint]);

        if (CheckWallCollider())
            Jump();

        if (distance < 3f)
            _currentWaypoint++;

        if ((_isFacingRight && direction.x < 0f) ||
            (!_isFacingRight && direction.x > 0f))
            Flip();

        _animator.SetBool("isRunning", true);
        _animator.SetBool("isGrounded", IsGrounded());
    }


    protected override void OnDestroy()
    {
        if (_updateCoroutine != null)
            StopCoroutine(_updateCoroutine);
        Destroy(_seeker);
    }

    protected void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    protected IEnumerator UpdatePath()
    {
        while(true)
        {
            if (_seeker.IsDone())
                _seeker.StartPath(_rigidBody.position, _target.position, OnPathComplete);

            yield return new WaitForSeconds(0.2f);
        }
    }
}