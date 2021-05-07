using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : MonoBehaviour
{
    public GameObject people;
    public GameObject cubicle;
    //public GameObject cameraMgr;
    //public GameObject selectionMgr;
    public List<Entity381> entitiesPeople = new List<Entity381>();
    public List<GameObject> entitiesObjects = new List<GameObject>();
    public static EntityMgr inst;

    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GetMovablePeople();
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
        SelectionMgr.inst.Tick(dt);
        //etselectionMgr.GetComponent<SelectionMgr>().Tick(dt);
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
        foreach (Entity381 ent in entitiesPeople)
        {
            ent.Tick(dt);
        }
    }

    void GetMovablePeople()
    {
        entitiesPeople.Clear();
        entitiesPeople.TrimExcess();
        foreach (Transform child in people.transform)
        {
            //print("For each child: " + child.name);
            //print("    and parent: " + child.parent.gameObject.GetComponents<ShipPhysics>());
            if (child.gameObject.GetComponents<Entity381>() != null)
            {
                //print("   Found Component [Ship]");
                entitiesPeople.Add(child.gameObject.GetComponent<Entity381>());
            }
            else
            {
                //print("   Missing Component [Ship]");
            }
            //entitiesPhysics.Add(child.gameObject.GetComponent<PhysicsAndInput>());
            //entitiesPhysics.Add(child.GetComponent<PhysicsAndInput>());
        }
    }

    void GetMovableEntities()
    {
        entitiesObjects.Clear();
        entitiesObjects.TrimExcess();
        foreach (Transform child in cubicle.transform.Find("Decorations").transform)
        {
            //print("   Found Component [Ship]");
            entitiesObjects.Add(child.gameObject);
            if (child.gameObject.GetComponent<BoxCollider>() == null) { child.gameObject.AddComponent<BoxCollider>(); }
        }
    }
}
