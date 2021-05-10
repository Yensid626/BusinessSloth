using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    //public CameraMgr CameraMgr;
    //public EntityMgr EntityMgr;
    //public SelectionMgr SelectionMgr;

    // Update is called once per frame

    public bool gameEnding = false;
    public static Main inst;

    private bool awoken = false;
    private bool started = false;
    private void Awake()
    {
        inst = this;
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
        if (!gameEnding) { SelectionMgr.inst.Tick(dt); CameraMgr.inst.Tick(dt); }
        if (gameEnding) { CameraMgr.inst.MoveToCenter(dt); AlertSystem.inst.ClearSoundEvents(); }
        EntityMgr.inst.Tick(dt);
        UIMgr.inst.Tick(dt);

        ScoreGameOver.inst.Tick(dt);
    }
}
