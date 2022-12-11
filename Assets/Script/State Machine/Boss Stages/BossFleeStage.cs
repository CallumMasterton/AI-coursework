using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFleeStage : State
{
    BossAI owner;
    GameObject bombPrefab;
    Transform original;
    float timeRemaining = 1;
    Transform cachedTransform;
    RaycastHit info = new RaycastHit();
    public bool Hit { get; private set; }


    public BossFleeStage(BossAI owner)//Pulls information from boss script 
    {
        this.owner = owner;
        bombPrefab = owner.deathBomb;
        original = owner.transform;
    }

    public override void Enter()
    {
        Debug.Log("Run away start");
        owner.bossPos.position = owner.fleeStagePoint.position;//Moves boss to new postion
        cachedTransform = owner.GetComponent<Transform>();

        for (int i = 0; i < owner.gameObject.transform.childCount; i++)
        {
            owner.gameObject.transform.GetChild(i).gameObject.SetActive(false);//Set all children to off
        }
    }

    public override void Execute()
    {
        Debug.Log("Run away is going on");
        owner.transform.LookAt(owner.Target);


        Vector3 dir = cachedTransform.forward;
        if (Physics.Linecast(cachedTransform.position, cachedTransform.position + dir * 10, out info, owner.hitMask, QueryTriggerInteraction.Ignore)) { Hit = true; }
        else { Hit = false; }

        if (!Hit)//Cecks if boss has a clear shot
        {
            float tarDis = Vector3.Distance(owner.transform.position, owner.Target.transform.position);
            Debug.DrawRay(owner.transform.position + Vector3.up * -0.5f, owner.transform.forward * tarDis, Color.green);//Draws a line to the player from the boss

            if (timeRemaining > 0)//When timer is done it will spawn a bomb prefab
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Shooting Bomb");
                GameObject.Instantiate(bombPrefab, new Vector3(original.position.x, original.position.y + 2, original.position.z + 1), Quaternion.identity);
                timeRemaining = 5;
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Back to fight");
    }
}
