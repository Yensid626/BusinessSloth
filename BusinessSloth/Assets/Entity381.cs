﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity381 : MonoBehaviour
{
    public float deltaDesiredSpeed = 5; //How much the desired speed is changed each key press
    public float deltaDesiredHeading = 2.5f; //How much the desired heading is changed each key press
    public float acceleration = 1;
    public float turnSpeed = 1;
    public float maxSpeed = 5;
    public float minSpeed = -2;
    public float speed;
    public float desiredSpeed;
    public float heading;
    public float desiredHeading;
    public bool isSelected = false;
    public OrientedPhysics OrientedPhysics381;
    public UnitAI unitAI;

    public GameObject decorations;

    public bool showLine = false;
    internal GameObject line;
    // Start is called before the first frame update
    private void Awake()
    {
        OrientedPhysics381 = transform.GetComponent<OrientedPhysics>();
        if (!(OrientedPhysics381 != null))
        {
            OrientedPhysics381 = transform.gameObject.AddComponent<OrientedPhysics>();
        }
        unitAI = transform.GetComponent<UnitAI>();
        if (!(unitAI != null))
        {
            unitAI = transform.gameObject.AddComponent<UnitAI>();
        }
    }
    void Start()
    {
        addLine();
        addPatrol();
    }

    // Update is called once per frame
    /*void Update()
    { }*/

    internal void Tick(float dt)
    {
        if (showLine)
        {
            //line.transform.GetComponent<LineRenderer>().positionCount = 2;
            //drawLine(transform.position, transform.position + Utils.getPositionFromAngle(heading, speed * 2), "Display", 0);
            //line.SetActive(isSelected);
        }
        //decorations.SetActive(isSelected);
        OrientedPhysics381.Tick(dt);
        unitAI.Tick(dt);
        //transform.localPosition += transform.TransformDirection(Vector3.forward * dt * 1.2f);
    }

    Vector3 GetMousePos()
    {
        Plane plane = new Plane(Vector3.up, 0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //float distance;
        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.positiveInfinity;
    }

    public GameObject ClickEntity()
    {
        Vector3 mousePosition = GetMousePos();
        GameObject entity = null;
        if (mousePosition != Vector3.positiveInfinity)
        {
            float shortestDist = 5.5f;
            float dist;
            foreach (Transform child in this.transform.parent.transform)
            {
                dist = Utils.getDist(mousePosition, child.position);
                //dist = Vector3.Distance(mousePosition, ent.transform.position);
                if (dist < shortestDist)
                {
                    shortestDist = dist;
                    entity = child.gameObject;
                }
            }
        }
        return entity;
    }

    void drawLine(Vector3 start, Vector3 end, string material, int index)
    {
        line.transform.GetComponent<LineRenderer>().SetPosition(index, start);
        line.transform.GetComponent<LineRenderer>().SetPosition(index + 1, end);
        line.transform.GetComponent<LineRenderer>().startWidth = 0.25f;
        line.transform.GetComponent<LineRenderer>().endWidth = 0.25f;
        line.transform.GetComponent<LineRenderer>().material = Resources.Load("LineMaterials/" + material, typeof(Material)) as Material;
    }

    void addLine()
    {
        line = new GameObject();
        line.AddComponent<LineRenderer>();
        line.transform.parent = transform;
        line.name = "LineObject";
        line.SetActive(false);
    }

    void addPatrol()
    {
        unitAI.patrol = true;
        //unitAI.SetCommand(new Command(gameObject, CommandType.Move, transform.position));
        unitAI.SetCommand(new Command(gameObject, CommandType.Move, new Vector3(4.07f, 0.0f, -11.54f)));
        unitAI.AddCommand(new Command(gameObject, CommandType.Move, new Vector3(4.07f, 0.0f, -1.48f)));
        unitAI.AddCommand(new Command(gameObject, CommandType.Move, new Vector3(4.07f, 0.0f, -6.26f)));
        unitAI.AddCommand(new Command(gameObject, CommandType.Move, new Vector3(4.07f, 0.0f, 5.17f)));
    }
}
