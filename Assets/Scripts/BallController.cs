using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameManager;

    [Header("Colliders")]
    [SerializeField]
    private BoxCollider2D _groundCollider;
    [SerializeField]
    private BoxCollider2D _holeCollider;

    [Header("UI Elements")]
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private GameObject _gameOverPanel;

    private const float _MAX_FORCE = 400.0f;
    private const float _FORCE_INCREASE_WITH_LEVEL = 30.0f;
    private const float _INITIAL_FORCE = 50.0f;
    
    private TrajectoryController _trajectory;
    private float _forceIncrementSpeed = 40.0f;
    private Rigidbody2D _rigidbody;
    private float _force = _INITIAL_FORCE;
    private int _score = 0;
    private bool _ballThrown = false;
    private Vector3 _originalPos;

    void Awake()
    {
        _gameOverPanel.SetActive(false);
        _originalPos = gameObject.transform.position;
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        
        _trajectory = _gameManager.GetComponent<TrajectoryController>();
        _trajectory.initialize();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && _force <= _MAX_FORCE && !_ballThrown)
        {
            _force += _forceIncrementSpeed * Time.deltaTime;
            _trajectory.draw(_rigidbody.mass, _rigidbody.gravityScale, _force, transform.position);
        }

        if ((Input.GetKeyUp(KeyCode.Space) || _force > _MAX_FORCE) && !_ballThrown)
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody.AddForce(new Vector2(_force, _force));
            _ballThrown = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D p_other)
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
                        _score++;
                        _forceIncrementSpeed += _FORCE_INCREASE_WITH_LEVEL;
                        _scoreText.text = _score.ToString();
                        Restart();
                    }
                }
            }
            else if (p_other == _groundCollider)
            {
                float bestScore = PlayerPrefs.GetFloat("BestScore");
                Text[] textArray = _gameOverPanel.GetComponentsInChildren<Text>();
                textArray[1].text = "Score: " + _score;
                textArray[2].text = "Best: " + bestScore;

                _gameOverPanel.SetActive(true);
                if (_score > bestScore)
                {
                    PlayerPrefs.SetFloat("BestScore", _score);
                }
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    private void Restart()
    {
        transform.position = _originalPos;
        _force = _INITIAL_FORCE;
        _ballThrown = false;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        _trajectory.hide();
    }
}
