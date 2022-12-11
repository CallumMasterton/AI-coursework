using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    protected Node node;

    public Inverter(Node node)//Fills this with the diffrent nodes
    {
        this.node = node;
    }

    public override NodeState Evaluate()
    {

        switch (node.Evaluate())//Goes throgh each node 
        {
            case NodeState.RUNNING:
                _nodeState = NodeState.RUNNING;//Sets the node to running
                break;
            case NodeState.SUCCESS:
                _nodeState = NodeState.FAILURE;//Sets the node to failure
                break;
            case NodeState.FAILURE:
                _nodeState = NodeState.SUCCESS;//Sets the node to Success
                break;
            default:
                break;
        }
        return _nodeState;

    }
}
