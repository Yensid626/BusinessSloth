﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public float scoreAmount;
    public float pointsPerSecond;

    // Start is called before the first frame update
    void Start()
    {
        scoreAmount = 0f;
        pointsPerSecond = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Points: " + (int)scoreAmount;
        scoreAmount += pointsPerSecond * Time.deltaTime;
    }
}
