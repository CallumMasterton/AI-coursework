using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackRangeNode : Node
{
    public BossAI bossAi;
    private Transform origin;
    private float sightDis;
    private float hightMult;

    private GameObject deathBeam;
    private GameObject particalsCharge;


    public BossAttackRangeNode(Transform origin, float sightDis, float hightMult, BossAI bossAi)
    {
        this.origin = origin;//Brings the transform of the gameobject into the script
        this.sightDis = sightDis;
        this.hightMult = hightMult;
        this.bossAi = bossAi;

        deathBeam = bossAi.deathBeam;
        particalsCharge = bossAi.ChargeParticals;
    }

    public override NodeState Evaluate()
    {
        if (Sensors.rayPlayerHit == true)
        {
            Debug.Log("I saw the Player in attack range");
            return NodeState.SUCCESS;//Sets the node to success
        }

        Debug.Log("I don't see the Player in attack range");
        particalsCharge.SetActive(false);
        deathBeam.SetActive(false);
        return NodeState.FAILURE;//Sets the node to failure
    }
}

