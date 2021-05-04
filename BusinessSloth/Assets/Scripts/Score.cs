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
        int i = 0;
        for (i = 0; i <= pointDisplays.Count - 1; i++)
        {
            if (!pointDisplays[i].IsDone())
            {
                pointDisplays[i].Tick(Time.deltaTime);
            }
            else
            {
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
    internal Color color;
    private bool ready = false;
    private bool done = false;
    private GameObject display;
    private RectTransform rectTransform;
    private Text text;
    private float timer;
    private GameObject p;

    public void Init()
    {
        display = new GameObject();
        display.transform.parent = p.transform;
        rectTransform = display.AddComponent<RectTransform>();
        display.AddComponent<CanvasRenderer>();
        text = display.AddComponent<Text>();

        rectTransform.anchorMax = new Vector2(0.5f, 0);
        rectTransform.anchorMin = new Vector2(0.5f, 0);
        rectTransform.pivot = new Vector2(0.5f, 1);
        rectTransform.sizeDelta = new Vector2(185, 25);
        rectTransform.position = new Vector3(0, 0, 0);

        text.resizeTextForBestFit = true;
        text.alignment = TextAnchor.UpperCenter;
        text.color = color;
        text.text = message;

        timer = 0;
        ready = true;
    }

    public PointDisplay(GameObject parent, string displayText, Color textColor)
    {
        p = parent;
        message = displayText;
        timer = 0.8f;
        fade = 9;
        speed = 3;
        spread = 2;
        color = textColor;
        Init();
    }

    public PointDisplay(GameObject parent, string displayText, float displayTime, float fadeTime, float moveSpeed, float randomSpread, Color textColor)
    {
        p = parent;
        message = displayText;
        timer = displayTime;
        fade = fadeTime;
        speed = moveSpeed;
        spread = randomSpread;
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
        timer -= dt;
        rectTransform.position += new Vector3(Random.Range(-spread, spread), speed, 0) * dt;
        if (timer <=0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - fade);
        }
        if (text.color.a <= 0) { ready = false; done = true; }
    }

    public bool IsDone()
    { return done; }

}
