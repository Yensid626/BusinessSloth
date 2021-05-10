using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    public static UIMgr inst;
    // Start is called before the first frame update
    private void Awake()
    {
        inst = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    internal void Tick(float dt)
    {
        Score.inst.Tick(dt);
        AlertSystem.inst.Tick(dt);
    }

    public void gameOver()
    {
        GameObject screen = GameObject.Find("PointScreen").gameObject;
        if (screen != null)
        {
            screen.transform.Find("Menu").gameObject.SetActive(true);
        }
    }
}
