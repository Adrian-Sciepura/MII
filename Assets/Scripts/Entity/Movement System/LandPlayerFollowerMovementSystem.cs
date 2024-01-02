using Pathfinding;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class LandPlayerFollowerMovementSystem : WalkFromRightToLeft
{
    protected Transform _target;
    protected Path _path;
    protected Seeker _seeker;
    protected int _currentWaypoint = 0;
    protected bool _reachedEndOfPath = false;
    protected Coroutine _updateCoroutine = null;

    public override GameEntity context 
    { 
        get => _context; 
        set
        {
            base.context = value;

            _target = LevelManager.playerEntity.transform;
            _seeker = _context.AddComponent<Seeker>();

            _updateCoroutine = _context.StartCoroutine(UpdatePath());
        }
    }

    public override void Update()
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

        _rigidBody.velocity = new Vector2(_movementData.speed * direction.x, _rigidBody.velocity.y);

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

    public override void Dispose()
    {
        _context.StopCoroutine(_updateCoroutine);
        Object.Destroy(_seeker);
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