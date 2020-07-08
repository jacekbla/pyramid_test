using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HoleController : MonoBehaviour
{
    [SerializeField]
    private GameObject flagPole;
    [SerializeField]
    private GameObject flag;

    private float _MAX_X = 9.0f;
    private float _MIN_X = 5.0f;

    void Start ()
    {
        RandomizePosition();
    }
	
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        float posX = Random.Range(_MIN_X, _MAX_X);
        transform.position = new Vector3(posX, transform.position.y, 0.0f);
        flagPole.transform.position = new Vector3(posX, flagPole.transform.position.y, 0.0f);
        flag.transform.position = new Vector3(posX + 0.73f, flag.transform.position.y, 0.0f);
    }
}
