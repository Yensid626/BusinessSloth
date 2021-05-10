using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreGameOver : MonoBehaviour
{
    public static ScoreGameOver inst;

    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Tick(float dt)
    {
        if(Score.inst != null && Score.inst.GetPoints() <= -20)
        {
            ScoreFinal();
        }
    }

    public void ScoreFinal()
    {
        PlayerPrefs.SetFloat("FinalScore", Score.inst.GetPoints());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
