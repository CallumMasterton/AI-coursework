using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttackNode : Node
{
    public BossAI bossAi;
    GameObject spinningLaser;
    GameObject AOE;
    GameObject chargeA;
    Transform origen;
    float RotationSpeed = 6;

    public SpinAttackNode(BossAI bossAi)
    {
        this.bossAi = bossAi;
        spinningLaser = bossAi.spinningLasers;
        origen = bossAi.bossPos;
        AOE = bossAi.aoeAttack;
        chargeA = bossAi.ChargeParticals;
    }

    public override NodeState Evaluate()
    {
        if (origen.transform.rotation.x != 0 || origen.transform.rotation.z != 0) origen.rotation = Quaternion.identity;//resets the rotatoin of the boss
        origen.transform.Rotate(Vector3.up * (RotationSpeed * Time.deltaTime));//Spins the boss
        AOE.SetActive(false);
        chargeA.SetActive(false);
        spinningLaser.SetActive(true);//Turns on attack
        return NodeState.RUNNING;
    }
}
