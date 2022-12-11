using UnityEngine;
using System.Collections;

public class PathNode
{   //Based on Sebastian Lague - https://www.youtube.com/watch?v=mZfyt03LDH4 and Week 4 and 5 Pathfinding - https://moodle.bcu.ac.uk/course/view.php?id=79511
    public Vector3 worldPosition;
    public PathNode parent;

    public float gCost;
    public float hCost;
    //Sets the world position
    public PathNode(Vector3 _worldPos)
    {
        worldPosition = _worldPos;
    }

    public float fCost//Allows for the cost to be worked out
    {
        get { return gCost + hCost; }
    }
}