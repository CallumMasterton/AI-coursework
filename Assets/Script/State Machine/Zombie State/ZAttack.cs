using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAttack : State
{

    AiCrowd aiCrowd;

    public static bool TakenDamage = false;


    public ZAttack(AiCrowd aiCrowd)
    {
        this.aiCrowd = aiCrowd;
    }


    public override void Enter()
    {
        aiCrowd.gameObject.GetComponent<CrowedEntity>().enabled = false;
        TakenDamage = true;
        Debug.Log("Shadow Attack");
        aiCrowd.agent.isStopped = true;
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        TakenDamage = false;
        aiCrowd.agent.isStopped = false;
    }
}
