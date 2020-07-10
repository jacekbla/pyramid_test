﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HoleController : MonoBehaviour
{
    private float _MAX_X = 8.0f;
    private float _MIN_X = 0.0f;

    void Awake()
    {
        RandomizePosition();
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        float posX = Random.Range(_MIN_X, _MAX_X);

        Vector3 position = transform.parent.position;
        position.x = posX;
        transform.parent.position = position;
    }
}
