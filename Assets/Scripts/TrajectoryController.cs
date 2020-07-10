using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryController : MonoBehaviour
{
    [SerializeField]
    private GameObject _trajectoryDotPrefab;
    [SerializeField]
    private GameObject _ground;
    
    private const float _DOT_TIME_STEP = 0.1f;
    private const int _DOTS_COUNT = 20;
    private GameObject[] _trajectoryDots = new GameObject[_DOTS_COUNT];
    private GameObject _trajectory;
    private float _groundLevel;


    public void Initialize()
    {
        _groundLevel = _ground.transform.position.y + _ground.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        _trajectory = new GameObject("Trajectory");
        for (int i = 0; i < _DOTS_COUNT; i++)
        {
            _trajectoryDots[i] = Instantiate(_trajectoryDotPrefab, _trajectory.transform);
            _trajectoryDots[i].SetActive(false);
        }
    }

    public void Draw(float p_mass, float p_gravityScale, float p_force, Vector3 p_initialPos)
    {
        for (int i = 0; i < _DOTS_COUNT; i++)
        {
            _trajectoryDots[i].transform.position = CalculatePosition(_DOT_TIME_STEP * i, p_mass, p_gravityScale, p_force, p_initialPos);
            if (_trajectoryDots[i].transform.position.y > _groundLevel)
            {
                _trajectoryDots[i].SetActive(true);
            }
        }
    }

    public void Hide()
    {
        for (int i = 0; i < _DOTS_COUNT; i++)
        {
            _trajectoryDots[i].SetActive(false);
        }
    }

    private Vector2 CalculatePosition(float p_elapsedTime, float p_mass, float p_gravityScale, float p_force, Vector3 p_initialPos)
    {
        float velocity = (p_force / p_mass) * Time.fixedDeltaTime;
        Vector2 initialPos_vec2 = p_initialPos;
        Vector2 outPosition = p_gravityScale * Physics2D.gravity * p_elapsedTime * p_elapsedTime * 0.5f + new Vector2(velocity * p_elapsedTime, velocity * p_elapsedTime) + initialPos_vec2;

        return outPosition;
    }
}
