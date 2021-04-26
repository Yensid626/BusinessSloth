using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public CameraMgr CameraMgr;
    public EntityMgr EntityMgr;
    public SelectionMgr SelectionMgr;

    // Update is called once per frame
    void Update()
    {
        Tick(Time.deltaTime);
    }

    internal void Tick(float dt)
    {
        EntityMgr.Tick(dt);
    }
}
