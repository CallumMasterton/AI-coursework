using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackRangeNode : Node
{
    public BossAI bossAi;

    public ChargeAttackRangeNode(BossAI bossAi)
    {
        this.bossAi = bossAi;
    }

    public override NodeState Evaluate()
    {
        if (Sensors.rayPlayerHit == true || ChargeAttackNode.isCharging == true)
        {
            Debug.Log("I saw the Player in attack range");
            return NodeState.SUCCESS;//Sets the node to success
        }

        Debug.Log("I don't see the Player in attack range");
        return NodeState.FAILURE;//Sets the node to failure
    }
}
