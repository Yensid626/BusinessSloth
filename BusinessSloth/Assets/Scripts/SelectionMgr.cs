using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMgr : MonoBehaviour
{
    //public GameObject entityMgr;
    //public GameObject cameraMgr;
    public GameObject selectedEntity;
    private GameObject tempEntity;
    //internal Vector3 movePosition;
    internal Vector3 mousePosition;
    //public LayerMask selectableLayer;
    //internal List<GameObject> entities;
    //public int selectedEntityIndex = 0;
    public float timer;
    public float multiplier;

    public static SelectionMgr inst;
    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //entities = entityMgr.GetComponent<EntityMgr>().entitiesPeople;
        //SelectEntity(false);
        timer = 0;
        multiplier = 1;
    }

    // Update is called once per frame
    /*void Update()
    { }*/

    internal void Tick(float dt)
    {
        ProcessInput(dt);
    }

    void ProcessInput(float dt)
    {
        if (!Input.GetMouseButton(0)) { multiplier = 1; }
        if ((Input.GetMouseButton(0)) && timer >= 0.3)
        {
            timer = 0;
            multiplier += dt;
            tempEntity = selectedEntity;
            if (tempEntity != (selectedEntity = GetMouseOver()))
                { multiplier = 1; }
            if (selectedEntity != null)
                { selectedEntity.GetComponent<Points>().AwardPoints(multiplier); }
        }
        if (Input.GetMouseButtonUp(0)) { timer += 1;}
        timer += dt;
    }

    public GameObject GetMouseEntity()
    {
        mousePosition = GetMousePos();
        GameObject entity = null;
        if (mousePosition != Vector3.positiveInfinity)
        {
            float shortestDist = 5.5f;
            float dist;
            foreach (Entity381 ent in EntityMgr.inst.entitiesPeople)
            {
                dist = Utils.getDist(mousePosition, ent.transform.position);
                //dist = Vector3.Distance(mousePosition, ent.transform.position);
                if (dist < shortestDist)
                {
                    shortestDist = dist;
                    entity = ent.gameObject;
                }
            }
        }
        return entity;
    }

    /*void CastRay()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitData, 1000))
        {
            hitObject = hitData.transform.gameObject;
            print("Object Found");
            print("   " + hitObject.name);
            //UnselectAll();
            //selectedEntity = hitObject.GetComponent<Entity381>();
            //selectedEntity.isSelected = true;
            //cameraMgr.GetComponent<CameraMgr>().FPChar = hitObject;
        }
        else
        {
            Plane plane = new Plane(Vector3.up, 0);
            //float distance;
            hitObject = null;
            print("Nothing Hit");
            if (plane.Raycast(ray, out float distance))
            {
                movePosition = ray.GetPoint(distance);
            }
        }
    }*/

    Ray ray;
    RaycastHit hitData;
    Vector3 GetMousePos()
    {
        //Plane plane = new Plane(Vector3.up, 0);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //float distance;
        //if (plane.Raycast(ray, out float distance))
        if (Physics.Raycast(ray, out hitData, 1000))
        {
            return hitData.point;
        }
        return Vector3.positiveInfinity;
    }

    GameObject GetMouseOver()
    {
        //Plane plane = new Plane(Vector3.up, 0);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //float distance;
        //if (plane.Raycast(ray, out float distance))
        if (Physics.Raycast(ray, out hitData, 1000))
        {
            //Debug.Log(hitData.transform.gameObject.name);
            foreach (GameObject ent in EntityMgr.inst.entitiesObjects)
            {
                if (ent == hitData.transform.gameObject) { return hitData.transform.gameObject; }
            }
        }
        return null;
    }
}
