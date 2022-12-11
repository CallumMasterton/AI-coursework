using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispFlockEntity : MonoBehaviour
{
    //Flock was made from https://www.youtube.com/watch?v=mBVarJm3Tgk
    [SerializeField] private float FOVAngle;
    [SerializeField] private float smoothDamp;

    private List<WispFlockEntity> cohesionNeigbours = new List<WispFlockEntity>();
    private List<WispFlockEntity> avoidanceNeigbours = new List<WispFlockEntity>();
    private List<WispFlockEntity> aligmentNeigbours = new List<WispFlockEntity>();

    private WispFlock assignedWispFlock;
    private Vector3 currentVolcity;
    float speed;

    public Transform wispTransform { get; set; }

    private void Awake()
    {
        wispTransform = transform;
    }

    public void AssignedFlock(WispFlock wispFlock)//Brings in the flock information
    {
        assignedWispFlock = wispFlock;
    }

    public void InitSpeed(float speed)//Brings in the speed information
    {
        this.speed = speed;
    }

    public void MoveWisp()//Moves the wisp in relation to the rest of the flock
    {
        FindNeighbours();//Finds the neighbours
        CalculateSpeed();//Calculates the Speed
        //Works out the Vectors
        Vector3 cohesionVector = CalculateCohesionVector() * assignedWispFlock.cohisionWeight;
        Vector3 avoidanceVector = CalculateAvoidanceVector() * assignedWispFlock.avoidanceDistance;
        Vector3 aligemntVector = CalculateAligemntVector() * assignedWispFlock.aligementDistance;
        Vector3 boundsVector = CalculateBoundVector() * assignedWispFlock.boundsWeight;
        //calculates the move vector
        Vector3 moveVector = cohesionVector + avoidanceVector + aligemntVector + boundsVector;
        moveVector = Vector3.SmoothDamp(wispTransform.forward, moveVector, ref currentVolcity, smoothDamp);
        moveVector = moveVector.normalized * speed;
        if (moveVector == Vector3.zero) moveVector = transform.forward;

        wispTransform.forward = moveVector;
        wispTransform.position += moveVector * Time.deltaTime;
    }

    private Vector3 CalculateCohesionVector()//This will work out how close the wisp has to stay by it nigbours 
    {
        Vector3 cohesionVector = Vector3.zero;
        if (cohesionNeigbours.Count == 0) return cohesionVector;

        int neigbourInFOV = 0;
        for (int i = 0; i < cohesionNeigbours.Count; i++)
        {
            if (IsInFOV(cohesionNeigbours[i].wispTransform.position))//Effects only the the whips in the FOV
            {
                neigbourInFOV++;
                cohesionVector += cohesionNeigbours[i].wispTransform.forward;
            }
        }

        cohesionVector /= neigbourInFOV;
        cohesionVector -= wispTransform.position;
        cohesionVector = cohesionVector.normalized;
        return cohesionVector;
    }

    private Vector3 CalculateAvoidanceVector()//This will caluclate how to avoid other wisps so that they do not collide 
    {
        Vector3 avoidanceVector = Vector3.zero;
        if (avoidanceNeigbours.Count == 0) return Vector3.zero;

        int neighboursInFOV = 0;
        for (int i = 0; i < avoidanceNeigbours.Count; i++)
        {
            if (IsInFOV(avoidanceNeigbours[i].wispTransform.position))//Effects only the the whips in the FOV
            {
                neighboursInFOV++;
                avoidanceVector += (wispTransform.position - avoidanceNeigbours[i].wispTransform.position);
            }
        }

        avoidanceVector /= neighboursInFOV;
        avoidanceVector = avoidanceVector.normalized;
        return avoidanceVector;
    }

    private Vector3 CalculateAligemntVector()//This will calculate the direction each nigbour moves in ether simaler ones or diffrent ones
    {
        Vector3 aligementVector = wispTransform.forward;
        if (aligmentNeigbours.Count == 0) return wispTransform.forward;

        int neighboursInFOV = 0;
        for (int i = 0; i < aligmentNeigbours.Count; i++)
        {
            if (IsInFOV(aligmentNeigbours[i].wispTransform.position))//Effects only the the whips in the FOV
            {
                neighboursInFOV++;
                aligementVector += aligmentNeigbours[i].wispTransform.position;
            }
        }

        aligementVector /= neighboursInFOV;
        aligementVector = aligementVector.normalized;
        return aligementVector;
    }

    private Vector3 CalculateBoundVector()//Works out the movement space the wisps can move in
    {
        Vector3 centerOffset = assignedWispFlock.transform.position - wispTransform.position;
        bool isNearCenter = centerOffset.magnitude >= assignedWispFlock.boundsDistance * 0.9f;
        return isNearCenter ? centerOffset.normalized : Vector3.zero;

    }

    private void CalculateSpeed()//This works out the speed the wisp needs to go in relation to the rest of the flock 
    {
        if (cohesionNeigbours.Count == 0) return;
        speed = 0;
        for (int i = 0; i < cohesionNeigbours.Count; i++) speed += cohesionNeigbours[i].speed;

        speed /= cohesionNeigbours.Count;
        speed = Mathf.Clamp(speed, assignedWispFlock.minSpeed, assignedWispFlock.maxSpeed);
    }

    private void FindNeighbours()//This will look at all wisps in a locl area and set them to a list
    {
        cohesionNeigbours.Clear();
        aligmentNeigbours.Clear();
        avoidanceNeigbours.Clear();

        WispFlockEntity[] allWisps = assignedWispFlock.allWisps;
        for (int i = 0; i < allWisps.Length; i++)
        {
            WispFlockEntity currentUnit = allWisps[i];
            if (currentUnit != this)
            {
                float currentNighbourDistance = Vector3.SqrMagnitude(currentUnit.transform.position - transform.position);
                if (currentNighbourDistance <= assignedWispFlock.cohisionDistance * assignedWispFlock.cohisionDistance)//Checks the currnt distance of the nighbours for the cohesion and if the nighbours distance are smaller or equal
                {
                    cohesionNeigbours.Add(currentUnit);
                }
                if (currentNighbourDistance <= assignedWispFlock.avoidanceDistance * assignedWispFlock.avoidanceDistance)//Checks the currnt distance of the nighbours for the avoidance and if the nighbours distance are smaller or equal
                {
                    avoidanceNeigbours.Add(currentUnit);
                }
                if (currentNighbourDistance <= assignedWispFlock.aligementDistance * assignedWispFlock.aligementDistance)//Checks the currnt distance of the nighbours for the aligment and if the nighbours distance are smaller or equal
                {
                    aligmentNeigbours.Add(currentUnit);
                }
            }
        }
    }

    private bool IsInFOV(Vector3 position)//Sets the Field of view (FOV)
    {
        return Vector3.Angle(wispTransform.forward, position - wispTransform.position) <= FOVAngle;
    }
}
