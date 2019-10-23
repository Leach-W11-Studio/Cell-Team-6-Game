using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navmesh2D : MonoBehaviour
{
    public LayerMask layers;
    public float squareDistance;
    public 

    void Initialize() {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class NavmeshGrid {
    public Bounds boundaries;
    public LayerMask layers;
    public float squareDistance;
    public List<List<NavmeshNode>> grid;

    public NavmeshGrid(LayerMask layers, Bounds boundaries, float squareDistance) {
        this.boundaries = boundaries;
        this.layers = layers;
        this.squareDistance = squareDistance;

        grid = new List<List<NavmeshNode>>();

        int length = Mathf.FloorToInt(squareDistance / (boundaries.max.x - boundaries.min.x));
        int height = Mathf.FloorToInt(squareDistance / (boundaries.max.y - boundaries.min.y));

        for (int x = 0; x < length; x++) {
            for (int y = 0; y < height; y++) {
                Vector2 worldPosition = new Vector2(boundaries.min.x + (squareDistance * x), boundaries.min.y + (squareDistance * y));
                NavmeshNode newNode = new NavmeshNode(worldPosition, new Vector2(x, y));
                grid[x][y] = newNode;

                newNode.neighbors = new List<NavmeshNode>();
            }
        }
    }

    public void UpdateGrid(Bounds checkBounds = new Bounds()) {
        if (checkBounds == new Bounds()) {
            foreach (List<NavmeshNode> row in grid) {
                foreach (NavmeshNode node in row) {
                    node.GetConnections(layers);
                }
            }
        }
    }
}

[System.Serializable]
public class NavmeshNode {
    public Vector2 worldPosition;
    public Vector2 gridPosition;
    public bool navigable;
    public List<NavmeshNode> neighbors;
    public List<NavmeshNode> connections;

    public NavmeshNode() {
        navigable = true;
        worldPosition = Vector2.zero;
        gridPosition = Vector2.zero;
    }

    public NavmeshNode(Vector2 worldPosition, Vector2 gridPosition, bool navigable = true) {
        this.navigable = navigable;
        this.worldPosition = worldPosition;
        this.gridPosition = gridPosition;
    }

    public List<NavmeshNode> GetConnections(LayerMask layers) {
        if (connections != null)
        {
            connections = new List<NavmeshNode>();
        }

        else if (!navigable) {
            connections = new List<NavmeshNode>();
            return connections;
        }

        foreach (NavmeshNode neighbor in neighbors) {
            CheckNavigability(layers);
            if (neighbor.navigable)
            {
                if (!connections.Contains(neighbor))
                {
                    connections.Add(neighbor);
                }
            }
            else {
                if (connections.Contains(neighbor)) {
                    connections.Remove(neighbor);
                }
            }
        }

        return connections;
    }

    public bool CheckNavigability(LayerMask layers) {
        if (Physics2D.OverlapCircleAll(worldPosition, 0f, layers).Length > 0) {
            navigable = false;
        }

        return navigable;
    }
}
