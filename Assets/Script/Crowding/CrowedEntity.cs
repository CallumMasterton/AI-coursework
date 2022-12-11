using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowedEntity : MonoBehaviour
{   //Based on Week 9 Boids and crowding - https://moodle.bcu.ac.uk/course/view.php?id=79511
    public float speed = 1f;
    GameObject player;
    Vector3 playerPos;
    float rotSpeed = 3f;
    float neighbourDistance = 10f;
    bool turning = true;

    NavMeshAgent agent;

    void Awake()//Sets the basic value when the Entity is awakend
    {
        speed = Random.Range(0.5f, 2f);
        player = GameObject.Find("Player");
        agent = this.GetComponent<NavMeshAgent>();
        playerPos = player.transform.position;

    }

    void Update()
    {
        playerPos = GameObject.Find("Player").transform.position;
        agent.SetDestination(playerPos);

        if (Random.Range(0, 5) < 1)
        {
            GameObject[] goShadow;
            goShadow = CrowedManager.shadows;

            Vector3 vcentre = this.transform.position; //cohesion
            Vector3 vavoid = playerPos; //separation

            float gSpeed = 0.0f;
            Vector3 goalPos = CrowedManager.goalPos;

            float dist;
            int groupSize = 0;

            foreach (GameObject go in goShadow)//For each shadow it will set its speed 
            {
                if (go != this.gameObject)//Ignore all but this shadow
                {
                    dist = Vector3.Distance(go.transform.position, transform.position);
                    if (dist <= neighbourDistance)//Cheaks the distance from its neighbours if is less or the same
                    {
                        vcentre += go.transform.position;
                        groupSize++;
                        if (dist < 6f)
                        {
                            vavoid = vavoid + (this.transform.position - go.transform.position); //separation
                        }
                        CrowedEntity anotherCrowedEntity = go.GetComponent<CrowedEntity>();
                        gSpeed = gSpeed + anotherCrowedEntity.speed;


                    }
                }
            }

            if (groupSize >= 0)//Looks at the groupe size
            {
                vcentre = vcentre / groupSize + (goalPos - this.transform.position); //sets the cohesion
                speed = (speed / groupSize) + Random.Range(-0.1f, 0.1f);//adgusts the speed to fit in ebtter
                if (speed > 5f)//If the speed is to much it will be slowed down
                {
                    speed = Random.Range(0.5f, 3f);
                }

                Vector3 direction = (vcentre + vavoid) - transform.position; //sets the alignment
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);//Adjustes the rotation
            }

        }
        transform.Translate(0, 0, Time.deltaTime * speed);//Moves the shadow


    }
}
