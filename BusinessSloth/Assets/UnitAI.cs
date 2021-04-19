﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandType { Move, Follow, Intercept };

public class UnitAI : MonoBehaviour
{
    public List<Command> commands = new List<Command>();
    internal bool followingCommand = false;
    internal GameObject line;
    public bool patrol = false;
    // Start is called before the first frame update
    void Start()
    {
        commands.Clear();
        commands.TrimExcess();
        line = new GameObject();
        line.AddComponent<LineRenderer>();
        line.transform.parent = transform;
        line.name = "LineObject";
        line.SetActive(false);
    }

    // Update is called once per frame
    /*void Update()
    { }*/

    internal void Tick(float dt)
    {
        if (commands.Count >= 1)
        {
            //print("running command");
            followingCommand = true;
            if (!commands[0].isDone())
            {
                //line.positionCount = (commands.Count * 2) + 1;
                commands[0].enabled = true;
                //commands[0].Tick(dt);
            }
            else
            {
                commands[0].Stop();
                if (patrol) { AddCommand(commands[0]); }
                commands.RemoveAt(0);
            }
            int i = 0;
            for(i = 0; i <= commands.Count - 1; i++)
            {
                if (i < 1)
                    { commands[i].startPos = commands[i].me.transform.position; }
                else
                    { commands[i].startPos = commands[i - 1].endPos; }
                commands[i].Tick(dt);
            }
        }
        else { followingCommand = false; }
    }

    internal void SetCommand(Command c)
    {
        //for (var l = 0; l < commands.Count; l++)
        while (commands.Count >= 1)
        {
            commands[0].Stop();
            commands.RemoveAt(0);
            commands.TrimExcess();
        }
        //print("Commands removed!");
        AddCommand(c);
        //empty command list and set in new Command
    }

    internal void AddCommand(Command c)
    {
        commands.Add(c);
        line.GetComponent<LineRenderer>().positionCount = commands.Count + 2;
        //print("Command added!");
        //Add command to list
        //only add if player is holding left shift and right clicks
    }
}

public class Command
{
    CommandType command;
    Vector3 mousePos = Vector3.zero;
    GameObject targetEntity = null;
    internal GameObject me = null;
    internal Vector3 targetPos = Vector3.zero;
    internal Vector3 endPos = Vector3.zero;
    internal Vector3 startPos = Vector3.zero;
    LineRenderer line;
 //   internal int index;
    bool finished = false;
    internal bool enabled = false;
    /*public Command(GameObject entity381, CommandType c) //I dont know why I put this here.  A command like this should end instantly...
    {
        this.me = entity381;
        this.command = c;
    }*/
    public Command(GameObject entity381, CommandType c, Vector3 pos)
    {
        this.me = entity381;
        this.command = c;
        this.mousePos = pos;
        this.targetPos = pos;
        Init();
    }
    public Command(GameObject entity381, CommandType c, GameObject obj)
    {
        this.me = entity381;
        this.command = c;
        this.targetEntity = obj;
        this.targetPos = obj.transform.position;
        Init();
    }

    public void Init()
    {
        finished = false;
        enabled = false;
        //line = new GameObject();
        line = me.transform.GetComponent<UnitAI>().line.transform.GetComponent<LineRenderer>();
        //line.transform.parent = me.transform.parent.transform;
        //line.AddComponent<LineRenderer>();
        //line.positionCount = 3;
        startPos = me.transform.position;
        drawLine(me.transform.position, targetPos, "NA", 0);
        drawLine(targetPos, targetPos, "NA", 1);
    }

    public void Tick(float dt)
    {
        if (!finished)
        {
            //index = me.transform.GetComponent<UnitAI>().lineIndex;
            //line.gameObject.SetActive(me.GetComponent<Entity381>().isSelected);
            switch (command)
            {
                case CommandType.Move:
                    endPos = targetPos;
                    drawLine(startPos, targetPos, "Move", 0);
                    drawLine(targetPos, endPos, "Move", 1);
                    break;
                case CommandType.Follow:
                    endPos = targetEntity.transform.position;
                    targetPos = targetEntity.transform.TransformPoint(Vector3.right * 10);
                    drawLine(startPos, targetPos, "Follow", 0);
                    drawLine(targetPos, endPos, "Follow", 1);
                    break;
                case CommandType.Intercept:
                    endPos = targetEntity.transform.position;
                    Vector3 travel = Utils.getPositionFromAngle(targetEntity.GetComponent<Entity381>().heading, targetEntity.GetComponent<Entity381>().speed);
                    float time = Utils.getDist(startPos, targetPos) / me.GetComponent<Entity381>().speed;
                    time = Utils.Clamp(time, 0, 100);
                    targetPos = targetEntity.transform.position + (travel * time);
                    //Vector3 travel2 = Utils.getPositionFromAngle(me.GetComponent<Entity381>().heading, me.GetComponent<Entity381>().speed);
                    //targetPos = Utils.get2DIntercept(targetEntity.transform.position, targetEntity.transform.position + travel, me.transform.position, me.transform.position + travel2);
                    //targetPos = targetEntity.transform.TransformPoint(Vector3.forward * 2);
                    drawLine(startPos, targetPos, "Intercept", 0);
                    drawLine(targetPos, endPos, "Intercept", 1);
                    break;
            }
            if (enabled) { Move(); }
        }
        //isDone();
    }

    public bool isDone()
    {
        if (Utils.getDist(startPos, targetPos) <= 0.25 || finished)
        {
            Stop();
            return true;
        }
        return false;
    }

    public void Stop()
    {
        finished = true;
        me.GetComponent<Entity381>().desiredHeading = Utils.RoundToNearest(me.GetComponent<Entity381>().heading, me.GetComponent<Entity381>().deltaDesiredHeading);
        me.GetComponent<Entity381>().desiredSpeed = 0;
        //UnityEngine.Object.DestroyObject(line, 0);
    }

    void Move()
    {
        Vector3 positionOffset = startPos - targetPos;
        //Debug.Log(positionOffset);
        //Debug.Log("  From:" + me.transform.position);
        //Debug.Log("  To:" + targetPos);
        positionOffset = Vector3.Normalize(positionOffset);
        Entity381 ent = me.GetComponent<Entity381>();
        ent.desiredHeading = Utils.Atan(-positionOffset.x, -positionOffset.z);
        float speedMultiplier = Utils.differenceDegrees(ent.desiredHeading, ent.heading);
        speedMultiplier = (speedMultiplier < 0.5f ? 0 : Utils.exponentialMap(0.5f, 1, 0, 1, speedMultiplier));
        ent.desiredSpeed = ent.maxSpeed * speedMultiplier;
    }

    void drawLine(Vector3 start, Vector3 end, string material, int index)
    {
        line.SetPosition(index, start);
        line.SetPosition(index+1, end);
        line.startWidth = 0.25f;
        line.endWidth = 0.25f;
        line.material = Resources.Load("LineMaterials/"+material, typeof(Material)) as Material;
    }
}