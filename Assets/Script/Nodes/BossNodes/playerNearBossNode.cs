using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNearBossNode : Node
{
    BossAI bossAi;
    GameObject spinningLaser;

    public PlayerNearBossNode(BossAI bossAi)
    {
        this.bossAi = bossAi;
        spinningLaser = bossAi.spinningLasers;
    }

    public override NodeState Evaluate()
    {
        if (Sensors.rayPlayerHit == false)//If its false it will move to attack
        {
            Debug.Log("Player is not in the box");
            return NodeState.SUCCESS;
        }
        Debug.Log("Player is in the box");
        spinningLaser.SetActive(false);//If it fails it will turn off the attack
        return NodeState.FAILURE;
    }
}
