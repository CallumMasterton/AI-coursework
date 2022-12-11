using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTwoBoss : State
{
    BossAI owner;

    private Node topNode;

    public static bool stage2Active = false;

    public static bool boxSensorSwap = false;

    public StageTwoBoss(BossAI owner)//Pulls all information needed
    {
        this.owner = owner;
    }

    public override void Enter()
    {
        boxSensorSwap = true;//Changes the sensore to a box raycast
        stage2Active = true;
        owner.transform.position = owner.fleeTarget.position;
        StageTwoBehaviourTree();
    }

    public override void Execute()
    {
        topNode.Evaluate();//Goes throgh the behaviour tree
        if (topNode.nodeState == NodeState.FAILURE) { Debug.Log("All Failure"); }//looks if all nodes failed
    }

    public override void Exit()
    {
        boxSensorSwap = false;//Changes the sensore to a box raycast
        Debug.Log("Moveing on");
    }

    private void StageTwoBehaviourTree()//Creats the Behaviour Tree
    {
        //Runaway
        ResetAttackNode resetAttackNode = new ResetAttackNode(owner.ChargeParticals, owner.aoeAttack, owner.spinningLasers, owner);
        isExposedNode exposedNode = new isExposedNode(owner);
        BeenHitNode beenHitNode = new BeenHitNode(owner);
        BossMoveNode bossRunAwayNode = new BossMoveNode(owner.pFind, owner.fleeTarget, owner.bossPos);//Creats the node for movement
        //Spin Attack node
        PlayerNearBossNode playerNearBossNode = new PlayerNearBossNode(owner);
        SpinAttackNode spinAttackNode = new SpinAttackNode(owner);
        //Attack nodes
        SmallAOECharge smallAOECharge = new SmallAOECharge(owner.ChargeParticals, owner.aoeAttack, owner);
        SmallAOENode smallAOENode = new SmallAOENode(owner.aoeAttack);
        //Movement nodes
        IsInCenterNode isInCenterNode = new IsInCenterNode();
        BossMoveNode bossMoveNode = new BossMoveNode(owner.pFind, owner.centerNode, owner.bossPos);//Creats the node for movement 
        //Inverter
        Inverter beenHitNodeInver = new Inverter(beenHitNode);
        //Sequncer
        //Runaway Sequncer
        Sequence runawaySequencer = new Sequence(new List<Node> { beenHitNodeInver, bossRunAwayNode });
        Sequence runawaySequencerReset = new Sequence(new List<Node> { resetAttackNode, exposedNode, runawaySequencer });
        //Attack Sequncer
        Sequence basicAttckSequence = new Sequence(new List<Node> { smallAOECharge, smallAOENode });
        Sequence spinAttackSequence = new Sequence(new List<Node> { playerNearBossNode, spinAttackNode });
        Selector attackChoice = new Selector(new List<Node> { spinAttackSequence, basicAttckSequence });//Cecks if ether one is a succuess
        //Move to player Sequncer
        Sequence getToCenterSquncer = new Sequence(new List<Node> { isInCenterNode, bossMoveNode });//Creats the Sequence that will be used

        topNode = new Selector(new List<Node> { runawaySequencerReset, getToCenterSquncer, attackChoice });//Creats the top nodes
    }
}
