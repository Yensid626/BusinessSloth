using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public float scoreAmount;
    //public float pointsPerSecond;

    public static Score inst;
    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        scoreAmount = 0f;
        //pointsPerSecond = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Points: " + (int)scoreAmount;
        //scoreAmount += pointsPerSecond * Time.deltaTime;
    }

    public void AddPoints(float amount)
    {
        scoreAmount += (amount > 0 ? amount : 0); //if amount is greater than 0, add that amount, otherwise add 0 points
    }

    public void RemovePoints(float amount)
    {
        scoreAmount -= (amount > 0 ? amount : 0); //if amount is greater than 0, subtract that amount, otherwise subtract 0 points
    }
}
