using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAOENode : Node
{
    GameObject smallAOE;

    public SmallAOENode(GameObject smallAOE)
    {
        this.smallAOE = smallAOE;
    }

    public override NodeState Evaluate()
    {
        smallAOE.SetActive(true);//Turns on attack
        return NodeState.RUNNING;
    }
}
