using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandType { Move, Follow, Intercept, Wait};

public class UnitAI : MonoBehaviour
{
    public List<Command> commands = new List<Command>();
    public bool followingCommand = false;
    public GameObject line;
    public bool patrol = false;
    // Start is called before the first frame update
    private void Awake()
    {
        addLine();
        //line.AddComponent<LineRenderer>();
        commands.Clear();
        commands.TrimExcess();
        Debug.Log("UnitAI Started. All commands Cleared");
    }

        void Start()
    {
    }

    // Update is called once per frame
    /*void Update()
    {
        count = commands.Count;
        Debug.Log(count);
    }*/

    internal void Tick(float dt)
    {
        if (commands.Count >= 1)
        {
            followingCommand = true;
            if (!commands[0].isDone())
            {
                //line.positionCount = (commands.Count * 2) + 1;
                commands[0].enabled = true;
            }
            else
            {
                //Debug.Log("Command Finished");
                commands[0].Stop();
                if (patrol) { AddCommand(new Command(commands[0].me, commands[0].command, commands[0].targetPos, commands[0].targetEntity)); }
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
            //Debug.Log("Command removed!");
        }
        AddCommand(c);
        //empty command list and set in new Command
    }

    internal void AddCommand(Command c)
    {
        commands.Insert(commands.Count,c);
        //Debug.Log("Command #" + commands.Count);
        //commands.Add(c);
        line.GetComponent<LineRenderer>().positionCount = (commands.Count*2) + 1;
        //Debug.Log("Command added!");
        //Add command to list
        //only add if player is holding left shift and right clicks
    }

    void addLine()
    {
        if (line == null) { line = new GameObject(); }
        line.AddComponent<LineRenderer>();
        line.transform.parent = transform;
        line.name = "LineObject-AI2";
        line.SetActive(false);
    }
}

public class Command
{
    internal CommandType command;
    Vector3 mousePos = Vector3.zero;
    internal GameObject targetEntity = null;
    internal GameObject me = null;
    internal Vector3 targetPos = Vector3.zero;
    internal Vector3 endPos = Vector3.zero;
    internal Vector3 startPos = Vector3.zero;
    internal bool waiting = false;
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
    public Command(GameObject entity381, CommandType c, Vector3 pos, GameObject obj)
    {
        this.me = entity381;
        this.command = c;
        if (pos != null)
        {
            this.mousePos = pos;
            this.targetPos = pos;
        }
        if (obj != null)
        {
            this.targetEntity = obj;
            this.targetPos = obj.transform.position;
        }
        Init();
    }

    public void Init()
    {
        waiting = false;
        finished = false;
        enabled = false;
        //line = new GameObject();
        //line = me.transform.GetComponent<UnitAI>().line.transform.GetComponent<LineRenderer>();
        //line.transform.parent = me.transform.parent.transform;
        //line.AddComponent<LineRenderer>();
        //line.positionCount = 3;
        startPos = me.transform.position;
        drawLine(me.transform.position, targetPos, "NA", 0);
        drawLine(targetPos, targetPos, "NA", 1);
        //Debug.Log("Command Initialized To: " + command);
    }

    public void Tick(float dt)
    {
        if (!finished && enabled)
        {
            //Debug.Log(command);
            //index = me.transform.GetComponent<UnitAI>().lineIndex;
            //line.gameObject.SetActive(me.GetComponent<Entity381>().isSelected);
            switch (command)
            {
                case CommandType.Move:
                    endPos = targetPos;
                    drawLine(startPos, targetPos, "Move", 0);
                    drawLine(targetPos, endPos, "Move", 1);
                    break;
                case CommandType.Wait:
                    mousePos.x -= dt; waiting = (mousePos.x > 0);
                    targetPos = startPos + new Vector3(0, waiting ? 1.0f : 0.0f, 0);
                    drawLine(startPos, targetPos, "Wait", 0);
                    drawLine(targetPos, endPos, "Wait", 1);
                    //Debug.Log(waiting + ": " + mousePos.x);
                    //Debug.Log(waiting);
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
            if (enabled && !waiting) { Move(); }
        }
        //isDone();
    }

    public bool isDone()
    {
        if (Utils.getDist(startPos, targetPos) <= 0.3 || finished)
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
        //Debug.Log("Command Stopped Doing: " + command);
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
        /*
        line.SetPosition(index, start);
        line.SetPosition(index+1, end);
        line.startWidth = 0.25f;
        line.endWidth = 0.25f;
        line.material = Resources.Load("LineMaterials/"+material, typeof(Material)) as Material;
        */
    }
}