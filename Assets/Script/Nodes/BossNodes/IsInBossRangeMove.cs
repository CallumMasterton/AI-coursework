using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInBossRangeMove : Node
{
    private Transform origin;
    private float sightDis;
    private float hightMult;


    public IsInBossRangeMove(Transform origin, float sightDis, float hightMult)
    {
        this.origin = origin;//Brings the transform of the gameobject into the script
        this.sightDis = sightDis;
        this.hightMult = hightMult;
    }

    public override NodeState Evaluate()
    {
        if (Sensors.rayPlayerHit == true)//if the player is hit then it is true
        {
            Debug.Log("I saw the Player");
            return NodeState.FAILURE;//Sets the node to success
        }

        Debug.Log("I don't see the Player");
        return NodeState.SUCCESS;//Sets the node to failure
    }
}
