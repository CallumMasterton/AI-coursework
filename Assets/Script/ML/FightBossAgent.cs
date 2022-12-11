using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;


public class FightBossAgent : Agent
{
    //This code is based of this video https://www.youtube.com/watch?v=zPFU30tbyKs
    public GameObject targetTransform;
    //public GameObject floor;

    public float speed = 5;
    Rigidbody rb;

    public override void OnEpisodeBegin()//When the Ai restarts from hitting the wall it will set the tartgs random pos and rest set its locatio to the middle
    {
        targetTransform.transform.localPosition = new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6));
        transform.localPosition = Vector3.zero;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 5;
    }

    public override void CollectObservations(VectorSensor sensor)//Gets the Agents onservations
    {
        // Agent location sensor
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(targetTransform.transform.localPosition);

        //Agent velocity sensor
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.z);

    }
    public override void OnActionReceived(float[] vectorAction)//Runs the actions
    {
        MoveAgent(vectorAction);//Calls the movemnt
    }

    public override void Heuristic(float[] actionsOut)//Used to test the movemt manulaly 
    {   //Takes key input and uses that instaed of the Agents input
        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 2;
        }
    }

    public void MoveAgent(float[] vectorActio)//Allows the agent to move
    {
        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;

        float action = vectorActio[0];
        switch (action)//Sets the movemnt and rotation of the agent using its inputs
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
        }
        //Applies the movement and rotation
        transform.Rotate(rotateDir, Time.deltaTime * 150f);
        rb.velocity = dirToGo * speed;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall"))//Punshise the Agent for touching the walls 
        {
            AddReward(-10);
            GetComponent<MeshRenderer>().material.color = Color.red;
            EndEpisode();
        }
        if (col.gameObject.CompareTag("Reward"))//Reweds the agent for toching the target
        {
            AddReward(5);
            targetTransform.transform.localPosition = new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6));
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
}
