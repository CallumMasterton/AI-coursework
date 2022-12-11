using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Behavoir tree scripts were all based of https://www.youtube.com/watch?v=F-3nxJ2ANXg
[System.Serializable]
public abstract class Node
{
    protected NodeState _nodeState;//Stores the node state

    public NodeState nodeState { get { return _nodeState; } }//Alows the node states to be set

    public abstract NodeState Evaluate();//Creats the evaluation function
}
public enum NodeState
{
    RUNNING, SUCCESS, FAILURE,//Creats the diffrent states if the tree
}