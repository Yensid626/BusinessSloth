using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver_ReturnToMainMenu : MonoBehaviour
{



    public void LoadMenu()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        //SceneManager.LoadScene("Main Menu");
    }

}

