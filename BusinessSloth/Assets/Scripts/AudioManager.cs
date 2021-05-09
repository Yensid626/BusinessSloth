using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
    }
    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex < 2)
            DontDestroyOnLoad(this.gameObject);
        else
            Destroy(this.gameObject);
    }
}
