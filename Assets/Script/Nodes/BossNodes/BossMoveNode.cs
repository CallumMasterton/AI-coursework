using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveNode : Node
{
    private PathfindingBFS pFind;
    private Transform target;
    private Transform origin;
    private Vector3 movement;
    private int incraNodes = 0;
    private float speed = 5.0f;
    bool fin = false;
    public BossMoveNode(PathfindingBFS pFind, Transform target, Transform origin)
    {
        this.pFind = pFind;
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        origin.LookAt(target);
        pFind.ListOfNodes();//Gets the list of nodes
        pFind.FindPathBFS(origin.position, target.position);//Sets the loction of the of the start and end node
        Vector3 dir = movement - origin.position;
        origin.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        if (!fin)//Moves from node to node
        {
            movement = pFind.travelNodes[incraNodes].worldPosition;
            if (Vector3.Distance(origin.position, movement) < 0.5f)
            {
                incraNodes++;
            }
            if (incraNodes >= pFind.travelNodes.Count)
            {
                fin = true;
            }
        }

        return NodeState.RUNNING;//Sets this node to running
    }
}