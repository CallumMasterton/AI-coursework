using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDoingChargeNode : Node
{
    public IsDoingChargeNode()
    {

    }

    public override NodeState Evaluate()
    {
        if (!ChargeAttackNode.isCharging)//If the boss is not chargeing this will work
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }

}
