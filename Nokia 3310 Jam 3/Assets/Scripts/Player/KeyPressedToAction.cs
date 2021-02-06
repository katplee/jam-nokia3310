using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPressedToAction : MonoBehaviour
{
    //Will contain list of corresponding methods which contain player actions.

    public static event Action<Vector3Int> MovePlayer;

    [SerializeField] private PlayerWalkableNodeList nodeList;

    private void Start()
    {   
        GameManager.ActionKeyPress += ActionCheck;
    }

    private void OnDestroy()
    {
        GameManager.ActionKeyPress -= ActionCheck;
    }

    private void ActionCheck(KeyCode actionKey)
    {
        switch (actionKey)
        {
            //Tasks relating to the bed
            case KeyCode.Q:
                MovePlayer?.Invoke(GoTo("Bed"));
                break;

            default:
                Debug.Log($"You just pressed {actionKey}!");
                break;
        }
    }

    private Vector3Int GoTo(string destination)
    {
        Vector3Int transform = nodeList.destinationToTransform[destination];

        return transform;
    }

    public void Testing()
    {
        MovePlayer?.Invoke(new Vector3Int(-7, -1, 0));
    }
}
