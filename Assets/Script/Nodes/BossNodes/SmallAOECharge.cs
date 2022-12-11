using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAOECharge : Node
{
    public BossAI bossAi;
    private GameObject smallAOE;
    private GameObject particalsCharge;
    private float timeRemaining = 0.5f;
    float fireTimeRemaining = 0.5f;
    float defulatTime = 0.5f;
    AudioSource BAS;
    AudioClip sEffect;
    bool playSounds = false;
    // Start is called before the first frame update
    public SmallAOECharge(GameObject particalsCharge, GameObject smallAOE, BossAI bossAi)//Brings the transform of the gameobject into the script
    {
        this.particalsCharge = particalsCharge;
        this.particalsCharge.SetActive(false);
        this.smallAOE = smallAOE;
        this.smallAOE.SetActive(false);
        this.bossAi = bossAi;
        BAS = bossAi.GetComponent<AudioSource>();
        sEffect = bossAi.basicAttack;
        //this.timeRemaining = defulatTime;
    }

    public override NodeState Evaluate()
    {
        bossAi.gameObject.GetComponent<BoxCollider>().enabled = true;
        if (timeRemaining > 0)//Turns on partical effect while chargeing and set up a cycle for attacking with timers
        {
            timeRemaining -= Time.deltaTime;
            particalsCharge.SetActive(true);
            smallAOE.SetActive(false);
            if (!playSounds)
            {
                BAS.PlayOneShot(sEffect);
                playSounds = true;
            }
            fireTimeRemaining = 2;
        }
        else
        {
            if (fireTimeRemaining > 0)
            {
                fireTimeRemaining -= Time.deltaTime;
                Debug.Log("It is charged");
                particalsCharge.SetActive(false);
                return NodeState.SUCCESS;//Sets this node to success
            }
            else
            {
                playSounds = false;
                timeRemaining = defulatTime;
            }

        }

        return NodeState.FAILURE;//Sets this node to fail

    }
}
