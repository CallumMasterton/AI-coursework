using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    StateManager stateManager = new StateManager();

    public PathfindingBFS pFind;

    public GameObject TargetObject;
    public GameObject ChargeParticals;
    public GameObject deathBeam;
    public GameObject deathBomb;
    public GameObject spinningLasers;
    public GameObject aoeAttack;
    public GameObject zombiePrefab;
    public GameObject centerNodeObject;

    public Transform Target;
    public Transform bossPos;
    public Transform fleeTarget;
    public Transform fleeStagePoint;
    public Transform zombieSpawn;
    public Transform centerNode;

    public int bossHP = 100;
    public float sightDis = 5f;
    public float hightMult = 0f;
    public static bool beenHit = false;
    public static bool atCenterNode = false;
    public float timeRemaining = 2;
    public LayerMask hitMask;
    Collider col;

    public int bossCurrentStage = 0;
    public int fleeHP;
    bool stateChange = false;

    public int numberOfEnemys;
    float zTimerRemaining = 1f;
    bool beenCharged = false;
    public static Vector3 instantVelocity;
    public static bool justZomSpawned = false;

    public AudioClip basicAttack;
    public AudioClip hurtSound;

    public Slider HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        bossCurrentStage = 1;
        fleeHP = 75;
        bossPos = this.transform;
        col = GetComponent<Collider>();
        stateManager.ChangeState(new StageOneBoss(this));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        RestFleePoint();
        CenterPoint();
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("enemy");
        numberOfEnemys = enemys.Length;

        stateManager.Update();//Updates the cuurent state
        if (bossHP > 75 && !stateChange)//Triggers the first stage
        {
            Debug.Log("Stage 1");
            //stateManager.ChangeState(new StageThreeBoss(this));
            stateManager.ChangeState(new StageOneBoss(this));
            bossCurrentStage++;
            stateChange = !stateChange;
        }
        if (bossHP == fleeHP && stateChange)//Triggers the flee stage
        {
            if (bossCurrentStage == 2 || bossCurrentStage == 4 || bossCurrentStage == 6 && stateChange)
            {
                stateManager.ChangeState(new BossFleeStage(this));
                justZomSpawned = true;
                for (int i = 0; i < 10; i++)//Spawns the mobs
                {
                    if (zTimerRemaining > 0)
                    {
                        zTimerRemaining -= Time.deltaTime;
                    }
                    else
                    {
                        GameObject.Instantiate(zombiePrefab, zombieSpawn.position, Quaternion.identity);
                        zTimerRemaining = 1f;
                    }
                }

                if (numberOfEnemys == 10 && stateChange)
                {
                    justZomSpawned = false;
                    bossCurrentStage++;
                    fleeHP -= 25;
                    stateChange = !stateChange;
                }
            }
        }
        if (numberOfEnemys <= 5 && bossCurrentStage == 3 && !stateChange)//Triggers the second stage
        {
            Debug.Log("Stage 2");
            stateManager.ChangeState(new StageTwoBoss(this));
            bossCurrentStage++;
            stateChange = !stateChange;
        }
        if (numberOfEnemys <= 5 && bossCurrentStage == 5 && !stateChange)//Triggers the third stage
        {
            Debug.Log("Stage 3");
            stateManager.ChangeState(new StageThreeBoss(this));
            bossCurrentStage++;
            stateChange = !stateChange;
        }
        if (numberOfEnemys <= 5 && bossCurrentStage == 7 && !stateChange)//Triggers the forth stage
        {
            Debug.Log("Stage 4");
            stateManager.ChangeState(new StageFourBoss(this));
            bossCurrentStage++;
            stateChange = !stateChange;
        }
        if (bossHP <= 0)//triggers the Death stage
        {
            Debug.Log("Death");
            stateManager.ChangeState(new BossStageDeath(this));
        }

        HealthBar.value = bossHP;
        instantVelocity = transform.position - pos;//Calculates velocity
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Weapon"))//Cheacks if the boss has been hit by the player
        {
            bossHP -= 5;
            Debug.Log("Boss been hit");
            beenHit = true;
            gameObject.GetComponent<AudioSource>().PlayOneShot(hurtSound);
        }
    }
    void OnTriggerExit(Collider othercol)
    {
        atCenterNode = false;
    }

    void RestFleePoint()//Cheacks if the boss has reached the flee point 
    {
        float tempDist = Vector3.Distance(this.transform.position, fleeTarget.position);
        if (tempDist < 0.5f && beenHit == true)
        {
            beenHit = false;
        }
    }
    void CenterPoint()//Cheacks if the boss has reached the center node
    {
        if (StageTwoBoss.stage2Active == true || StageFourBoss.boxSensorSwap == true)
        {
            float tempDist = Vector3.Distance(this.transform.position, centerNode.position);
            if (tempDist < 0.5f && beenHit == false)
            {
                Debug.Log("At the middle");
                atCenterNode = true;
            }
            else if (tempDist > 0.5f && beenHit == true)
            {
                Debug.Log("Not at the middle");
                atCenterNode = false;
            }
        }
    }
}
