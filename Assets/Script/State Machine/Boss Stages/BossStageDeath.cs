using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageDeath : State
{
    BossAI owner;

    public BossStageDeath(BossAI owner)//Pulls all information needed
    {
        this.owner = owner;
    }

    public override void Enter()//Turns the boss off
    {
        owner.gameObject.GetComponent<Rigidbody>().useGravity = true;
        owner.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        owner.gameObject.GetComponent<BoxCollider>().enabled = true;
        owner.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        owner.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }

}
