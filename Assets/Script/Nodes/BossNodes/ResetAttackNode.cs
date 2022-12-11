using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAttackNode : Node
{
    BossAI bossAI;
    private GameObject smallAOE;
    private GameObject spinningLaser;
    private GameObject particalsCharge;
    GameObject bossObject;

    bool isChiledActive = false;

    // Start is called before the first frame update
    public ResetAttackNode(GameObject particalsCharge, GameObject smallAOE, GameObject spinningLaser, BossAI bossAI)//Brings the transform of the gameobject into the script
    {
        this.particalsCharge = particalsCharge;
        this.smallAOE = smallAOE;
        this.spinningLaser = spinningLaser;
        this.bossAI = bossAI;
        //this.timeRemaining = defulatTime;
    }

    public override NodeState Evaluate()
    {
        for (int i = 0; i < bossAI.gameObject.transform.childCount; i++)//Cheacks the children of the boss to see if any are active
        {
            if (bossAI.gameObject.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                isChiledActive = true;
            }
        }

        if (BossAI.beenHit == true && isChiledActive == true)//If the boss has been hit it wiil turn off all it children 
        {
            for (int i = 0; i < bossAI.gameObject.transform.childCount; i++)
            {
                bossAI.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < bossAI.gameObject.transform.childCount; i++)//Restes the check when done
            {
                if (!bossAI.gameObject.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    isChiledActive = false;
                }
            }

            for (int i = 0; i < bossAI.gameObject.transform.childCount; i++)//turns off all the bosses children
            {
                bossAI.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            return NodeState.FAILURE;
        }

        return NodeState.SUCCESS;
    }

}
