using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    //----------------------------------------------------------------------------------------
    //    Place This Script Inside Objects That Award Points When Clicked On
    //----------------------------------------------------------------------------------------

    void Start() { }
    public int points = 0; //how many points the user recieves from clicking

    public void AddPoints()
    {
        Score.inst.AddPoints(points);
    }

    public void RemovePoints()
    {
        Score.inst.RemovePoints(points);
    }

    public void AwardPoints()
    {
        AwardPoints(1);
    }
    public void AwardPoints(float multiplier)
    {
        if (points < 0)
        {
            Score.inst.RemovePoints(Utils.Round(points * multiplier, 0));
            return;
        }
        Score.inst.AddPoints(Utils.Round(points * multiplier, 0));
    }
}
