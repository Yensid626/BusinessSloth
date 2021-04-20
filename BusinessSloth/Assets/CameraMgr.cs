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

    public GameObject FPRig;
    internal GameObject FPChar;
    public bool firstPerson = false;

    // Start is called before the first frame update
    void Start()
    {
        position = FPRig.transform.position;
        facing = FPRig.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    /*void Update()
    { }*/

    internal void Tick(float dt)
    {
        ProcessInput();
        Physics(dt);
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
}
