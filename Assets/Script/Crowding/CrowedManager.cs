using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowedManager : MonoBehaviour
{   //Based on Week 9 Boids and crowding - https://moodle.bcu.ac.uk/course/view.php?id=79511
    public GameObject wispPrefab;
    public static int airSize = 100;
    static int numShadows = 200;
    public static GameObject[] shadows = new GameObject[numShadows];
    public static Vector3 goalPos = new Vector3(0, 0, 0);
    public GameObject player;
    Vector3 playPos;

    void Start()
    {
        player = GameObject.Find("Player");

        playPos = player.transform.position;

        goalPos = new Vector3(Random.Range(playPos.x - airSize, playPos.x + airSize),//Sets the postion of the goal target 
                              Random.Range(playPos.y - 10f, playPos.y + airSize),
                              Random.Range(playPos.z - airSize, playPos.z + airSize));
        for (int i = 0; i < numShadows; i++)
        {
            Vector3 pos = new Vector3(playPos.x, playPos.y, playPos.z);
        }
    }

    void Update()
    {
        if (Random.Range(0, 100) < 1)
        {
            goalPos = new Vector3(Random.Range(playPos.x - airSize, playPos.x + airSize), Random.Range(playPos.y + 10f, playPos.y + airSize), Random.Range(playPos.z - airSize, playPos.z + airSize));//updates the postion of the goal target 
        }
    }
}
