using System.Collections.Generic;

public class Sequence : Node
{
    protected List<Node> nodes = new List<Node>();//creats a list for the nodes

    public Sequence(List<Node> nodes)//Fills this with the diffrent nodes
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        bool isAnyNodeRunning = false;
        foreach (var node in nodes)//Goes throgh each node 
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING://Sets the node to running
                    isAnyNodeRunning = true;
                    break;
                case NodeState.SUCCESS://Sets the node to Success
                    break;
                case NodeState.FAILURE://Sets the node to failure
                    _nodeState = NodeState.FAILURE;//Tells the tree that this nodes state is a failure
                    return _nodeState;
                default:
                    break;
            }
        }
        _nodeState = isAnyNodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;//Sets a condtion to tell the tree that this is ether running or success 
        return _nodeState;
    }
}
