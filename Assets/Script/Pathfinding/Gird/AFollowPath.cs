using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFollowPath : MonoBehaviour
{
    List<APathNode> thePathRoot;
    public Vector3 startPos;
    public int listSize, nodeCount;
    public int speed = 4;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(thePathRoot[nodeCount].worldPosition);//Looks at the grid space its going to
        gameObject.transform.position += gameObject.transform.forward * speed * Time.deltaTime;//Moves to node
        if (gameObject.transform.position == thePathRoot[nodeCount].worldPosition) nodeCount++;//Moves it onto the next node

    }

    public void GetPathInfo(List<APathNode> thePathRoot, Vector3 startPos)//Brings in all the the data
    {
        this.thePathRoot = thePathRoot;
        this.startPos = startPos;
        listSize = thePathRoot.Count;
    }
}
