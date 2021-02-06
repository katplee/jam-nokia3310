using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerStep : MonoBehaviour
{
    //Contains code which controls the step movement of the player.

    [SerializeField] private Tilemap playerWalkablePath;
    [SerializeField] private Transform step = null;
    private float stepSpeed = 5f;

    void Start()
    {
        step.parent = null;
    }

    public void SetDestination(Vector3Int destination)
    {
        Vector3 playerPositionWorld = transform.position;
        Vector3Int playerPosition = playerWalkablePath.WorldToCell(playerPositionWorld);

        if (Vector3Int.Distance(playerPosition, destination) < 0.05f) { return; }

        if (Vector3.Distance(step.position, transform.position) > 0.05f) { return; }

        Vector3 unitStepVector = DetermineUnitStepVector(playerPosition, destination);
        
        step.position += unitStepVector;
    }

    private Vector3 DetermineUnitStepVector(Vector3Int playerPosition, Vector3Int destination)
    {
        float intermediateDirection = 0f;
        Vector3Int intermediateDistance = destination - playerPosition;

        if (Mathf.Abs(intermediateDistance.x) > Mathf.Abs(intermediateDistance.y))
        {
            intermediateDirection = Mathf.Sign(intermediateDistance.x);
            return new Vector3(intermediateDirection, 0, 0);
        }
        else
        {
            intermediateDirection = Mathf.Sign(intermediateDistance.y);
            return new Vector3(0, intermediateDirection, 0);
        }
    }

    void Update()
    {
        if(Vector3.Distance(step.position, transform.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, step.position, Time.deltaTime * stepSpeed);
        }
    }
}
