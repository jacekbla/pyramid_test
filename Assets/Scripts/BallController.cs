using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private const float _MAX_FORCE = 400.0f;
    private const float _FORCE_INCREMENT = 40.0f;

    private Rigidbody2D _rigidbody;
    private float _force;
    private bool _ballThrown = false;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && _force <= _MAX_FORCE)
        {
            _force += _FORCE_INCREMENT * Time.deltaTime;
            Debug.Log(_force);
        }

        if ((Input.GetKeyUp(KeyCode.Space) || _force > _MAX_FORCE) && !_ballThrown)
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody.AddForce(new Vector2(_force, _force));
            _ballThrown = true;
        }
    }
}
