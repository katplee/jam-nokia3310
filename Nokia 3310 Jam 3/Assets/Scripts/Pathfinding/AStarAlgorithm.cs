using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarAlgorithm : MonoBehaviour
{
    //Credits to inScope Studios for the A* Search algorithm tutorial and implementation.

    [SerializeField] private Tilemap playerWalkablePath;
    private Dictionary<Vector3Int, AStarNode> nodeRegister;
    private Vector3Int startPosition;
    private AStarNode currentNode;
    private Vector3Int destination;

    private HashSet<AStarNode> openList;
    private HashSet<AStarNode> closedList;
    private Stack<Vector3Int> finalPath;

    private bool declarationDone = false;

    //This method will be called from the PlayerMovement script.
    //This object will not be erased and will be attached to the Player object.

    private void Start()
    {
        PlayerMovement.ReadyToCompute += Declaration;
        PlayerMovement.DoneComputing += ClearData;
    }

    private void OnDestroy()
    {
        PlayerMovement.ReadyToCompute -= Declaration;
        PlayerMovement.DoneComputing -= ClearData;
    }

    private void Declaration()
    {
        nodeRegister = new Dictionary<Vector3Int, AStarNode>();
        currentNode = null;

        finalPath = null;

        declarationDone = true;
    }

    public Stack<Vector3Int> Implement(Vector3Int startPosition, Vector3Int destination)
    {
        this.startPosition = startPosition;
        this.destination = destination;

        if (!declarationDone) { return null; }

        if (finalPath != null) { return finalPath; }

        if (currentNode == null)
        {
            Initialize();
        }

        while (openList.Count > 0)
        {
            List<AStarNode> neighborNodes = FindNeighbors(currentNode.Position);

            ExamineNeighbors(neighborNodes, currentNode);

            UpdateCurrentNode(ref currentNode);

            GenerateFinalPath(currentNode);
        }

        return finalPath;
    }

    private void Initialize()
    {
        currentNode = GetRegisteredNode(startPosition);

        openList = new HashSet<AStarNode>();
        openList.Add(currentNode);
        closedList = new HashSet<AStarNode>();
    }

    private AStarNode GetRegisteredNode(Vector3Int nodePosition)
    {
        if (nodeRegister.ContainsKey(nodePosition)) { return nodeRegister[nodePosition]; }

        AStarNode registeredNode = new AStarNode(nodePosition);
        nodeRegister.Add(nodePosition, registeredNode);

        return registeredNode;
    }

    private List<AStarNode> FindNeighbors(Vector3Int parentPosition)
    {
        List<AStarNode> neighborNodes = new List<AStarNode>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int neighborPosition = new Vector3Int(parentPosition.x + i, parentPosition.y + j, parentPosition.z);

                if(neighborPosition == parentPosition) { continue; }

                if (!playerWalkablePath.GetTile(neighborPosition)) { continue; }

                if (i == 0 || j == 0)
                {
                    AStarNode qualifiedNeighbor = GetRegisteredNode(neighborPosition);
                    neighborNodes.Add(qualifiedNeighbor);
                }
            }
        }

        return neighborNodes;
    }

    private void ExamineNeighbors(List<AStarNode> neighborNodes, AStarNode currentNode)
    {
        for (int i = 0; i < neighborNodes.Count; i++)
        {
            AStarNode neighborNode = neighborNodes[i];

            int gScore = CalculateGScore(currentNode.Position, neighborNode.Position);

            if (openList.Contains(neighborNode))
            {
                if (currentNode.G + gScore < neighborNode.G)
                {
                    CalculateValues(currentNode, neighborNode, gScore);
                }
            }
            //if not in the closed list, but is also not yet in the open list
            else if (!closedList.Contains(neighborNode))
            {
                CalculateValues(currentNode, neighborNode, gScore);
                openList.Add(neighborNode);
            }
        }
    }

    private int CalculateGScore(Vector3Int currentNodePosition, Vector3Int neighborNodePosition)
    {
        Vector3Int neighborDistance = currentNodePosition - neighborNodePosition;

        if (Mathf.Abs(neighborDistance.x) + Mathf.Abs(neighborDistance.y) % 2 == 0) { return 14; }
        else { return 10; }
    }

    private void CalculateValues(AStarNode parentNode, AStarNode neighborNode, int cost)
    {
        neighborNode.Parent = parentNode;

        neighborNode.G = parentNode.G + cost;
        neighborNode.H = (Mathf.Abs(destination.x - neighborNode.Position.x) + Mathf.Abs(destination.y - neighborNode.Position.y)) * 10;
        neighborNode.F = neighborNode.G + neighborNode.H;
    }

    private void UpdateCurrentNode(ref AStarNode currentNode)
    {
        openList.Remove(currentNode);
        closedList.Add(currentNode);

        if (openList.Count > 0)
        {
            currentNode = openList.OrderBy(AStarNode => AStarNode.F).First();
        }
    }

    private void GenerateFinalPath(AStarNode currentNode)
    {
        if (currentNode.Position != destination) { return; }
        
        Stack<Vector3Int> path = new Stack<Vector3Int>();

        while (currentNode.Position != startPosition)
        {
            path.Push(currentNode.Position);
            currentNode = currentNode.Parent;
        }

        finalPath = path;

        return;
    }

    private void ClearData()
    {
        nodeRegister = null;
        startPosition = Vector3Int.zero;
        currentNode = null;
        destination = Vector3Int.zero;
        openList = null;
        closedList = null;
        finalPath = null;
        declarationDone = false;        
    }
}
