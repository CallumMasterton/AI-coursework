using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{

    public LayerMask unwalkableAreaMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    APathNode[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()//Sets up the size of the gird for nav
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
    }

    void FixedUpdate()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new APathNode[gridSizeX, gridSizeY];//Sets the size
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;//Works out the the bottom left of the grid

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)//Creats each grid section and works out if it is walkable
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableAreaMask));
                grid[x, y] = new APathNode(walkable, worldPoint, x, y);
            }
        }
    }

    public List<APathNode> GetNeighbours(APathNode node)
    {
        List<APathNode> neighbours = new List<APathNode>();

        for (int x = -1; x <= 1; x++)//Goes thorgh each node and works out its neighbours
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) neighbours.Add(grid[checkX, checkY]);
            }
        }

        return neighbours;
    }


    public APathNode NodeFromWorldPoint(Vector3 worldPosition)//Takes a X and y and works out its location on the grid
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<APathNode> path;
    void OnDrawGizmos()//Draws cubes in the grids and highlights the ones used to navigate
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (APathNode n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}