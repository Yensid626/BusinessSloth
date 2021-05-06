using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVision : MonoBehaviour
{
    public bool collectSuspision = true;
    public bool collectGossip = false;
    public float gossipRange = 20;
    internal float FOV = 70;
    internal float farClipping = 12;
    internal float nearClipping = 0.2f;
    internal bool visible = false;
    internal List<LineRenderer> lines = new List<LineRenderer>();
    float cosAng;
    float sinAng;
    float tanAng;
    float renderedFOV;
    internal float timer;
    public GameObject objectsToFind;
    public List<GameObject> players = new List<GameObject>();
    Transform p;
    Vector3 offset;
    GameObject folder;

    private Entity381 entity;

    // Start is called before the first frame update
    void Start()
    {
        entity = gameObject.GetComponent<Entity381>();
        //transform.Find("originMarker").gameObject.SetActive(false);
        p = transform;
        folder = new GameObject("FOVLines");
        folder.transform.parent = transform;
        folder.transform.localPosition = new Vector3(0, 1.93f, 0);
        Debug.Log(p.position);
        Debug.Log(folder.transform.position);
        Debug.Log(folder.transform.localPosition);
        if (transform.Find("FOV") != null)
        { folder.transform.position = transform.Find("FOV").transform.position; }
        //folder.transform.localScale = new Vector3(1, 1, 1);
        for (int i = 0; i < 12; i++)
        {
            GameObject obj = new GameObject("FOVLine:" + i);
            obj.transform.parent = folder.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.position = new Vector3(0, 0, 0);
            obj.transform.localScale = new Vector3(1, 1, 1);
            LineRenderer line = obj.AddComponent<LineRenderer>();
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;
            line.material = Resources.Load("LineMaterials/Move", typeof(Material)) as Material;
            lines.Add(line);
        } //add 4 lines for farClip, 4 for near Clip, and 4 for edges
        renderedFOV = FOV + 1;

        if (objectsToFind == null) { players.Clear(); players.TrimExcess(); players.Add(CameraMgr.inst.FPRig.gameObject); }
        else
        {
            foreach (Transform child in objectsToFind.transform)
            {
                if (child.transform.gameObject != gameObject) { players.Add(child.transform.gameObject); }
            }
        }
        updateFOV();
    }

    internal void Tick(float dt)
    {
        updateFOV();
        if (timer <= 0)
        {
            detectPlayer();
            timer = Random.Range(0.4f, 1.6f);
        }
        timer -= dt;
    }

    void updateFOV()
    {
        if (renderedFOV != FOV) //If FOV settings were changed (not AI position, only FOV)
        {
            renderedFOV = FOV;
            cosAng = Utils.Cos(90 - FOV); //using Mathf instead of Utils to get radians returned
            sinAng = Utils.Sin(90 - FOV);
            tanAng = Utils.Tan(90 - FOV);
            folder.SetActive(visible);
        }

        offset = folder.transform.position;
        lines[0].SetPosition(0, offset + p.TransformVector(new Vector3(cosAng * nearClipping, tanAng * nearClipping, sinAng * nearClipping))); lines[0].SetPosition(1, offset + p.TransformVector(new Vector3(cosAng * farClipping, tanAng * farClipping, sinAng * farClipping)));
        lines[1].SetPosition(0, offset + p.TransformVector(new Vector3(cosAng * nearClipping, -tanAng * nearClipping, sinAng * nearClipping))); lines[1].SetPosition(1, offset + p.TransformVector(new Vector3(cosAng * farClipping, -tanAng * farClipping, sinAng * farClipping)));
        lines[2].SetPosition(0, offset + p.TransformVector(new Vector3(-cosAng * nearClipping, -tanAng * nearClipping, sinAng * nearClipping))); lines[2].SetPosition(1, offset + p.TransformVector(new Vector3(-cosAng * farClipping, -tanAng * farClipping, sinAng * farClipping)));
        lines[3].SetPosition(0, offset + p.TransformVector(new Vector3(-cosAng * nearClipping, tanAng * nearClipping, sinAng * nearClipping))); lines[3].SetPosition(1, offset + p.TransformVector(new Vector3(-cosAng * farClipping, tanAng * farClipping, sinAng * farClipping)));

        for (int clip = 0; clip < 4; clip++) //set edsge lines
        {
            lines[(clip * 2) + 4].SetPosition(0, lines[clip].GetPosition(0)); lines[(clip * 2) + 4].SetPosition(1, lines[(clip + 1) % 4].GetPosition(0)); //set near clipping plane
            lines[(clip * 2) + 5].SetPosition(0, lines[clip].GetPosition(1)); lines[(clip * 2) + 5].SetPosition(1, lines[(clip + 1) % 4].GetPosition(1)); //set far clipping plane
        }
    }

    Ray ray;
    RaycastHit hitData;
    void detectPlayer()
    {
        for(int i = 0; i < players.Count; i++)
        {
            GameObject player = castRayToObject(players[i]);
            if (player != null)
            {
                if (collectSuspision)
                {
                    GameObject selectedObject = SelectionMgr.inst.selectedEntity;
                    if (selectedObject == null) { entity.suspision = Utils.Clamp(entity.suspision += 0.5f, -1, 10); return; }
                    //entity.suspision += selectedObject.GetComponent<Points>().points;
                    entity.suspision = Utils.Clamp(entity.suspision += selectedObject.GetComponent<Points>().points, -1, 10);
                    //Debug.Log("Ha Found you Slacking Off");
                }
                if (collectGossip)
                {
                    if ((player.GetComponent<Entity381>() != null) && (Utils.getDist(entity.gameObject.transform.position, player.transform.position) <= gossipRange))
                    {
                        Debug.Log("Yummy Gossip!");
                        entity.pause = true;
                        player.GetComponent<Entity381>().pause = true;
                        entity.suspision += player.GetComponent<Entity381>().suspision;
                        player.GetComponent<Entity381>().suspision = 0;
                        Utils.Sleep(1.5f);
                        entity.pause = false;
                        player.GetComponent<Entity381>().pause = false;
                    }
                    else
                    {
                        Debug.Log("Gossip Failed");
                    }
                }

            }
        }
    }

    internal GameObject castRayToObject(GameObject obj) // ####Folder Pos is this entitys eyesight!####
    {
        //Plane plane = new Plane(Vector3.up, 0);
        ray = new Ray(folder.transform.position, obj.transform.position - folder.transform.position);
        //ray = new Ray(folder.transform.position, CameraMgr.inst.position - folder.transform.position);
        LineRenderer l;
        if ((l = entity.gameObject.transform.Find("LineObject").gameObject.GetComponent<LineRenderer>()) != null)
        {
            //if (Mathf.Abs(Utils.Atan(-CameraMgr.inst.position.x + folder.transform.position.x, -CameraMgr.inst.position.z + folder.transform.position.z)) <= FOV/2)
            //{ l.material = Resources.Load("LineMaterials/Follow", typeof(Material)) as Material; }
            //else
            //{ l.material = Resources.Load("LineMaterials/Intercept", typeof(Material)) as Material; }
            l.material = Resources.Load("LineMaterials/Intercept", typeof(Material)) as Material;
            l.SetPosition(0, folder.transform.position);
            l.SetPosition(1, folder.transform.position + (obj.transform.position - folder.transform.position));
        }
        //float distance;
        //if (plane.Raycast(ray, out float distance))
        if (Physics.Raycast(ray, out hitData, 50))
        {
            if (hitData.transform.gameObject == obj) { return hitData.transform.gameObject; }
            if (hitData.transform.gameObject.name == "Main Camera") { return hitData.transform.gameObject; }
            if (hitData.transform.gameObject.GetComponent<Entity381>() != null) { return hitData.transform.gameObject; }
        }
        return null;
    }
}
