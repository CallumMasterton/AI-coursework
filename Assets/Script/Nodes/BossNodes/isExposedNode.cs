using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isExposedNode : Node
{
    BossAI bossAi;

    public isExposedNode(BossAI bossAi)//Brings the transform of the gameobject into the script
    {
        this.bossAi = bossAi;
    }

    public override NodeState Evaluate()
    {
        bossAi.gameObject.GetComponent<BoxCollider>().enabled = false;//Makes it so the boss can't take damage 
        bossAi.gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
        return NodeState.SUCCESS;
    }
}
