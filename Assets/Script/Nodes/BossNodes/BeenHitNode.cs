using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeenHitNode : Node
{
    public BossAI bossAi;
    private GameObject deathBeam;
    private GameObject particalsCharge;
    private GameObject aoeAttack;

    public BeenHitNode(BossAI bossAi)//Brings the values into the script
    {
        this.bossAi = bossAi;
        particalsCharge = bossAi.ChargeParticals;
        deathBeam = bossAi.deathBeam;
        aoeAttack = bossAi.aoeAttack;
    }

    public override NodeState Evaluate()
    {
        if (BossAI.beenHit == false)
        {
            //aoeAttack.SetActive(false);
            Debug.Log("Boss has not been hit");
            return NodeState.SUCCESS;//Sets this node to success
        }
        particalsCharge.SetActive(false);
        deathBeam.SetActive(false);
        Debug.Log("Ow I have been Hit");
        return NodeState.FAILURE;//Sets this node to fail

    }
}
