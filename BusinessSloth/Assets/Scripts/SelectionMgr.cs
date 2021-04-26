using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMgr : MonoBehaviour
{
    public GameObject entityMgr;
    public GameObject cameraMgr;
    public GameObject selectedEntity;
    private GameObject tempEntity;
    //internal Vector3 movePosition;
    internal Vector3 mousePosition;
    //public LayerMask selectableLayer;
    //internal List<GameObject> entities;
    //public int selectedEntityIndex = 0;
    public float timer;

    //GameObject hitObject;

    // Start is called before the first frame update
    void Start()
    {
        //entities = entityMgr.GetComponent<EntityMgr>().entitiesPeople;
        //SelectEntity(false);
        timer = 0;
    }

    // Update is called once per frame
    /*void Update()
    { }*/

    internal void Tick(float dt)
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        if ((Input.GetMouseButtonDown(0)))
        {
            if ((selectedEntity = GetMouseOver()) != null)
                { selectedEntity.GetComponent<Points>().AddPoints(); }
        }
    }
    /*void ProcessInput()
    {

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (Input.GetKey(KeyCode.LeftShift)) { SelectNextEntity(true); }
            else { SelectNextEntity(); }
        }

        if (Input.GetMouseButtonDown(0))
        {
            tempEntity = GetMouseEntity();
            if (tempEntity != null)
            {
                selectedEntity = tempEntity.GetComponent<Entity381>();
                selectedEntityIndex = entities.IndexOf(selectedEntity);
                SelectEntity();
            }
        }
    }

    void SelectNextEntity()
    { SelectNextEntity(false); }
    void SelectNextEntity(bool keepPrevious)
    {
        selectedEntityIndex = (selectedEntityIndex >= entities.Count - 1 ? 0 : selectedEntityIndex + 1);
        SelectEntity(keepPrevious);
    }

    void SelectEntity()
    { SelectEntity(false); }
    void SelectEntity(bool keepPrevious)
    {
        if (!keepPrevious) { UnselectAll(); }
        if (selectedEntityIndex >= 0 && selectedEntityIndex < entities.Count)
        {
            selectedEntity = entities[selectedEntityIndex];
            selectedEntity.gameObject.GetComponent<Entity381>().isSelected = true;
            cameraMgr.GetComponent<CameraMgr>().FPChar = selectedEntity.gameObject;
        }
    }

    void UnselectAll()
    {
        foreach (Entity381 phx in entities)
        {
            phx.isSelected = false;
        }
    }

    public int IndexEntity(Entity381 ent)
    { return entities.IndexOf(selectedEntity); } */

    public GameObject GetMouseEntity()
    {
        mousePosition = GetMousePos();
        GameObject entity = null;
        if (mousePosition != Vector3.positiveInfinity)
        {
            float shortestDist = 5.5f;
            float dist;
            foreach (Entity381 ent in entityMgr.GetComponent<EntityMgr>().entitiesPeople)
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
            foreach (GameObject ent in entityMgr.GetComponent<EntityMgr>().entitiesObjects)
            {
                if (ent == hitData.transform.gameObject) { return hitData.transform.gameObject; }
            }
        }
        return null;
    }
}
