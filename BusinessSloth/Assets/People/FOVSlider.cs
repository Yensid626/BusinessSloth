using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVSlider : MonoBehaviour
{
    // Start is called before the first frame update
    public float FOV = 70;
    public float farClipping = 10;
    public float nearClipping = 0.2f;
    public bool visible = true;
    internal List<LineRenderer> lines = new List<LineRenderer>();
    float cosAng;
    float sinAng;
    float tanAng;
    float renderedFOV;
    Transform p;
    Vector3 offset;
    void Start()
    {
        transform.Find("originMarker").gameObject.SetActive(false);
        p = transform.parent.transform;
        for (int i = 0; i < 12; i++)
        {
            GameObject obj = new GameObject("Line:" + i);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.parent = transform;
            LineRenderer line = obj.AddComponent<LineRenderer>();
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;
            line.material = Resources.Load("LineMaterials/Move", typeof(Material)) as Material;
            lines.Add(line);
        } //add 4 lines for farClip, 4 for near Clip, and 4 for edges
        renderedFOV = FOV + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (renderedFOV != FOV)
        {
            renderedFOV = FOV;
            cosAng = Utils.Cos(90 - FOV); //using Mathf instead of Utils to get radians returned
            sinAng = Utils.Sin(90 - FOV);
            tanAng = Utils.Tan(90 - FOV);
            gameObject.SetActive(visible);
        }

        offset = transform.position;
        lines[0].SetPosition(0, offset + p.TransformVector(new Vector3(cosAng * nearClipping, tanAng * nearClipping, sinAng * nearClipping))); lines[0].SetPosition(1, offset + p.TransformVector(new Vector3(cosAng * farClipping, tanAng * farClipping, sinAng * farClipping)));
        lines[1].SetPosition(0, offset + p.TransformVector(new Vector3(cosAng * nearClipping, -tanAng * nearClipping, sinAng * nearClipping))); lines[1].SetPosition(1, offset + p.TransformVector(new Vector3(cosAng * farClipping, -tanAng * farClipping, sinAng * farClipping)));
        lines[2].SetPosition(0, offset + p.TransformVector(new Vector3(-cosAng * nearClipping, -tanAng * nearClipping, sinAng * nearClipping))); lines[2].SetPosition(1, offset + p.TransformVector(new Vector3(-cosAng * farClipping, -tanAng * farClipping, sinAng * farClipping)));
        lines[3].SetPosition(0, offset + p.TransformVector(new Vector3(-cosAng * nearClipping, tanAng * nearClipping, sinAng * nearClipping))); lines[3].SetPosition(1, offset + p.TransformVector(new Vector3(-cosAng * farClipping, tanAng * farClipping, sinAng * farClipping)));

        for (int clip = 0; clip < 4; clip++) //set edsge lines
        {
            lines[(clip * 2) + 4].SetPosition(0, lines[clip].GetPosition(0)); lines[(clip * 2) + 4].SetPosition(1, lines[(clip + 1)%4].GetPosition(0)); //set near clipping plane
            lines[(clip * 2) + 5].SetPosition(0, lines[clip].GetPosition(1)); lines[(clip * 2) + 5].SetPosition(1, lines[(clip + 1) % 4].GetPosition(1)); //set far clipping plane
        }
    }
}
