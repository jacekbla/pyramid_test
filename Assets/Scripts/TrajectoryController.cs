using UnityEngine;

/// <summary>
/// This class prepares, enables and disables trajectory dots.
/// Its public methods are called in BallController.
/// The distance between each dot and their overall count is controlled with consts.
/// </summary>
public class TrajectoryController : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField]
    private GameObject _trajectoryDotPrefab;
    [SerializeField]
    private GameObject _ground;
    
    private const float _DOT_TIME_STEP = 0.1f;
    private const int _DOTS_COUNT = 20;
    private const string _TRAJECTORY_NAME = "Trajectory";

    private readonly GameObject[] _trajectoryDots = new GameObject[_DOTS_COUNT];

    private GameObject _trajectory;
    private float _groundLevel;

    /// <summary>
    /// Prepares trajectory: calculates ground level Y coordinate and creates 
    /// trajectory game object.
    /// </summary>
    public void Initialize()
    {
        _groundLevel = _ground.transform.position.y + _ground.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        _trajectory = new GameObject(_TRAJECTORY_NAME);
        for (int i = 0; i < _DOTS_COUNT; i++)
        {
            _trajectoryDots[i] = Instantiate(_trajectoryDotPrefab, _trajectory.transform);
            _trajectoryDots[i].SetActive(false);
        }
    }

    /// <summary>
    /// This method is used to determine which trajectory dots should be active.
    /// </summary>
    /// <param name="p_mass">float - mass of Ball game object</param>
    /// <param name="p_gravityScale">float - gravity scale of Ball game object</param>
    /// <param name="p_force">float - force with which Ball game object is being launched. 
    /// It is applied along both X and Y axes.</param>
    /// <param name="p_initialPos">Vector3 - Ball starting position</param>
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

    /// <summary>
    /// Disables trajectory dots.
    /// </summary>
    public void Hide()
    {
        for (int i = 0; i < _DOTS_COUNT; i++)
        {
            _trajectoryDots[i].SetActive(false);
        }
    }

    /// <summary>
    /// Calculates position at which the Ball game object would be after given time from 
    /// being launched.
    /// </summary>
    /// <param name="p_elapsedTime">float - time after which the ball reaches the 
    /// returned position</param>
    /// <param name="p_mass">float - mass of Ball game object</param>
    /// <param name="p_gravityScale">float - gravity scale of Ball game object</param>
    /// <param name="p_force">float - force with which Ball game object is being launched. 
    /// It is applied along both X and Y axes.</param>
    /// <param name="p_initialPos">Vector3 - Ball starting position</param>
    /// <returns>Vector2 - position reached after given time</returns>
    private Vector2 CalculatePosition(float p_elapsedTime, float p_mass, float p_gravityScale, float p_force, Vector3 p_initialPos)
    {
        float velocity = (p_force / p_mass) * Time.fixedDeltaTime;
        Vector2 initialPos_vec2 = p_initialPos;
        Vector2 outPosition = p_gravityScale * Physics2D.gravity * p_elapsedTime * p_elapsedTime * 0.5f + new Vector2(velocity * p_elapsedTime, velocity * p_elapsedTime) + initialPos_vec2;

        return outPosition;
    }
}
