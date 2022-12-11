using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPatrol : State
{
    AiCrowd aiCrowd;
    Transform waypointTarget;
    bool spawning = false;

    public ZPatrol(AiCrowd aiCrowd)
    {
        this.aiCrowd = aiCrowd;
    }

    public override void Enter()
    {
        if (BossAI.justZomSpawned) spawning = true;
        aiCrowd.gameObject.GetComponent<CrowedEntity>().enabled = false;
        if (spawning)//Uses spawn route 
        {
            waypointTarget = aiCrowd.wayPointsList.transform;
            float dist = Vector3.Distance(aiCrowd.transform.position, waypointTarget.position);
            Debug.Log(dist);
            aiCrowd.agent.SetDestination(waypointTarget.position);
        }
        else//Randomly picks a pth to use
        {
            waypointTarget = aiCrowd.wayPoints[Random.Range(0, aiCrowd.wayPoints.Length)].transform;
            float dist = Vector3.Distance(aiCrowd.transform.position, waypointTarget.position);
            Debug.Log(dist);
            aiCrowd.agent.SetDestination(waypointTarget.position);
        }
    }

    public override void Execute()
    {

        float dist = Vector3.Distance(aiCrowd.transform.position, waypointTarget.position);

        if (dist < 1)//Picks a new waypoint
        {
            spawning = false;
            Debug.Log("Trying to change waypoint");
            waypointTarget = aiCrowd.wayPoints[Random.Range(0, aiCrowd.wayPoints.Length)].transform;
            aiCrowd.agent.SetDestination(waypointTarget.position);

        }


    }

    public override void Exit()
    {
        Debug.Log("Z out");
    }
}
