using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : Node
{
    private GameObject deathBeam;
    public BossAI bossAi;
    public BossAttack(GameObject deathBeam, BossAI bossAi)//Brings the transform of the gameobject into the script
    {
        this.deathBeam = deathBeam;
        this.bossAi = bossAi;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Attacking the player");
        deathBeam.SetActive(true);
        return NodeState.RUNNING;//Sets this node to fail
    }
}
