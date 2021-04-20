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

    public static float Atan(float x, float y)
    { return Utils.Degrees360(Mathf.Rad2Deg * Mathf.Atan2(x, y)); }

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

    public static float RoundToNearest(float value, float to)
    {
        float multiplier = 1.0f;
        while (to < 1) { to *= 10; multiplier++; }
        float adjustedVal = value * multiplier;
        return ((adjustedVal) - (adjustedVal % to))/multiplier;
    }

    public static float differenceDegrees(float deg1, float deg2)
    {
        return 1 - (Mathf.Abs(Utils.AngleDiffPosNeg(deg1, deg2) / 180)); 
    }

    //public static float exponentialScaler(float currentMin, float currentMax, float newMin, float newMax, float value)
    public static float exponentialMap(float currentMin, float currentMax, float newMin, float newMax, float value)
    {
        float curve = 3;
        return (Mathf.Pow((value - currentMin) / (currentMax - currentMin), curve) * (newMax-newMin)) + newMin;
    }

    public static Vector3 getPositionFromAngle(float heading, float speed)
    {
        return new Vector3(Utils.Sin(heading) * speed, 0, Utils.Cos(heading) * speed);
    }

    public static Vector3 get2DIntercept(Vector3 point2a, Vector3 point1a, Vector3 point2b, Vector3 point1b)
    {
        float slopeA = (point1a.z - point2a.z)/ (point1a.x - point2a.x);
        float slopeB = (point1b.z - point2b.z) / (point1b.x - point2b.x);
        float offsetA = point1a.z - (point1a.x * slopeA);
        float offsetB = point1b.z - (point1b.x * slopeB);
        float intercept = (offsetB - offsetA) / (slopeA - slopeB);
        return new Vector3(intercept, 0, (intercept * slopeA) + offsetA);
    }
}
