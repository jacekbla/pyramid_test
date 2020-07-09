using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private GameObject trajectoryDotPrefab;
    [SerializeField]
    private BoxCollider2D groundCollider;
    [SerializeField]
    private BoxCollider2D holeCollider;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private GameObject gameOverPanel;

    private const float _MAX_FORCE = 400.0f;
    private const float _DOT_TIME_STEP = 0.1f;
    private const int _DOTS_COUNT = 20;

    private float _forceIncrementSpeed = 40.0f;
    private float _forceIncreaseWithLevel = 30.0f;
    private Rigidbody2D _rigidbody;
    private float _force;
    private GameObject[] _trajectoryDots;
    private int _score;
    private bool _ballThrown = false;
    private Vector3 _originalPos;

    void Start()
    {
        gameOverPanel.SetActive(false);
        _trajectoryDots = new GameObject[_DOTS_COUNT];
        _originalPos = gameObject.transform.position;
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && _force <= _MAX_FORCE && !_ballThrown)
        {
            _force += _forceIncrementSpeed * Time.deltaTime;

            for (int i = 0; i < _DOTS_COUNT; i++)
            {
                Destroy(_trajectoryDots[i]);
                GameObject trajectoryDot = Instantiate(trajectoryDotPrefab);
                trajectoryDot.transform.position = CalculatePosition(_DOT_TIME_STEP * i);
                _trajectoryDots[i] = trajectoryDot;
            }
        }

        if ((Input.GetKeyUp(KeyCode.Space) || _force > _MAX_FORCE) && !_ballThrown)
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody.AddForce(new Vector2(_force, _force));
            _ballThrown = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_ballThrown)
        {
            Collider2D[] colliderArray = new Collider2D[2];
            int colliderCount = _rigidbody.OverlapCollider(new ContactFilter2D(), colliderArray);

            if (colliderCount > 1)
            {
                foreach (Collider2D c in colliderArray)
                {
                    if (c == holeCollider)
                    {
                        _score++;
                        _forceIncrementSpeed += _forceIncreaseWithLevel;
                        scoreText.text = _score.ToString();
                        Restart();
                    }
                }
            }
            else if (other == groundCollider)
            {
                float bestScore = PlayerPrefs.GetFloat("BestScore");
                Text[] textArray = gameOverPanel.GetComponentsInChildren<Text>();
                textArray[1].text = "Score: " + _score;
                textArray[2].text = "Best: " + bestScore;

                gameOverPanel.SetActive(true);
                if (_score > bestScore)
                {
                    PlayerPrefs.SetFloat("BestScore", _score);
                }
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    private Vector2 CalculatePosition(float elapsedTime)
    {
        float velocity = (_force / _rigidbody.mass) * Time.fixedDeltaTime;
        Vector2 outPosition = _rigidbody.gravityScale * Physics2D.gravity * elapsedTime * elapsedTime * 0.5f + new Vector2(velocity * elapsedTime, velocity * elapsedTime) + new Vector2(transform.position.x, transform.position.y);

        return outPosition;
    }

    private void Restart()
    {
        transform.position = _originalPos;
        _force = 0.0f;
        _ballThrown = false;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        for (int i = 0; i < _DOTS_COUNT; i++)
        {
            Destroy(_trajectoryDots[i]);
        }
    }
}
