using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackCountNode : Node
{
    public BossAI bossAi;
    private GameObject deathBeam;
    private GameObject particalsCharge;
    AudioSource BAS;
    AudioClip sEffect;
    private float timeRemaining;
    float fireTimeRemaining = 2;
    float defulatTime;
    bool playSounds = false;
    // Start is called before the first frame update
    public ChargeAttackCountNode(GameObject particalsCharge, GameObject deathBeam, BossAI bossAi)//Brings the transform of the gameobject into the script
    {
        timeRemaining = bossAi.timeRemaining;
        defulatTime = timeRemaining;
        this.particalsCharge = particalsCharge;
        this.particalsCharge.SetActive(false);
        this.deathBeam = deathBeam;
        this.deathBeam.SetActive(false);
        this.bossAi = bossAi;
        BAS = bossAi.GetComponent<AudioSource>();
        sEffect = bossAi.basicAttack;
        //this.timeRemaining = defulatTime;
    }

    public override NodeState Evaluate()
    {
        bossAi.gameObject.GetComponent<BoxCollider>().enabled = true;//Allows the boss to take damage 
        if (timeRemaining > 0)//Setup the charging effect 
        {
            bossAi.bossPos.LookAt(bossAi.Target);
            //Debug.Log(timeRemaining);
            timeRemaining -= Time.deltaTime;
            if (!playSounds)
            {
                BAS.PlayOneShot(sEffect);
                playSounds = true;
            }
            particalsCharge.SetActive(true);
            deathBeam.SetActive(false);
            fireTimeRemaining = 2;
        }
        else//When timer is done it will start anothe timer but start the laser firing
        {
            if (fireTimeRemaining > 0)
            {
                fireTimeRemaining -= Time.deltaTime;
                //Debug.Log(fireTimeRemaining);
                //BAS.enabled = false;
                playSounds = false;
                Debug.Log("It is charged");
                particalsCharge.SetActive(false);
                return NodeState.SUCCESS;//Sets this node to success
            }
            else
            {
                timeRemaining = defulatTime;//Restarts the timer
            }

        }

        return NodeState.FAILURE;//Sets this node to fail

    }
}
