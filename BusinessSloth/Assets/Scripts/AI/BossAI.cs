using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Events { Wait, WonderFront, WonderBack, GoToOffice };//, GoToPlayer,TalkToPlayer};
    public enum Location { Office, OutsideOffice, BackCenter, BackLeft, MiddleLeft, FrontLeft, FrontCenter, MiddleCenter, PlayerOffice, OutsidePlayerOffice};

    public Dictionary<int, Location> locations = new Dictionary<int, Location>();

    public Location location;
    public int currentLocation = 0;


    public static float minGameTimer = 90;
    //public float eventTimer = 2;

    private Entity381 entity;
    private UnitAI unitAI;

    void Start()
    {
        locations.Add(0, Location.Office);
        locations.Add(1, Location.OutsideOffice);
        locations.Add(2, Location.BackCenter);
        locations.Add(3, Location.BackLeft);
        locations.Add(4, Location.MiddleLeft);
        locations.Add(5, Location.FrontLeft);
        locations.Add(6, Location.OutsidePlayerOffice);
        locations.Add(7, Location.FrontCenter);
        locations.Add(8, Location.MiddleCenter);
        locations.Add(9, Location.BackCenter);
        locations.Add(10, Location.OutsideOffice);

        entity = gameObject.GetComponent<Entity381>();
        unitAI = gameObject.GetComponent<UnitAI>();
        unitAI.patrol = false;
    }

    // Update is called once per frame
    public void Tick(float dt)
    {
        minGameTimer -= dt;
        //eventTimer -= dt;
        if (minGameTimer > 0)
            {DoStuff(); return; }

        if ((!unitAI.followingCommand) && (currentLocation != 6))
            { RoutePath(6); SendAIToLocation(Location.PlayerOffice); return; }
        if (unitAI.followingCommand) { return; }


        if (!Main.inst.gameEnding)
        {
            Main.inst.gameEnding = true;
            GameObject screen = GameObject.Find("PointScreen").gameObject;
            if (screen != null)
            {
                float pointMultiplier = 1;
                screen.transform.Find("Menu").Find("PointsEarned").GetComponent<Text>().text = ("Points Earned: " + Score.inst.GetPoints().ToString());
                if (entity.suspicion <= 0) { screen.transform.Find("Menu").Find("BossMessage").GetComponent<Text>().text = "\"Great job today! After seeing you in action today, I sure am glad I hired you!\"\n- Boss"; pointMultiplier = 2f; }
                if (entity.suspicion <= -2) { pointMultiplier = 3f; }
                if (entity.suspicion > 0) { screen.transform.Find("Menu").Find("BossMessage").GetComponent<Text>().text = "\"Good job today. It looks like you worked most of the day!\"\n- Boss"; pointMultiplier = 1f; }
                if (entity.suspicion > 5) { screen.transform.Find("Menu").Find("BossMessage").GetComponent<Text>().text = "\"Okay job today. You might not want to slack off as much tomorrow though.\"\n- Boss"; pointMultiplier = 0.9f; }
                if (entity.suspicion > 10) { screen.transform.Find("Menu").Find("BossMessage").GetComponent<Text>().text = "\"Awful job today. You better slack off again tomorrow!\"\n- Boss"; pointMultiplier = 0.5f; }
                if (entity.suspicion >= 15) { screen.transform.Find("Menu").Find("BossMessage").GetComponent<Text>().text = "\"Did you even do any work today? Don't bother comming to work tomorrow.\"\n- Boss"; pointMultiplier = 0f; }
                screen.transform.Find("Menu").Find("BonusMultiplier").GetComponent<Text>().text = ("Point Multiplier: " + (pointMultiplier).ToString() + "x");
                screen.transform.Find("Menu").Find("TotalPoints").GetComponent<Text>().text = ("Total Points: " + (Score.inst.GetPoints() * pointMultiplier).ToString());

            }
            UIMgr.inst.gameOver();
        }
        
    }

    void DoStuff()
    {
        if (!unitAI.followingCommand)
        {
            int newlocation = Random.Range(1, 8);
            RoutePath(newlocation);
        }
    }

    void RoutePath(int newlocation)
    {
        int offset = currentLocation - newlocation;
        for (int i = currentLocation; i != newlocation; i = i)
        {
            if (currentLocation > newlocation) { i--; } else { i++; }
            locations.TryGetValue(i, out Location next);
            SendAIToLocation(next);
            location = next; currentLocation = i;
        }
    }

    void SendAIToLocation(Location nextLocation)
    {
        switch (nextLocation)
        {
            case Location.Office:               unitAI.AddCommand(new Command(entity.gameObject, CommandType.Move, new Vector3(14.33f, 0f, 16.85f))); break;
            case Location.OutsideOffice:        unitAI.AddCommand(new Command(entity.gameObject, CommandType.Move, new Vector3(19.1f, 0f, 16.9f))); break;
            case Location.BackCenter:           unitAI.AddCommand(new Command(entity.gameObject, CommandType.Move, new Vector3(19.1f, 0f, 9.4f))); break;
            case Location.BackLeft:             unitAI.AddCommand(new Command(entity.gameObject, CommandType.Move, new Vector3(19.1f, 0f, -11.7f))); break;
            case Location.MiddleLeft:           unitAI.AddCommand(new Command(entity.gameObject, CommandType.Move, new Vector3(11.6f, 0f, -12.5f))); break;
            case Location.FrontLeft:            unitAI.AddCommand(new Command(entity.gameObject, CommandType.Move, new Vector3(4.3f, 0f, -12.5f))); break;
            case Location.OutsidePlayerOffice:  unitAI.AddCommand(new Command(entity.gameObject, CommandType.Move, new Vector3(4.3f, 0f, -4.1f))); break;
            case Location.PlayerOffice:         unitAI.AddCommand(new Command(entity.gameObject, CommandType.Move, new Vector3(6.15f, 0f, -4.1f))); break;
            case Location.FrontCenter:          unitAI.AddCommand(new Command(entity.gameObject, CommandType.Move, new Vector3(4.3f, 0f, 9.4f))); break;
            case Location.MiddleCenter:         unitAI.AddCommand(new Command(entity.gameObject, CommandType.Move, new Vector3(11.7f, 0f, 9.4f))); break;
        }
    }
}

