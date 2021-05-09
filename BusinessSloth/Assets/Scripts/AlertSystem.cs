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

public enum EventType { RightEdge, LeftEdge, BottomEdge, TopEdge, FullScreen };
public class AlertEvent
{
    internal EventType type = EventType.FullScreen;
    internal Color color = Color.red;
    //internal bool noTime = false;
    internal float timer = 0;
    internal float fadeIn = 0.1f;
    private float fadeInTimer;
    internal float fadeOut = 0.3f;
    private float fadeOutTimer;
    internal bool persist = false; //Overrides fadeOut and timer
    internal int priority = 1;
    //private bool finished = false;
    private static int highestPriority = 1;

    private GameObject alert = null;
    private RectTransform rect = null;
    private Image img = null;

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
        color = new Color(color.r, color.g, color.b, 1);
        alert = new GameObject("AlertEvent-Priority:" + priority);
        alert.SetActive(false);
        alert.transform.parent = AlertSystem.inst.HUD.transform;

        rect = alert.AddComponent<RectTransform>();
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchorMax = Vector2.one;
        rect.anchorMin = Vector2.zero;
        rect.localScale = Vector3.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        img = alert.AddComponent<Image>();
        img.sprite = Resources.Load("UIElements/WhiteScreen", typeof(Sprite)) as Sprite;
        switch (type)
        {
            case EventType.RightEdge:
                img.sprite = Resources.Load("UIElements/WhiteEdge", typeof(Sprite)) as Sprite;
                rect.localScale = new Vector3(-1, 1, 1);
                break;
            case EventType.LeftEdge:
                img.sprite = Resources.Load("UIElements/WhiteEdge", typeof(Sprite)) as Sprite;
                break;
            case EventType.BottomEdge:
                img.sprite = Resources.Load("UIElements/WhiteCenter", typeof(Sprite)) as Sprite;
                break;
            case EventType.TopEdge:
                img.sprite = Resources.Load("UIElements/WhiteCenter", typeof(Sprite)) as Sprite;
                rect.localScale = new Vector3(1, -1, 1);
                break;
            case EventType.FullScreen:
                img.sprite = Resources.Load("UIElements/WhiteScreen", typeof(Sprite)) as Sprite;
                break;
        }
        img.color = color;
        //Debug.Log("------------\n" + img.color + "\n" + color);
        timer += (fadeIn + fadeOut);
        fadeInTimer = timer - fadeIn;
        fadeOutTimer = fadeOut;
    }
    public void Tick(float dt)
    {
         timer -= dt; 

        if ((priority < highestPriority) || (alert == null)) { alert.SetActive(false); return; } //dont run if there are higher priority event runnning
        //if ((priority >= highestPriority) && (alert != null)) { alert.transform.SetAsLastSibling(); }

        Fade();
        UpdateHighestPriority();
    }

    void UpdateHighestPriority()
    {
        highestPriority = (highestPriority <= priority ? priority : highestPriority);
        alert.SetActive(true);
    }

    public void Stop()
    {
        if (priority >= highestPriority) { highestPriority = 0; }
        Object.Destroy(alert);
    }

    public bool IsDone()
    {
        return ((timer <= 0) && (!persist));
    }

    void Fade()
    {
        if (timer >= fadeInTimer)
            { img.color = new Color(color.r, color.g, color.b, 1 - ((timer-fadeInTimer) / fadeIn));}
            else { img.color = color; }

        if ((timer <= fadeOutTimer) && !persist)
        {
            if (timer > 0)
            { img.color = new Color(color.r, color.g, color.b, (timer / fadeOut));}
            else { img.color = new Color(color.r, color.g, color.b, 0); }
        }
        //alert.SetActive(true);
    }
}