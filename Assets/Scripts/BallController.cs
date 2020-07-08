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

    private const float _MAX_FORCE = 8.0f;
    private const float _FORCE_INCREMENT = 1.0f;
    private const float _DOT_TIME_STEP = 0.1f;
    private const int _DOTS_COUNT = 20;

    private Rigidbody2D _rigidbody;
    private float _force;
    private GameObject[] _trajectoryDots;
    private int _score;
    private bool _ballThrown = false;

    void Start()
    {
        _trajectoryDots = new GameObject[_DOTS_COUNT];

        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && _force <= _MAX_FORCE)
        {
            _force += _FORCE_INCREMENT * Time.deltaTime;
            Debug.Log(_force);

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
            //_rigidbody.AddForce(new Vector2(_force, _force));
            _rigidbody.velocity = new Vector2(_force, _force);
            _ballThrown = true;
        }
    }


    List<GameObject> currentCollisions = new List<GameObject>();

    //void OnCollisionEnter(Collision col)
    //{

    //    // Add the GameObject collided with to the list.
    //    currentCollisions.Add(col.gameObject);

    //    // Print the entire list to the console.
    //    foreach (GameObject gObject in currentCollisions)
    //    {
    //        if (gObject.GetComponent<BoxCollider2D>() == holeCollider)
    //        {
    //            Debug.Log("Win");
    //            _score++;
    //            scoreText.text = _score.ToString();
    //            Restart();
    //            return;
    //        }

    //    }
    //    // Print the entire list to the console.
    //    foreach (GameObject gObject in currentCollisions)
    //    {
    //        if (gObject.GetComponent<BoxCollider2D>() == groundCollider)
    //        {
    //            gameOverPanel.SetActive(true);
    //            Debug.Log("Fail");
    //            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    //            return;
    //        }
    //    }
    //}

    //void OnCollisionExit(Collision col)
    //{
    //    // Remove the GameObject collided with from the list.
    //    currentCollisions.Remove(col.gameObject);
    //}



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == holeCollider)
        {
            Debug.Log("Win");
            _score++;
            scoreText.text = _score.ToString();
            Restart();
            gameOverPanel.SetActive(false);
        }
        else if (other == groundCollider)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("Fail");
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private Vector2 CalculatePosition(float elapsedTime)
    {
        float x = 0.0f * elapsedTime * elapsedTime * 0.5f + _force * elapsedTime + transform.position.x;
        float y = -9.81f * elapsedTime * elapsedTime * 0.5f + _force * elapsedTime + transform.position.y;

        return new Vector2(x, y);
    }

    private void Restart()
    {
        transform.position = new Vector3(-4.0f, -2.64f, 0.0f);
        _force = 0.0f;
        _ballThrown = false;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        for (int i = 0; i < _DOTS_COUNT; i++)
        {
            Destroy(_trajectoryDots[i]);
        }
    }
}
