using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    protected List<Node> nodes = new List<Node>();//creats a list for the nodes

    public Selector(List<Node> nodes)//Fills this with the diffrent nodes
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (var node in nodes)//Goes throgh each node 
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING://Sets the node to running
                    _nodeState = NodeState.RUNNING;//Tells the tree that this nodes state is a running
                    return _nodeState;
                case NodeState.SUCCESS://Sets the node to Success
                    _nodeState = NodeState.SUCCESS;//Tells the tree that this nodes state is a Success
                    return _nodeState;
                case NodeState.FAILURE://Sets the node to failure
                    break;
                default:
                    break;
            }
        }
        _nodeState = NodeState.FAILURE;//Tells the tree that this nodes state is a failure
        return _nodeState;
    }
}
