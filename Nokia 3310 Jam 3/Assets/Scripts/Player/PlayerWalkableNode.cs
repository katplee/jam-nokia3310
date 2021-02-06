using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerWalkableNode : MonoBehaviour
{
    //A label attached to all nodes that are walkable.
    //Generate the DestinationToTransform dictionary using these nodes.

    [SerializeField] private Tilemap playerWalkablePath;
    [SerializeField] private PlayerWalkableNodeList walkableNodeList;

    private void Start()
    {
        //Add node to player position dictionary
        //Position is sent in terms of cell position

        Vector3Int nodePosition = playerWalkablePath.WorldToCell(transform.position);
        walkableNodeList.destinationToTransform.Add(gameObject.name, nodePosition);           
    }
}
