using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingAGrid : MonoBehaviour
{

    public Transform targetHolder;
    public Transform[] targets;
    AFollowPath aFollowPath;
    public Transform seeker, target;
    Grid grid;

    void Awake()
    {
        aFollowPath = seeker.GetComponent<AFollowPath>();
        grid = GetComponent<Grid>();
        targets = new Transform[targetHolder.transform.childCount];//Sets array size
        targets = targetHolder.GetComponentsInChildren<Transform>();//fills array

    }

    void Update()
    {
        float dist = Vector3.Distance(seeker.position, target.position);//Works out distance to target
        if (dist < 4) target = targets[Random.Range(0, targetHolder.transform.childCount)];//When seeker gets close to the target it will change the target

        FindPath(seeker.position, target.position);//defines the seeker and the target position
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {   //Sets the start and target location onto the grid
        APathNode startNode = grid.NodeFromWorldPoint(startPos);
        APathNode targetNode = grid.NodeFromWorldPoint(targetPos);

        List<APathNode> openNodes = new List<APathNode>();//filled with the unused Nods
        HashSet<APathNode> closedodes = new HashSet<APathNode>();//filled with the used Nods
        openNodes.Add(startNode);

        while (openNodes.Count > 0)
        {
            APathNode node = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++)//uses the fcost to work out if the node needs to be open
            {
                if (openNodes[i].fCost < node.fCost || openNodes[i].fCost == node.fCost)
                {
                    if (openNodes[i].hCost < node.hCost) node = openNodes[i];
                }
            }

            openNodes.Remove(node);//Removes useds nodes
            closedodes.Add(node);//Adds the used nodes

            if (node == targetNode)//works out the path if the node is equal to target
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (APathNode neighbour in grid.GetNeighbours(node))
            {
                if (!neighbour.walkable || closedodes.Contains(neighbour)) continue;//Stops the node from being used if its unwalkable

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);//Gets the new cost for the Neighbour
                if (newCostToNeighbour < neighbour.gCost || !openNodes.Contains(neighbour))//Works out the costs for current node
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openNodes.Contains(neighbour)) openNodes.Add(neighbour);//If there is no Neighbour the node is added back to the openNodes
                }
            }
        }
    }

    void RetracePath(APathNode startNode, APathNode endNode)//Works out the path to the target
    {
        List<APathNode> path = new List<APathNode>();
        APathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
        aFollowPath.GetPathInfo(path, startNode.worldPosition);//Sends needed data to movement scripts

    }

    int GetDistance(APathNode nodeA, APathNode nodeB)//Gets the distance from node A to node B 
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }
}
