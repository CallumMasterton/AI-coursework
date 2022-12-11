using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCrowd : MonoBehaviour
{
    public GameObject target;
    public GameObject boss;
    public NavMeshAgent agent;
    Rigidbody rb;
    StateManager stateManager = new StateManager();
    public GameObject wayPointsList;
    public GameObject[] wayPoints;
    bool stateChange = false;
    public static bool onTheHunt = false;

    bool attackPlayer;
    bool stun;
    bool fleeFromBoss = false;
    public float timeRemaining = 2;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        agent = this.GetComponent<NavMeshAgent>();
        rb.isKinematic = true;
        target = GameObject.Find("Player");
        boss = GameObject.Find("Boss");
        wayPointsList = GameObject.Find("WayPoint");
        wayPoints = GameObject.FindGameObjectsWithTag("Waypoint");
        stateManager.ChangeState(new ZPatrol(this));//Sets defult to ZPatrol

    }

    // Update is called once per frame
    void Update()
    {
        stateManager.Update();//Allows the state machine to update
        float dist = Vector3.Distance(target.transform.position, this.transform.position);//Checks teh distance from shadow to player
        float bossDist = Vector3.Distance(boss.transform.position, this.transform.position);//Checks teh distance from shadow to player
        if (bossDist < 5) fleeFromBoss = true;
        else if (bossDist > 5) fleeFromBoss = false;

        if (!fleeFromBoss && stun && !stateChange)
        {
            Debug.Log("ZStun");
            if (timeRemaining > 0)
            {
                GetComponent<MeshRenderer>().material.color = Color.blue;
                stateManager.ChangeState(new ZStun(this));
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                stun = false;
                stateChange = true;
                timeRemaining = 2;
            }
        }
        else if (!attackPlayer && !fleeFromBoss && dist > 10 && AiCrowd.onTheHunt == false && !stateChange)//This changes the state to ZPatrol
        {
            Debug.Log("ZPatrol");
            GetComponent<MeshRenderer>().material.color = Color.green;
            stateManager.ChangeState(new ZPatrol(this));
            stateChange = true;
        }
        else if (!attackPlayer && !fleeFromBoss && dist < 10 || AiCrowd.onTheHunt == true && stateChange)//This changes the state to ZChase
        {
            Debug.Log("ZChase");
            GetComponent<MeshRenderer>().material.color = Color.red;
            stateManager.ChangeState(new ZChase(this));
            stateChange = false;
        }
        else if (fleeFromBoss && !stateChange)
        {
            Debug.Log("ZEvadeBoss");
            GetComponent<MeshRenderer>().material.color = Color.yellow;
            stateManager.ChangeState(new ZEvadeBoss(this));
            stateChange = false;
            onTheHunt = false;
        }
        else if (!fleeFromBoss && attackPlayer && !stateChange)
        {
            Debug.Log("ZAttack");
            GetComponent<MeshRenderer>().material.color = Color.black;
            stateManager.ChangeState(new ZAttack(this));
            onTheHunt = false;
            stateChange = true;
        }
    }


    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hitting the player");
            attackPlayer = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        attackPlayer = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("stun by the player");
            stun = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        stun = false;
    }

}
