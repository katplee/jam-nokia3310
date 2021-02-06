using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    //Contains code that gives movement to the player.
    //Leads player to the destination depending on the key/squence of keys pressed.

    public static event Action ReadyToCompute;
    public static event Action DoneComputing;
    [SerializeField] private bool isSetToMove = false;

    [SerializeField] private AStarAlgorithm aStar;

    [SerializeField] private Tilemap playerWalkablePath;
    [SerializeField] private PlayerStep step;
    private Vector3Int currentPlayerPosition; //covers the starting position of the player
    private Vector3Int destination;
    private Stack<Vector3Int> computedPath;

    private void Start()
    {        
        KeyPressedToAction.MovePlayer += SetDestination;
    }

    private void OnDestroy()
    {
        KeyPressedToAction.MovePlayer -= SetDestination;
    }

    private void SetDestination(Vector3Int destination)
    {
        isSetToMove = true;
        this.destination = destination;
        ComputePathToDestination();
    }

    private void ComputePathToDestination()
    {
        ReadyToCompute?.Invoke();
        computedPath = aStar.Implement(currentPlayerPosition, destination);
        DoneComputing?.Invoke();
    }

    private void Update()
    {
        if (!isSetToMove) { return; }

        Vector3 playerPositionWorld = transform.position;
        currentPlayerPosition = playerWalkablePath.WorldToCell(playerPositionWorld);

        CheckIfInFinalDestination(destination, currentPlayerPosition);
    }

    private void CheckIfInFinalDestination(Vector3Int destination, Vector3Int playerPosition)
    {
        if (Vector3Int.Distance(destination, playerPosition) < 0.1f)
        {
            isSetToMove = false;
        }
        else
        {
            MoveToIntermediateDestination(destination, playerPosition);
        }
    }

    private void MoveToIntermediateDestination(Vector3Int destination, Vector3Int playerPosition)
    {
        if (computedPath == null) { return; }

        if (computedPath.Count == 0) { return; }

        if (Vector3Int.Distance(playerPosition, computedPath.Peek()) < 0.05)
        {
            computedPath.Pop();
        }

        step.SetDestination(computedPath.Peek());
    }
}
