using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOneBoss : State
{
    BossAI owner;

    private Node topNode;

    public StageOneBoss(BossAI owner)//Pulls all information needed
    {
        this.owner = owner;
    }

    public override void Enter()
    {
        StageOneBehaviourTree();
    }

    public override void Execute()
    {
        topNode.Evaluate();//Goes throgh the behaviour tree
        if (topNode.nodeState == NodeState.FAILURE) { Debug.Log("All Failure"); }//looks if all nodes failed
    }

    public override void Exit()
    {
        Debug.Log("Moveing on");
    }

    private void StageOneBehaviourTree()//Creats the Behaviour Tree
    {
        //Runaway
        ResetAttackNode resetAttackNode = new ResetAttackNode(owner.ChargeParticals, owner.aoeAttack, owner.spinningLasers, owner);
        isExposedNode exposedNode = new isExposedNode(owner);
        BeenHitNode beenHitNode = new BeenHitNode(owner);
        BossMoveNode bossRunAwayNode = new BossMoveNode(owner.pFind, owner.fleeTarget, owner.bossPos);//Creats the node for movement
        //Attack nodes
        ChargeAttackCountNode chargeAttackCountNode = new ChargeAttackCountNode(owner.ChargeParticals, owner.deathBeam, owner);
        BossAttack bossAttack = new BossAttack(owner.deathBeam, owner);
        BossAttackRangeNode bossAttackRangeNode = new BossAttackRangeNode(owner.bossPos, owner.sightDis, owner.hightMult, owner);
        //Movement nodes
        IsInBossRangeMove isInBossRangeMove = new IsInBossRangeMove(owner.bossPos, owner.sightDis, owner.hightMult);
        BossMoveNode bossMoveNode = new BossMoveNode(owner.pFind, owner.Target, owner.bossPos);//Creats the node for movement 
        //Inverter
        Inverter beenHitNodeInver = new Inverter(beenHitNode);
        //Sequncer
        //Runaway Sequncer
        Sequence runawaySequencer = new Sequence(new List<Node> { beenHitNodeInver, bossRunAwayNode });
        Sequence runawaySequencerReset = new Sequence(new List<Node> { resetAttackNode, exposedNode, runawaySequencer });
        //Attack Sequncer
        Sequence chargeAttack = new Sequence(new List<Node> { beenHitNode, chargeAttackCountNode });
        Sequence fireAttack = new Sequence(new List<Node> { chargeAttack, bossAttack });
        Sequence attckSequence = new Sequence(new List<Node> { bossAttackRangeNode, fireAttack });
        //Move to player Sequncer
        Sequence chaseSequence = new Sequence(new List<Node> { isInBossRangeMove, exposedNode, bossMoveNode });//Creats the Sequence that will be used

        topNode = new Selector(new List<Node> { runawaySequencerReset, attckSequence, chaseSequence });//Creats the top nodes
    }

}
