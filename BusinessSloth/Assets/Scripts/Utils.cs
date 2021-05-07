using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {


    public static float EPSILON = 0.01f;
    public static bool ApproximatelyEqual(float a, float b)
    {
        return (Mathf.Abs(a - b) < EPSILON);
    }

    public static float Clamp(float val, float min, float max)
    {
        if (val < min)
            val = min;
        if (val > max)
            val = max;
        return val;
    }

    public static float AngleDiffPosNeg(float a, float b)
    {
        float diff = a - b;
        if (diff > 180)
            return diff - 360;
        if (diff < -180)
            return diff + 360;
        return diff;
    }

    public static float Degrees360(float angleDegrees)
    {
        while (angleDegrees >= 360)
            angleDegrees -= 360;
        while (angleDegrees < 0)
            angleDegrees += 360;
        return angleDegrees;
    }

    public static float Degrees180(float angleDegrees)
    {
        while (angleDegrees > 180)
            angleDegrees -= 360;
        while (angleDegrees <= -180)
            angleDegrees += 360;
        return angleDegrees;
    }

    public static float Cos(float degrees)
    { return Mathf.Cos(Mathf.Deg2Rad * degrees); }

    public static float Sin(float degrees)
    { return Mathf.Sin(Mathf.Deg2Rad * degrees); }

    public static float Tan(float degrees)
    { return Mathf.Tan(Mathf.Deg2Rad * degrees); }

    public static float Atan(float x, float y)
    { return Utils.Degrees360(Mathf.Rad2Deg * Mathf.Atan2(x, y)); }

    public static Vector3 DirectionTo(Vector3 from, Vector3 to)
    { return (new Vector3(to.x - from.x, 0, to.z - from.z)).normalized; }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    public static float Map(float currentMin, float currentMax, float newMin, float newMax, float value)
    {
        float OldRange = (currentMax - currentMin);
        float NewRange = (newMax - newMin);
        return (((value - currentMin) * NewRange) / OldRange) + newMin;
    }

    public static float getDist(Vector3 pos1, Vector3 pos2)
    {
        return (Vector3.Distance(pos2, pos1));
        //Vector3 sum = new Vector3(Mathf.Abs(pos1.x - pos2.x), pos1.y - pos2.y, pos1.z - pos2.z);
        //Debug.Log(sum);
        //return Mathf.Sqrt(sum.x + sum.y + sum.z);
    }

    public static float RoundToNearest(float value, float step)
    {
        float multiplier = 1.0f;
        while (step < 1) { step *= 10; multiplier++; }
        float adjustedVal = value * multiplier;
        return ((adjustedVal) - (adjustedVal % step))/multiplier;
    }

    public static float differenceDegrees(float deg1, float deg2)
    {
        return 1 - (Mathf.Abs(Utils.AngleDiffPosNeg(deg1, deg2) / 180)); //find absolute difference between deg1 and 2 (will always be between 0-180) and map between (1-0)
    }

    //public static float exponentialScaler(float currentMin, float currentMax, float newMin, float newMax, float value)
    public static float exponentialMap(float currentMin, float currentMax, float newMin, float newMax, float value, float curve = 3)
    {
        return (Mathf.Pow((value - currentMin) / (currentMax - currentMin), curve) * (newMax-newMin)) + newMin;
    }

    public static Vector3 getPositionFromAngle(float heading, float speed) //returns a direction vector scaled by speed
    {
        return new Vector3(Utils.Sin(heading) * speed, 0, Utils.Cos(heading) * speed);
    }

    public static float Facing(GameObject self, GameObject target)
    {
        return Vector3.Dot(self.transform.forward, (target.transform.position - self.transform.position).normalized);
    }
}
