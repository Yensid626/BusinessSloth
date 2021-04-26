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
}
