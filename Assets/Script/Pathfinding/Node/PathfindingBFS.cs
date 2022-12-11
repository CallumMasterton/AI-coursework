using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingBFS : MonoBehaviour
{

    public GameObject nodeHolder;
    PathNode[] pNodes;
    public List<PathNode> travelNodes;

    public float maxDistance = 10.0f;
    public float maxHight = 4f;

    public List<Vector3> links;
    public List<Vector3> from_pos;

    void Update()
    {
        //Drawns all the connecting lines
        int i = 0;
        foreach (Vector3 link in links)//Draws a line between each node that has been checked
        {
            Debug.DrawLine(from_pos[i], from_pos[i] + link, Color.green);
            i++;
        }
    }

    //works out and creats the path to the player thoght the nodes that are bnxt to eachother
    public void FindPathBFS(Vector3 startPos, Vector3 targetPos)
    {   //Sets the first and last node
        PathNode startNode = ClosestNodeFinder(startPos);
        PathNode endtNode = ClosestNodeFinder(targetPos);
        //Sets the nodes that it hasnt looked at and nodes it has into a queue and a list
        Queue<PathNode> frontier = new Queue<PathNode>();
        List<PathNode> explored = new List<PathNode>();

        frontier.Enqueue(startNode);
        while (frontier.Count != 0)//While there are still nodes in the frontire 
        {
            PathNode pathNode = frontier.Dequeue();
            explored.Add(pathNode);

            if (pathNode == endtNode)
            {
                RetracePath(startNode, endtNode);
                return;
            }

            foreach (PathNode neighbour in GetTheNeighbours(pathNode))//cycle throgh each of the nodes on the frontier and looking if it has any frontier neighbours
            {
                if (explored.Contains(neighbour))
                {
                    continue;
                }
                if (!frontier.Contains(neighbour))
                {
                    frontier.Enqueue(neighbour);
                    neighbour.parent = pathNode;
                }
            }
        }
    }
    //Creats the list of nodes
    public void ListOfNodes()
    {
        int numberOfNondes = nodeHolder.transform.childCount;
        pNodes = new PathNode[numberOfNondes];
        for (int i = 0; i < numberOfNondes; i++)
        {
            pNodes[i] = new PathNode(nodeHolder.transform.GetChild(i).gameObject.transform.position);
        }
    }

    //This finds the closest node to the gameobject
    PathNode ClosestNodeFinder(Vector3 startPos)
    {
        PathNode pNode = new PathNode(Vector3.zero);
        float closenode = float.MaxValue;
        foreach (PathNode n in pNodes)
        {
            if (closenode > Vector3.Distance(startPos, n.worldPosition))
            {
                pNode = n;
                closenode = Vector3.Distance(startPos, n.worldPosition);
            }
        }
        return pNode;
    }

    //This works out the path
    void RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        travelNodes = path;
    }
    //Works out the Neighbours of each node
    List<PathNode> GetTheNeighbours(PathNode pNode)
    {
        List<PathNode> neighbours = new List<PathNode>();
        foreach (PathNode node1 in pNodes)
        {
            if (node1 != pNode)
            {
                float dist = Vector3.Distance(node1.worldPosition, pNode.worldPosition);
                if (dist < maxDistance)
                {
                    Vector3 edge = pNode.worldPosition - node1.worldPosition;
                    bool cliff = false;
                    int steps = Mathf.FloorToInt(edge.magnitude);
                    for (int i = 0; i < steps; i++)
                    {
                        Vector3 pos = node1.worldPosition + edge.normalized * i;
                        RaycastHit hit;
                        if (Physics.Raycast(pos, Vector3.down, out hit, maxDistance))//Stops the nodes from seeing each other if a wall or cliff is in the way
                        {
                            if (hit.distance > maxHight) cliff = true;
                        }
                    }
                    if (!Physics.Raycast(node1.worldPosition, edge, maxDistance) && !cliff)//Allows the nodes to see eachother
                    {
                        neighbours.Add(node1);
                        links.Add(edge);
                        from_pos.Add(node1.worldPosition);
                    }
                }
            }
        }
        return neighbours;
    }
}
