using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
       if(PauseMenu.GameIsPaused == true)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("newMusic");
            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }
            if (SceneManager.GetActiveScene().buildIndex == 2)
                DontDestroyOnLoad(this.gameObject);
            else if(SceneManager.GetActiveScene().buildIndex != 2)
                Destroy(this.gameObject);
        } 
    }
}
