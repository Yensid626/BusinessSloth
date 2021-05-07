using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateChair : MonoBehaviour
{
    //public GameObject cameraMgr;
    public float rotateSpeed = 5.0f;
    GameObject cam;
    public static RotateChair inst;

    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = CameraMgr.inst.FPRig.gameObject;
    }

    // Update is called once per frame
    public void Tick(float dt)
    {
        transform.position = new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z);
        float dH = Utils.differenceDegrees(transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.y);
        dH = 1 - Utils.exponentialMap(0, 1, 0, 1, dH, 0.6f);
        float diff = Utils.AngleDiffPosNeg(transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.y);
        //Debug.Log(dH + " \\ " + diff + " // " + transform.rotation.eulerAngles.y + " | " + cam.transform.rotation.eulerAngles.y);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + (dH * -diff), 0);
    }
}
