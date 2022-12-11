using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackNode : Node
{
    public static bool GoingtoHitPlayer = false;
    public static bool isCharging = false;
    public BossAI bossAi;
    Transform chargeLocation;
    Transform bossPos;
    Rigidbody rb;
    float timeRemaining = 0.4f;
    float speed = 50;
    bool isDone = false;


    public ChargeAttackNode(BossAI bossAi)
    {
        this.bossAi = bossAi;
        bossPos = bossAi.bossPos;
        rb = bossAi.GetComponent<Rigidbody>();
    }


    public override NodeState Evaluate()
    {
        if (SlamAttackNode.doneSlam == true && isDone == true)//Resets this node
        {
            timeRemaining = 0.4f;
            isDone = false;
            isCharging = false;
            Debug.Log("Resting charge");
        }

        if (!isCharging && Sensors.rayPlayerHit)
        {
            isCharging = true;
        }

        if (bossPos.rotation.x != 0 || bossPos.rotation.z != 0) bossPos.rotation = Quaternion.Euler(0, bossPos.eulerAngles.y, 0);//resets the rotatoin of the boss

        if (timeRemaining > 0)
        {
            GoingtoHitPlayer = true;
            bossAi.gameObject.GetComponent<BoxCollider>().enabled = true;//Allows the boss to take damage 
            bossAi.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            bossPos.LookAt(null);
            rb.MovePosition(rb.position + bossAi.transform.forward * speed * Time.deltaTime);
            timeRemaining -= Time.deltaTime;
            return NodeState.FAILURE;
        }
        else
        {
            GoingtoHitPlayer = false;
            bossAi.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            isDone = true;
            Debug.Log("Time to rest for a bit");
            return NodeState.SUCCESS;
        }
    }
}
