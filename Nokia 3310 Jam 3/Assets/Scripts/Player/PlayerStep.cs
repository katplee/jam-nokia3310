using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStep : MonoBehaviour
{
    //Contains code which controls the step movement of the player.

    [SerializeField] private Transform step = null;
    private float stepSpeed = 5f;

    void Start()
    {
        step.parent = null;
    }

    public void SetDestination(Vector3 destination)
    {        
        if (Vector3.Equals(transform.position, destination)) { return; }

        if (Vector3.Distance(step.position, transform.position) > 0.05f) { return; }

        Vector3 unitStepVector = DetermineUnitStepVector();
        
        step.position += unitStepVector;
    }

    private Vector3 DetermineUnitStepVector()
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        if(Vector3.Distance(step.position, transform.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, step.position, Time.deltaTime * stepSpeed);
        }
    }
}
