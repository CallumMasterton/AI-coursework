using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispFlock : MonoBehaviour
{
    //Flock was made from https://www.youtube.com/watch?v=mBVarJm3Tgk
    [SerializeField] private WispFlockEntity Prefab;
    [SerializeField] private int flockSize;
    [SerializeField] private Vector3 SpawnBounds;

    [Header("Set Speed")]//Seys the basic speeds
    [Range(0, 10)]
    [SerializeField] private float _minSpeed;
    public float minSpeed { get { return _minSpeed; } }
    [Range(0, 10)]
    [SerializeField] private float _maxSpeed;
    public float maxSpeed { get { return _maxSpeed; } }

    [Header("Set Distances")]//Sets the ditance that will be used
    [Range(0, 10)]
    [SerializeField] private float _cohisionDistance;
    public float cohisionDistance { get { return _cohisionDistance; } }
    [Range(0, 10)]
    [SerializeField] private float _avoidanceDistance;
    public float avoidanceDistance { get { return _avoidanceDistance; } }
    [Range(0, 10)]
    [SerializeField] private float _aligementDistance;
    public float aligementDistance { get { return _aligementDistance; } }
    [Range(0, 100)]
    [SerializeField] private float _boundsDistance;
    public float boundsDistance { get { return _boundsDistance; } }

    [Header("SetWeights")]//Sets the random wights 
    [Range(0, 10)]
    [SerializeField] private float _cohisionWeight;
    public float cohisionWeight { get { return _cohisionWeight; } }
    [Range(0, 10)]
    [SerializeField] private float _avoidanceWeight;
    public float avoidanceWeight { get { return _avoidanceWeight; } }
    [Range(0, 10)]
    [SerializeField] private float _aligementWeight;
    public float aligementWeight { get { return _aligementWeight; } }
    [Range(0, 10)]
    [SerializeField] private float _boundsWeight;
    public float boundsWeight { get { return _boundsWeight; } }

    public WispFlockEntity[] allWisps { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        allWisps = new WispFlockEntity[flockSize];
        for (int i = 0; i < flockSize; i++)//Will spawn a equal number of wisps to the flock size
        {
            Vector3 randomVertor = Random.insideUnitSphere;//Gets random vectores in a sphere
            randomVertor = new Vector3(randomVertor.x * SpawnBounds.x, randomVertor.y * SpawnBounds.y, randomVertor.z * SpawnBounds.z);//Creats the area to spawn in
            Vector3 spawnPosition = transform.position + randomVertor;//Gets the spawn postion 
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);//Gets the spawn rotation 
            allWisps[i] = Instantiate(Prefab, spawnPosition, rotation);//Spawns the prefab
            allWisps[i].AssignedFlock(this);//Allocates its flock
            allWisps[i].InitSpeed(Random.Range(minSpeed, maxSpeed));//Sets the speed it can be
        }
    }

    void Update()
    {
        for (int i = 0; i < allWisps.Length; i++) allWisps[i].MoveWisp();//Moves all the units
    }
}
