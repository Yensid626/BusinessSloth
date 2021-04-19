using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : MonoBehaviour
{
    public GameObject cameraMgr;
    public GameObject entities;
    public GameObject selectionMgr;
    internal List<Entity381> entitiesPhysics = new List<Entity381>();

    // Start is called before the first frame update
    void Start()
    {
        GetMovableEntities();
    }
    //public List<GameObject> CubeEntities;

    // Update is called once per frame
    /*void Update()
    { }*/

    internal void Tick(float dt)
    {
        ProcessInput();
        TickEntities(dt);
        TickCamera(dt);
        selectionMgr.GetComponent<SelectionMgr>().Tick(dt);
    }

    void ProcessInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            print("Quit!");
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; //exit editot
            #else
                  Application.Quit();
            #endif
        }

    }

    void TickEntities(float dt)
    {
        foreach (Entity381 ent in entitiesPhysics)
        {
            ent.Tick(dt);
        }
    }

    void GetMovableEntities()
    {
        entitiesPhysics.Clear();
        entitiesPhysics.TrimExcess();
        foreach (Transform child in entities.transform)
        {
            //print("For each child: " + child.name);
            //print("    and parent: " + child.parent.gameObject.GetComponents<ShipPhysics>());
            if (child.gameObject.GetComponents<Entity381>() != null)
            {
                //print("   Found Component [Ship]");
                entitiesPhysics.Add(child.gameObject.GetComponent<Entity381>());
            }
            else
            {
                //print("   Missing Component [Ship]");
            }
            //entitiesPhysics.Add(child.gameObject.GetComponent<PhysicsAndInput>());
            //entitiesPhysics.Add(child.GetComponent<PhysicsAndInput>());
        }
    }

    void TickCamera(float dt)
    {
        cameraMgr.GetComponent<CameraMgr>().Tick(dt);
    }
}
