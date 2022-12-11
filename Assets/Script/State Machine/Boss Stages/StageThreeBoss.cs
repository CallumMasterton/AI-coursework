using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageThreeBoss : State
{
    BossAI owner;

    private Node topNode;

    public StageThreeBoss(BossAI owner)//Pulls all information needed
    {
        this.owner = owner;
    }

    public override void Enter()
    {
        StageThreeBehaviourTree();
    }

    public override void Execute()
    {
        topNode.Evaluate();//Goes throgh the behaviour tree
        if (topNode.nodeState == NodeState.FAILURE) { Debug.Log("All Failure"); }//looks if all nodes failed
    }

    public override void Exit()
    {
        owner.gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
        Debug.Log("Moveing on");
    }

    private void StageThreeBehaviourTree()//Creats the Behaviour Tree
    {
        //Runaway
        ResetAttackNode resetAttackNode = new ResetAttackNode(owner.ChargeParticals, owner.aoeAttack, owner.spinningLasers, owner);
        isExposedNode exposedNode = new isExposedNode(owner);
        BeenHitNode beenHitNode = new BeenHitNode(owner);
        BossMoveNode bossRunAwayNode = new BossMoveNode(owner.pFind, owner.fleeTarget, owner.bossPos);//Creats the node for movement
        //Attack nodes
        SlamAttackNode slamAttackNode = new SlamAttackNode(owner);
        ChargeAttackNode runAtPlayer = new ChargeAttackNode(owner);
        ChargeAttackRangeNode bossAttackRangeNode = new ChargeAttackRangeNode(owner);
        //Movement nodes
        IsDoingChargeNode isDoingChargeNode = new IsDoingChargeNode();
        IsInBossRangeMove isInBossRangeMove = new IsInBossRangeMove(owner.bossPos, owner.sightDis, owner.hightMult);
        BossMoveNode bossMoveNode = new BossMoveNode(owner.pFind, owner.Target, owner.bossPos);//Creats the node for movement 
        //Inverter
        Inverter beenHitNodeInver = new Inverter(beenHitNode);
        //Sequncer
        //Runaway Sequncer
        Sequence runawaySequencer = new Sequence(new List<Node> { beenHitNodeInver, bossRunAwayNode });
        Sequence runawaySequencerReset = new Sequence(new List<Node> { resetAttackNode, exposedNode, runawaySequencer });
        //Attack Sequncer
        Sequence doAttacks = new Sequence(new List<Node> { runAtPlayer, slamAttackNode });
        Sequence attckSequence = new Sequence(new List<Node> { bossAttackRangeNode, doAttacks });
        //Move to player Sequncer
        Sequence chaseSequence = new Sequence(new List<Node> { isDoingChargeNode, isInBossRangeMove, exposedNode, bossMoveNode });//Creats the Sequence that will be used

        topNode = new Selector(new List<Node> { runawaySequencerReset, attckSequence, chaseSequence });//Creats the top nodes
    }
}
