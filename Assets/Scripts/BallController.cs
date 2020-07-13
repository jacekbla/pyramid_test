using System;
using UnityEngine;

/// <summary>
/// This class is responsible for preparing, launching and collision detection of the ball 
/// controlled by player.
/// It subscribes to method invoked on Restart button click, defined in UIController class.
/// It contains definitions of events invoked when ball hits the target and when it misses.
/// Declared const variables control initial and maximum force, initial increase of force 
/// and the magnitude of that increase.
/// </summary>
public class BallController : MonoBehaviour
{
    public static event Action onHit;
    public static event Action onMiss;

    [SerializeField]
    private TrajectoryController _trajectory;

    [Header("Colliders")]
    [SerializeField]
    private BoxCollider2D _groundCollider;
    [SerializeField]
    private BoxCollider2D _holeCollider;

    private const float _MAX_FORCE = 400.0f;
    private const float _FORCE_INCREASE_WITH_LEVEL = 30.0f;
    private const float _INITIAL_FORCE = 50.0f;
    private const float _INITIAL_FORCE_INCREASE = 40.0f;

    private float _forceIncrementSpeed = _INITIAL_FORCE_INCREASE;
    private float _force = _INITIAL_FORCE;
    private bool _ballThrown = false;
    private Rigidbody2D _rigidbody;
    private Vector3 _originalPos;

    private void Awake()
    {
        _originalPos = transform.position;
        _rigidbody = GetComponent<Rigidbody2D>();
        _trajectory.Initialize();
    }

    private void OnEnable()
    {
        UIController.onRestart += Restart;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && _force <= _MAX_FORCE && !_ballThrown)
        {
            _force += _forceIncrementSpeed * Time.deltaTime;
            _trajectory.Draw(_rigidbody.mass, _rigidbody.gravityScale, _force, transform.position);
        }

        if ((Input.GetKeyUp(KeyCode.Space) || _force > _MAX_FORCE) && !_ballThrown)
        {
            _rigidbody.AddForce(new Vector2(_force, _force));
            _ballThrown = true;
        }
    }

    private void OnDisable()
    {
        UIController.onRestart -= Restart;
    }

    private void OnCollisionEnter2D(Collision2D p_collision)
    {
        if (_ballThrown)
        {
            Collider2D[] colliderArray = new Collider2D[2];
            int colliderCount = _rigidbody.OverlapCollider(new ContactFilter2D(), colliderArray);

            if (colliderCount > 1)
            {
                foreach (Collider2D c in colliderArray)
                {
                    if (c == _holeCollider)
                    {
                        onHit();

                        _forceIncrementSpeed += _FORCE_INCREASE_WITH_LEVEL;
                        PrepareNextThrow();
                    }
                }
            }
            else if (p_collision.collider == _groundCollider)
            {
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                onMiss();
            }
        }
    }

    private void PrepareNextThrow()
    {
        transform.position = _originalPos;
        _force = _INITIAL_FORCE;
        _ballThrown = false;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0.0f;
        _trajectory.Hide();
    }

    private void Restart()
    {
        PrepareNextThrow();
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _forceIncrementSpeed = _INITIAL_FORCE_INCREASE;
    }
}
