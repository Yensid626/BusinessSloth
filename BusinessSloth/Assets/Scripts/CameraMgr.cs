using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoBehaviour
{
    
    public Vector3 velocity;
    public Vector3 position;
    public Vector3 facing;
    public float deltaVelocity = 10;
    Vector3 rotation;
    public float deltaRotation = 3;
    public float soundEventTimer = 0;

    public GameObject FPRig;
    internal GameObject FPChar;
    public bool firstPerson = false;

    public static CameraMgr inst;

    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        position = FPRig.transform.position;
        facing = FPRig.transform.rotation.eulerAngles;
        /*AlertSystem.inst.CreateSoundEvent(EventType.RightEdge, 1, Color.red, true);
        AlertSystem.inst.CreateSoundEvent(EventType.RightEdge, 2, Color.blue, true);
        AlertSystem.inst.CreateSoundEvent(EventType.RightEdge, 3, Color.green, 3);
        AlertSystem.inst.CreateSoundEvent(EventType.RightEdge, 1, Color.cyan, true);*/
    }

    // Update is called once per frame
    /*void Update()
    { }*/

    internal void Tick(float dt)
    {
        RotateChair.inst.Tick(dt);
        ProcessInput();
        Physics(dt);
        ShowSoundEvents(dt);
    }

    void ProcessInput()
    {
        velocity = Vector3.zero;
        rotation = Vector3.zero;

            if (Input.GetKey(KeyCode.A))
                rotation.y -= deltaRotation;
            if (Input.GetKey(KeyCode.D))
                rotation.y += deltaRotation;
            if (Input.GetKey(KeyCode.W))
                rotation.x -= deltaRotation;
            if (Input.GetKey(KeyCode.S))
                rotation.x += deltaRotation;
            if (Input.GetKey(KeyCode.X))
                rotation.z -= deltaRotation;
            if (Input.GetKey(KeyCode.Z))
                rotation.z += deltaRotation;

            if (Input.GetKey(KeyCode.A))
                velocity.x -= deltaVelocity;
            if (Input.GetKey(KeyCode.D))
                velocity.x += deltaVelocity;
            if (Input.GetKey(KeyCode.W))
                velocity.z += deltaVelocity;
            if (Input.GetKey(KeyCode.S))
                velocity.z -= deltaVelocity;
            //if (Input.GetKey(KeyCode.F))
            //    velocity.y -= deltaVelocity;
            //if (Input.GetKey(KeyCode.E))
            //    velocity.y += deltaVelocity;

        //if (Input.GetKeyDown(KeyCode.C))
        //    firstPerson = !firstPerson;
    }

    void Physics(float dt)
    {
        if (!firstPerson)
        {
            FPRig.transform.position = position;
            FPRig.transform.rotation = Quaternion.Euler(facing.x, facing.y, facing.z);
            FPRig.transform.localPosition += FPRig.transform.TransformDirection(velocity * dt);

            //transform.localPosition = transform.localPosition + (velocity * dt);
            //transform.localPosition += (velocity.right * dt);
            //transform.localPosition += (velocity.forward * dt);
            //transform.localPosition += (velocity.up * dt);
            FPRig.transform.Rotate(rotation * dt);
            //Debug.Log(FPRig.transform.rotation.eulerAngles.x); Debug.Log(Utils.Clamp(Utils.Degrees180(FPRig.transform.rotation.eulerAngles.x), -60, 60));
            FPRig.transform.rotation = Quaternion.Euler(Utils.Clamp(Utils.Degrees180(FPRig.transform.rotation.eulerAngles.x), -60, 60), FPRig.transform.rotation.eulerAngles.y, 0);


            position = FPRig.transform.position;
            facing = FPRig.transform.rotation.eulerAngles;
        }
        else
        {
            FPRig.transform.position = FPChar.transform.Find("CameraRig").transform.position;
            //transform.localPosition = (FPChar.transform.localPosition + new Vector3(0,6,0));
            //transform.localPosition += transform.TransformDirection(new Vector3(-4,0,0));
            //Realy old stuff -->//transform.rotation = Quaternion.Euler(new Vector3(FPChar.transform.rotation.x, FPChar.transform.rotation.y, FPChar.transform.rotation.z));
            //transform.eulerAngles = FPChar.transform.eulerAngles;
            FPRig.transform.eulerAngles = FPChar.transform.Find("CameraRig").transform.eulerAngles;
        }
    }

    void ShowSoundEvents(float dt)
    {
        if ((soundEventTimer -= dt) > 0) { return; }

        soundEventTimer = 0.1f;
        foreach (Entity381 ent381 in EntityMgr.inst.entitiesPeople)
        {
            GameObject ent = ent381.gameObject;
            //Debug.Log(Utils.Facing(FPRig, ent));
            if (Utils.Facing(FPRig, ent) >= -50f / 180)
            {
                EventType type = EventType.FullScreen;
                switch (Utils.LeftRightCenter(FPRig.transform, ent.transform.position))
                {
                    case -1: type = EventType.LeftEdge; break;
                    case 0: type = EventType.LeftEdge; break;
                    case 1: type = EventType.RightEdge; break;
                }
                if (Utils.Facing(FPRig, ent) >= 145f / 180) { type = EventType.BottomEdge; }

                float minHearingDist = 2.9f;
                float maxHearingDist = 16f;
                float dist = Utils.Clamp(Utils.getDist(ent.transform.position, FPRig.transform.position) - minHearingDist, 0, 25);
                if (dist <= maxHearingDist)
                {
                    float soundLevel = ent381.speed / ent381.maxSpeed;
                    float lerp = Utils.exponentialMap(0, 1, 0, 1, Utils.exponentialMap(0, maxHearingDist, 1, 0, dist, 2.55f), 12f);
                    Color color = Color.Lerp(new Color(160, 255, 0), new Color(255, 0, 0), Utils.Clamp(lerp * soundLevel,0,1));
                    //Debug.Log(ent.name + ": " + dist + "\n" + lerp + "\n" + color);
                    //Color color = new Color(Utils.Map(3, 12, 255, 180, dist)/255f, Utils.Map(3, 12, 0, 255, dist)/255f, 0);
                    //Debug.Log(color*255);
                    //Debug.Log(Color.Lerp(new Color(200, 255, 0), new Color(255, 0, 0), Utils.Clamp(Utils.Map(0, 16, 0, 1, dist), 0, 1)));
                    AlertSystem.inst.CreateSoundEvent(type, (int)(Utils.Map(0,maxHearingDist,4,1,dist)), color / 255f, 0.2f);
                    //AlertSystem.inst.CreateSoundEvent(type, (int)(Utils.Map(0,maxHearingDist,4,1,dist)*soundLevel), color, soundEventTimer*2.5f);
                }
            }

        }
    }

    public void MoveToCenter(float dt)
    {
        rotation = Vector3.zero;
        Vector3 rotationMultiplier = Vector3.one;
        if (!Utils.ApproximatelyEqual(Utils.RoundToNearest(facing.x,1), 0)) { rotation.x = (facing.x < 0 ? deltaRotation : -deltaRotation); }
        if (!Utils.ApproximatelyEqual(Utils.RoundToNearest(facing.y,1), 278f)) { rotation.y = (facing.y < 278 ? deltaRotation : -deltaRotation); }
        if (!Utils.ApproximatelyEqual(Utils.RoundToNearest(facing.z,1), 0)) { rotation.z = (facing.z < 0 ? deltaRotation : -deltaRotation); }

        for (int i = 0; i < 11; i++)
        {
            if (Mathf.Abs(facing.x - 0) < deltaRotation * (1 - (i / 10))) { rotationMultiplier.x *= (0.84f - (i / 50)); }
            if (Mathf.Abs(facing.y - 278.5f) < deltaRotation * (1 - (i / 10))) { rotationMultiplier.y *= (0.84f - (i / 50)); }
            if (Mathf.Abs(facing.z - 0) < deltaRotation * (1 - (i / 10))) { rotationMultiplier.z *= (0.84f - (i / 50)); }
            //Debug.Log(rotationMultiplier.y);
        }
        rotation = new Vector3(rotation.x * rotationMultiplier.x, rotation.y * rotationMultiplier.y, rotation.z * rotationMultiplier.z) * 0.6f;
        //Debug.Log(facing.y + " ~ " + rotation.y);
        Physics(dt);
    }
}
