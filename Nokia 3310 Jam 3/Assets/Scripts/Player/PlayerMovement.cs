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
    private bool isSetToMove = false;

    [SerializeField] private AStarAlgorithm aStar;

    [SerializeField] private Tilemap playerWalkablePath;
    [SerializeField] private PlayerStep step;
    private Vector3Int startPosition;
    private Vector3Int destination;
    private Stack<Vector3Int> computedPath;
    
    private void Start()
    {
        Vector3 startPositionWorld = transform.position;
        startPosition = playerWalkablePath.WorldToCell(startPositionWorld);
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
        computedPath = aStar.Implement(startPosition, destination);
        DoneComputing?.Invoke();
    }

    private void Update()
    {
        if (!isSetToMove) { return; }

        Vector3 playerPositionWorld = transform.position;
        Vector3Int playerPosition = playerWalkablePath.WorldToCell(playerPositionWorld);

        CheckIfInFinalDestination(destination, playerPosition);
    }

    private void CheckIfInFinalDestination(Vector3Int destination, Vector3Int playerPosition)
    {
        if (Vector3Int.Distance(destination, playerPosition) < 0.1f)
        {
            
        }
        else
        {
            StartStepping(destination, playerPosition);
        }
    }

    private void StartStepping(Vector3Int destination, Vector3Int playerPosition)
    {
        MoveToIntermediateDestination();
    }

    private void MoveToIntermediateDestination()
    {
        throw new NotImplementedException();
    }
}
