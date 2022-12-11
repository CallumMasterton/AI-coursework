using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZChase : State
{
    AiCrowd aiCrowd;
    public ZChase(AiCrowd aiCrowd)
    {
        this.aiCrowd = aiCrowd;
    }

    public override void Enter()
    {
        aiCrowd.gameObject.GetComponent<CrowedEntity>().enabled = true;//Turns the Crowding on
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
