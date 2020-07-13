using UnityEngine;

/// <summary>
/// This class ensures the same scene content for different screen aspect ratios.
/// It it done by changing the size of the camera. Empty space created this way 
/// is filled with sky/lower ground sprites.
/// </summary>
public class AspectRatioManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField]
    private GameObject _upperGround;

    [Header("Background Elements")]
    [SerializeField]
    private GameObject _sky;
    [SerializeField]
    private GameObject _lowerGround;

    [Header("Sprite Renderers")]
    [SerializeField]
    private SpriteRenderer _backgroundSpriteRenderer;

    private const float _STANDARD_ASPECT = 16.0f / 9.0f;

    private Camera _camera;

	private void Awake()
    {
        //determining size of the camera
        _camera = GetComponent<Camera>();
        float halfSize = _STANDARD_ASPECT * _camera.orthographicSize / _camera.aspect;

        _camera.orthographicSize = halfSize;

        //calculating upper ground lower edge y coordinate
        SpriteRenderer lowerGroundSpriteRenderer = _lowerGround.GetComponent<SpriteRenderer>();
        float upperGroundLowerEdgeY = _upperGround.transform.position.y - _upperGround.GetComponent<SpriteRenderer>().bounds.size.y / 2.0f;
        
        //calculating lower ground position
        Vector3 lowerGroundPos = _lowerGround.transform.position;
        lowerGroundPos.y = upperGroundLowerEdgeY - (halfSize + upperGroundLowerEdgeY) / 2.0f;
        _lowerGround.transform.position = lowerGroundPos;

        //changing lower ground size accordingly
        Vector2 lowerGroundSpriteSize = lowerGroundSpriteRenderer.size;
        lowerGroundSpriteSize.y = halfSize + upperGroundLowerEdgeY;
        lowerGroundSpriteRenderer.size = lowerGroundSpriteSize;


        //calculating background upper edge y coordinate
        SpriteRenderer skySpriteRenderer = _sky.GetComponent<SpriteRenderer>();
        float backgroundUpperEdgeY = _backgroundSpriteRenderer.bounds.size.y / 2.0f;
        
        //calculating sky position
        Vector3 skyPos = _sky.transform.position;
        skyPos.y = backgroundUpperEdgeY + (halfSize - backgroundUpperEdgeY) / 2.0f;
        _sky.transform.position = skyPos;

        //changing sky size accordingly
        Vector2 skySpriteSize = skySpriteRenderer.size;
        skySpriteSize.y = halfSize - backgroundUpperEdgeY;
        skySpriteRenderer.size = skySpriteSize;
    }
}
