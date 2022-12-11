using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFourBoss : State
{
    BossAI owner;

    private Node topNode;

    float timeRemaining = 10;

    int stageOneWeight;
    int stageTwoWeight;
    int stageThreeWeight;

    public static bool boxSensorSwap = false;

    public StageFourBoss(BossAI owner)//Pulls all information needed
    {
        this.owner = owner;
    }

    public override void Enter()
    {
        owner.transform.position = owner.fleeTarget.position;
        StageOneBehaviourTree();
    }

    public override void Execute()
    {

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            topNode.Evaluate();//Goes throgh the behaviour tree
            if (topNode.nodeState == NodeState.FAILURE) { Debug.Log("All Failure"); }//looks if all nodes failed
        }
        else
        {
            for (int i = 0; i < owner.gameObject.transform.childCount; i++)
            {
                owner.gameObject.transform.GetChild(i).gameObject.SetActive(false);//Set all children to off
            }

            Debug.Log("The HP of the Boss is: " + owner.bossHP);
            if (owner.bossHP >= 20)//Sets the random wight 
            {
                Debug.Log("The HP of the Boss is more than 20");
                stageOneWeight = 4;
                stageTwoWeight = 8;
                stageThreeWeight = 10;
            }
            if (owner.bossHP < 20 && owner.bossHP >= 10)
            {
                Debug.Log("The HP of the Boss is more than 10");
                stageOneWeight = 2;
                stageTwoWeight = 6;
                stageThreeWeight = 10;
            }
            if (owner.bossHP < 10 && owner.bossHP > 0)
            {
                Debug.Log("The HP of the Boss is more than 0");
                stageOneWeight = 1;
                stageTwoWeight = 4;
                stageThreeWeight = 10;
            }

            int randomNum = Random.Range(1, 10);
            Debug.Log("The Random number is: " + randomNum);
            //Applies the random wight and uses the needed stage
            if (randomNum <= stageOneWeight)
            {
                Debug.Log("Using stage one");
                boxSensorSwap = false;
                StageOneBehaviourTree();
                timeRemaining = 10;
            }
            else if (randomNum > stageOneWeight && randomNum <= stageTwoWeight)
            {
                Debug.Log("Using stage two");
                boxSensorSwap = true;
                StageTwoBehaviourTree();
                timeRemaining = 10;
            }
            else if (randomNum > stageTwoWeight && randomNum <= stageThreeWeight)
            {
                Debug.Log("Using stage three");
                boxSensorSwap = false;
                StageThreeBehaviourTree();
                timeRemaining = 10;
            }
        }
    }

    public override void Exit()
    {

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
