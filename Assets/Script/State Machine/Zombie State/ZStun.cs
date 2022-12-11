using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZStun : State
{
    AiCrowd aiCrowd;
    public ZStun(AiCrowd aiCrowd)
    {
        this.aiCrowd = aiCrowd;
    }

    public override void Enter()//Stops the Mob from moving
    {
        aiCrowd.gameObject.GetComponent<CrowedEntity>().enabled = false;
        aiCrowd.agent.isStopped = true;

    }

    public override void Execute()
    {

    }

    public override void Exit()//allows it so move
    {
        aiCrowd.agent.isStopped = false;
    }
}
