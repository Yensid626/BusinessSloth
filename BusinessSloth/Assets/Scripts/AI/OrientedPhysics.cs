using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientedPhysics : MonoBehaviour
{
    // Start is called before the first frame update
    private Entity381 entity;
    void Start()
    {
        entity = gameObject.GetComponent<Entity381>();
    }

    // Update is called once per frame
    /*void Update()
    { }*/

    internal void Tick(float dt)
    {
        if (entity != null)
            Physics(dt);
    }

    public float turnSpeedMultiplier = 4.0f;
    public float accelerationMultiplier = 2.8f;
    void Physics(float dt)
    {
        //clamp heading
        entity.desiredHeading = Utils.Degrees360(entity.desiredHeading);
        //entity.desiredHeading = (entity.desiredHeading >= 360 ? entity.desiredHeading - 360 : entity.desiredHeading);
        //entity.desiredHeading = (entity.desiredHeading < 0 ? entity.desiredHeading + 360 : entity.desiredHeading);

        //rotate to desired heading
        entity.heading = transform.rotation.eulerAngles.y;
        float dH = Utils.AngleDiffPosNeg(entity.desiredHeading, entity.heading);
        float tempMultiplier = 1;
        //if (Mathf.Abs(dH) < 1f) { tempMultiplier = 0.9f; }
        //if (Mathf.Abs(dH) < 0.5f) { tempMultiplier = 0.7f; }
        if (Mathf.Abs(dH) <= 4 * entity.turnSpeed * turnSpeedMultiplier * dt) { tempMultiplier = 0.2f; } //less than 4 frames away
        if (Mathf.Abs(dH) <= 2 * entity.turnSpeed * turnSpeedMultiplier * dt) { tempMultiplier = 0.1f; } //less than 2 frames away
        if (Mathf.Abs(dH) <= 1 * entity.turnSpeed * turnSpeedMultiplier * dt) { tempMultiplier = 0.05f; } //less than 1 frame away
        //Debug.Log(dH + ":" + (entity.turnSpeed * dt * turnSpeedMultiplier));
        //Debug.Log("   " + tempMultiplier);
        if ((dH > 0) && (dH <= 180))
        { transform.Rotate(new Vector3(0, entity.turnSpeed * dt * turnSpeedMultiplier * tempMultiplier, 0)); }
        else if (!Utils.ApproximatelyEqual(dH,0))
        { transform.Rotate(new Vector3(0, -entity.turnSpeed * dt * turnSpeedMultiplier * tempMultiplier, 0)); }

        //accelerate
        if (Utils.ApproximatelyEqual(entity.speed, entity.desiredSpeed))
        {
            entity.speed = entity.desiredSpeed;
        }
        if (entity.speed < entity.desiredSpeed)
        { entity.speed += (entity.acceleration * dt * accelerationMultiplier); }
        else if (entity.speed > entity.desiredSpeed)
        { entity.speed -= entity.acceleration * dt * accelerationMultiplier; }
        entity.speed = Utils.Clamp(entity.speed, entity.minSpeed, entity.maxSpeed);
        /*if ((speed < desiredSpeed) && (speed < maxSpeed))
            { speed += acceleration * dt; }
        if ((speed > desiredSpeed) && (speed > minSpeed))
            { speed -= acceleration * dt; }*/

        //move
        Vector3 velocity;
        //velocity = new Vector3(Utils.Sin(entity.heading), 0, Utils.Cos(entity.heading));
        velocity = Utils.getPositionFromAngle(entity.heading, entity.speed * dt);
        entity.transform.localPosition = entity.transform.localPosition + velocity;
        //entity.transform.position = entity.transform.localPosition + (velocity * entity.speed * dt);
    }
}
