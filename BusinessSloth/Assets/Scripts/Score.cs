using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public float scoreAmount;
    public GameOverScreen GameOver;
    public List<PointDisplay> pointDisplays = new List<PointDisplay>();
    public int count;
    //public float pointsPerSecond;

    public static Score inst;
    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        scoreAmount = 0f;
        //pointsPerSecond = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Points: " + (int)scoreAmount;
        //Debug.Log(pointDisplays.Count);
        int i = 0;
        for (i = 0; i <= pointDisplays.Count - 1; i++)
        {
            //Debug.Log(pointDisplays[i].rectTransform.position);
            if (!pointDisplays[i].IsDone())
            {
                pointDisplays[i].Tick(Time.deltaTime);
            }
            else
            {
                Destroy(pointDisplays[i].Stop());
                pointDisplays.RemoveAt(i--);
            }
        }
        //scoreAmount += pointsPerSecond * Time.deltaTime;

    }

    public void AddPoints(float amount)
    {
        scoreAmount += amount;
        pointDisplays.Add(new PointDisplay(scoreText.gameObject.transform.parent.gameObject, "+" + amount.ToString(), new Color(0, 255, 0)));
        //scoreAmount += (amount > 0 ? amount : 0); //if amount is greater than 0, add that amount, otherwise add 0 points
    }

    public void RemovePoints(float amount)
    {
        scoreAmount += amount;
        pointDisplays.Add(new PointDisplay(scoreText.gameObject.transform.parent.gameObject, amount.ToString(), new Color(255, 0, 0)));
        //scoreAmount -= (amount > 0 ? amount : 0); //if amount is greater than 0, subtract that amount, otherwise subtract 0 points
    }

    public float GetPoints()
    {
        return scoreAmount;
    }
    
}

public class PointDisplay
{
    internal string message;
    internal float time;
    internal float fade;
    internal float speed;
    internal float spread;
    internal bool spreadLR;
    internal Color color;
    private bool ready = false;
    private bool done = false;
    private GameObject display;
    internal RectTransform rectTransform;
    private Text text;
    private float timer;
    private GameObject p;

    private float minSpread;
    private float maxSpread;

    public void Init()
    {
        maxSpread = Mathf.Abs(spread);
        minSpread = maxSpread/2;
        if (!spreadLR) { minSpread = -spread; maxSpread = minSpread/2; }

        display = new GameObject{name = "PointDisplay"};
        display.transform.parent = p.transform;
        rectTransform = display.AddComponent<RectTransform>();
        display.AddComponent<CanvasRenderer>();
        text = display.AddComponent<Text>();

        //rectTransform.position = Vector3.zero;
        rectTransform.anchorMax = new Vector2(0.5f, 0);
        rectTransform.anchorMin = new Vector2(0.5f, 0);
        rectTransform.pivot = new Vector2(0.5f, 1);
        rectTransform.localScale = Vector3.one;
        rectTransform.sizeDelta = new Vector2(185, 25);
        rectTransform.anchoredPosition3D = Vector3.zero;
        //rectTransform.localPosition = Vector3.zero;

        text.resizeTextForBestFit = true;
        text.alignment = TextAnchor.UpperCenter;
        text.color = color;
        text.text = message;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

        //timer = 0;
        ready = true;

        //Debug.Log("Points Ready1! " + rectTransform.position);
        //Debug.Log("Points Ready2! " + rectTransform.localPosition);
    }

    public PointDisplay(GameObject parent, string displayText, Color textColor)
    {
        p = parent;
        message = displayText;
        timer = Random.Range(0.7f, 1.0f);
        fade = Random.Range(4f, 6f);
        speed = Random.Range(160, 200);
        spread = Random.Range(100, 140);
        spreadLR = Random.Range(0.0f, 1.0f) >= 0.5 ? true : false;
        //Debug.Log(Random.Range(0.0f, 1.0f));
        color = textColor;
        Init();
    }

    public PointDisplay(GameObject parent, string displayText, Vector2 displayTime, Vector2 fadeTime, Vector2 moveSpeed, Vector2 randomSpread, bool spreadDirection, Color textColor)
    {
        p = parent;
        message = displayText;
        timer = Random.Range(displayTime.x, displayTime.y);
        fade = Random.Range(fadeTime.x, fadeTime.y);
        speed = Random.Range(moveSpeed.x, moveSpeed.y);
        spread = Random.Range(randomSpread.x,randomSpread.y);
        color = textColor;
        Init();
    }

    public void Send()
    {
        ready = true;
    }

    public void Tick(float dt)
    {
        if (!ready) { return; }
        timer = timer > 0 ? timer - dt : 0;
        rectTransform.position += new Vector3(Random.Range(minSpread,maxSpread)/(0.4f+(timer*3.5f)), speed, 0) * dt * text.color.a;
        if (timer <= 0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (fade*dt));
        }
        if (text.color.a <= 0) { ready = false; done = true; }
    }

    public bool IsDone()
    { return done; }

    public GameObject Stop()
    {
        done = true;
        ready = false;
        return display;
    }

}
