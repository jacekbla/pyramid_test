using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private const float _MAX_FORCE = 8.0f;
    private const float _FORCE_INCREMENT = 1.0f;

    private const int NUM_DOTS_TO_SHOW = 20;
    private const float DOT_TIME_STEP = 0.1f;
    [SerializeField]
    private GameObject trajectoryDotPrefab;

    private Rigidbody2D _rigidbody;
    private float _force;
    private bool _ballThrown = false;
    private GameObject[] _trajectoryDots;

    void Start()
    {
        _trajectoryDots = new GameObject[NUM_DOTS_TO_SHOW];

        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && _force <= _MAX_FORCE)
        {
            _force += _FORCE_INCREMENT * Time.deltaTime;
            Debug.Log(_force);

            for (int i = 0; i < NUM_DOTS_TO_SHOW; i++)
            {
                Destroy(_trajectoryDots[i]);
                GameObject trajectoryDot = Instantiate(trajectoryDotPrefab);
                trajectoryDot.transform.position = CalculatePosition(DOT_TIME_STEP * i);
                _trajectoryDots[i] = trajectoryDot;
            }
        }

        if ((Input.GetKeyUp(KeyCode.Space) || _force > _MAX_FORCE) && !_ballThrown)
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            //_rigidbody.AddForce(new Vector2(_force, _force));
            _rigidbody.velocity = new Vector2(_force, _force);
            _ballThrown = true;
        }
    }

    private Vector2 CalculatePosition(float elapsedTime)
    {
        float x = 0.0f * elapsedTime * elapsedTime * 0.5f + _force * elapsedTime + transform.position.x;
        float y = -9.81f * elapsedTime * elapsedTime * 0.5f + _force * elapsedTime + transform.position.y;

        return new Vector2(x, y);
    }
}
