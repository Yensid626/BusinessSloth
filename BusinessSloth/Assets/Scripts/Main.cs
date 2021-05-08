using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    //public CameraMgr CameraMgr;
    //public EntityMgr EntityMgr;
    //public SelectionMgr SelectionMgr;

    // Update is called once per frame

    private bool awoken = false;
    private bool started = false;
    private void Awake()
    {
        awoken = true;
    }
    private void Start()
    {
        //Destroy(UIMgr.inst);
        started = true;
    }

    void Update()
    {
        if (awoken && started) { Tick(Time.deltaTime); }
    }

    internal void Tick(float dt)
    {
        EntityMgr.inst.Tick(dt);
        CameraMgr.inst.Tick(dt);
        UIMgr.inst.Tick(dt);

        ScoreGameOver.inst.Tick(dt);
    }
}
