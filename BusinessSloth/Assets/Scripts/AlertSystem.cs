using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AlertSystem : MonoBehaviour
{

    public GameObject HUD;
    public static AlertSystem inst;
    public List<AlertEvent> events = new List<AlertEvent>();

    private void Awake()
    {
        inst = this;
        HUD = GameObject.Find("HUD");
        if (HUD != null) { Debug.Log("Found It!"); }
        ClearSoundEvents();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void Tick(float dt)
    {
        UpdateEvents(dt);
    }

    void UpdateEvents(float dt)
    {
        for (int i = 0; i < events.Count; i++)
        {
            events[i].Tick(dt);
            if (events[i].IsDone()) { events[i].Stop(); events.RemoveAt(i); i--; }
        }
    }

    public void CreateSoundEvent(EventType type, int priority, Color color, float timer)
    { events.Add(new AlertEvent(type, priority, color, timer)); }
    public void CreateSoundEvent(EventType type, int priority, Color color, float timer, float fadeIn, float fadeOut)
    { events.Add(new AlertEvent(type, priority, color, timer, fadeIn, fadeOut)); }
    public void CreateSoundEvent(EventType type, int priority, Color color, float fadeIn, bool persistAfterTimer)
    { events.Add(new AlertEvent(type, priority, color, fadeIn, persistAfterTimer)); }

    public void ClearSoundEvents()
    {
        while (events.Count > 0) { events[0].Stop(); events.RemoveAt(0); }
        events.Clear(); events.TrimExcess();
    }
}

public enum EventType { RightEdge, LeftEdge, FullScreen };
public class AlertEvent
{
    internal EventType type = EventType.FullScreen;
    internal Color color = Color.red;
    //internal bool noTime = false;
    internal float timer = 0;
    internal float fadeIn = 0.2f;
    private float fadeInTimer;
    internal float fadeOut = 0.2f;
    private float fadeOutTimer;
    internal bool persist = false; //Overrides fadeOut and timer
    internal int priority = 1;
    //private bool finished = false;
    private static int highestPriority = 1;

    private Image alert = null;

    //Class Initializers
    public AlertEvent(EventType eventType, int eventPriority, Color eventColor, float eventTimer)
    {
        type = eventType; priority = eventPriority; color = eventColor; timer = eventTimer;
        Init();
    }
    public AlertEvent(EventType eventType, int eventPriority, Color eventColor, float eventTimer, float eventFadeIn, float eventFadeOut)
    {
        type = eventType; priority = eventPriority; color = eventColor; timer = eventTimer; fadeIn = eventFadeIn; fadeOut = eventFadeOut;
        Init();
    }
    public AlertEvent(EventType eventType, int eventPriority, Color eventColor, float eventFadeIn, bool persistAfterTimer)
    {
        type = eventType; priority = eventPriority; color = eventColor; fadeIn = eventFadeIn; persist = persistAfterTimer;
        Init();
    }

    //Class Methods
    void Init()
    {
        switch (type)
        {
            case EventType.RightEdge: alert = AlertSystem.inst.HUD.transform.Find("RightEdgeAlert").GetComponent<Image>(); break;
            case EventType.LeftEdge: alert = AlertSystem.inst.HUD.transform.Find("LeftEdgeAlert").GetComponent<Image>(); break;
            case EventType.FullScreen: alert = AlertSystem.inst.HUD.transform.Find("Alert").GetComponent<Image>(); break;
        }
        fadeInTimer = fadeIn; fadeOutTimer = fadeOut;
    }
    public void Tick(float dt)
    {

        if ((priority < highestPriority) || (alert == null)) { return; } //dont run if there are higher priority event runnning
        UpdateHighestPriority();

        timer -= dt;
        Fade(dt);
    }

    void UpdateHighestPriority()
    {
        highestPriority = (highestPriority <= priority ? priority : highestPriority);
    }

    public void Stop()
    {
        if (priority >= highestPriority) { highestPriority = 0; }
    }

    public bool IsDone()
    {
        return ((fadeInTimer <= 0) && (timer <= 0) && (!persist) && (fadeOutTimer <= 0));
    }

    void Fade(float dt)
    {
        if (fadeInTimer >= 0)
            { alert.color = new Color(color.r, color.g, color.b, 1 - (fadeInTimer / fadeIn)); fadeInTimer -= dt; }
            else { alert.color = color; }

        if (timer <= 0 && !persist)
        {
            if (fadeOutTimer >= 0)
                { alert.color = new Color(color.r, color.g, color.b, (fadeOutTimer / fadeOut)); fadeOutTimer -= dt; }
                else { alert.color = new Color(color.r, color.g, color.b, 0); }
        }
    }
}