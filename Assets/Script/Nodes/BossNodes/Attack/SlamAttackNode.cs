using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttackNode : Node
{
    public BossAI bossAi;
    Transform bossPos;
    float dropTimeRemaining = 1;
    float timeRemaining = 3;
    bool moveTime = false;
    bool downAndOut = false;
    public static bool doneSlam = false;


    public SlamAttackNode(BossAI bossAi)
    {
        this.bossAi = bossAi;
        bossPos = bossAi.bossPos;
    }

    public override NodeState Evaluate()
    {
        bossAi.gameObject.GetComponent<BoxCollider>().enabled = true;//Allows the boss to take damage 
        bossAi.gameObject.GetComponent<BoxCollider>().size = new Vector3(2, 2, 2);

        if (ChargeAttackNode.isCharging && doneSlam)
        {
            Debug.Log("Resting Slam");
            dropTimeRemaining = 1;
            doneSlam = false;
        }
        if (bossPos.position.y != 1.5) { doneSlam = false; }
        if (moveTime) { bossPos.transform.position = Vector3.Lerp(bossPos.position, new Vector3(bossPos.position.x, 1.5f, bossPos.position.z), 1); }

        if (dropTimeRemaining > 0)//Sets the timer for the down time of the boss
        {

            timeRemaining = 3;
            doneSlam = false;
            moveTime = true;
            dropTimeRemaining -= Time.deltaTime;
            return NodeState.RUNNING;
        }
        else
        {
            if (timeRemaining > 0)
            {
                downAndOut = true;
                moveTime = false;
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                doneSlam = true;
                return NodeState.SUCCESS;
            }
        }

        return NodeState.FAILURE;
    }
}
