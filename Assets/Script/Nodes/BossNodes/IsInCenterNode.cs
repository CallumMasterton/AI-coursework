using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInCenterNode : Node
{
    public IsInCenterNode()
    {

    }

    public override NodeState Evaluate()
    {
        if (BossAI.atCenterNode == false)//Checks if boss is toching the center node
        {
            Debug.Log("Not in the middle");
            return NodeState.SUCCESS;
        }
        Debug.Log("in the middle");

        return NodeState.FAILURE;
    }
}
