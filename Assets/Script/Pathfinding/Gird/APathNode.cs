using UnityEngine;
using System.Collections;

public class APathNode
{
    //Week 4 and 5 Pathfinding - https://moodle.bcu.ac.uk/course/view.php?id=79511
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public APathNode parent;
    //Defines each node in the list
    public APathNode(bool walkable, Vector3 worldPos, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }
    //Defines the cost and stores it
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
