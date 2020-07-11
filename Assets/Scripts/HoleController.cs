using UnityEngine;

/// <summary>
/// This class is responsible for changing the position of the target flag along the X axis.
/// It subscribes to events that are invoked when the ball hits the target (BallController) 
/// and when Restart button is clicked (UIController).
/// Const values controlling the range of flag position are declared here.
/// </summary>
public class HoleController : MonoBehaviour
{
    private const float _MAX_X = 8.0f;
    private const float _MIN_X = 0.0f;

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

    private void RandomizePosition()
    {
        Vector3 position = transform.position;
        position.x = Random.Range(_MIN_X, _MAX_X);
        transform.position = position;
    }
}
