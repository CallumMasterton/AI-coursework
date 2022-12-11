using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZEvadeBoss : State
{   //based on Week 8 Steering Behaviours - https://moodle.bcu.ac.uk/course/view.php?id=79511
    AiCrowd aiCrowd;

    public ZEvadeBoss(AiCrowd aiCrowd)
    {
        this.aiCrowd = aiCrowd;
    }

    public override void Enter()
    {
        aiCrowd.gameObject.GetComponent<CrowedEntity>().enabled = false;
    }

    public override void Execute()
    {
        int iterationsAhead = 5;

        Vector3 bossSpeed = BossAI.instantVelocity;//Gets the speed
        Vector3 bossFuturePosition = aiCrowd.boss.transform.position + (bossSpeed * iterationsAhead);//Works out were it needs to be from the number of iterations ahead
        Vector3 direction = aiCrowd.transform.position - bossFuturePosition;//Works out the postion

        aiCrowd.agent.SetDestination(direction);//Moves to loaction
        Debug.Log("Giving the boss space");
    }

    public override void Exit()
    {

    }
}
