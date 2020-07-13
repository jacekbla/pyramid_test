using UnityEngine;

/// <summary>
/// This class is responsible for changing the position of target flag along the X axis.
/// Range of flag positions is declared with sonst values.
/// It subscribes to events that are invoked when the ball hits the target (BallController) 
/// and when Restart button is clicked (UIController).
/// </summary>
public class HoleController : MonoBehaviour
{
    private const float _MAX_X = 8.0f;
    private const float _MIN_X = 0.0f;

    /// <summary>
    /// Randomizes flag position before first throw.
    /// </summary>
    private void Awake()
    {
        RandomizePosition();
    }

    private void OnEnable()
    {
        BallController.onHit += RandomizePosition;
        UIController.onRestart += RandomizePosition;
    }
    
    private void OnDisable()
    {
        BallController.onHit -= RandomizePosition;
        UIController.onRestart -= RandomizePosition;
    }

    /// <summary>
    /// Ranomizes flag component along the X axis.
    /// </summary>
    private void RandomizePosition()
    {
        Vector3 position = transform.position;
        position.x = Random.Range(_MIN_X, _MAX_X);
        transform.position = position;
    }
}
