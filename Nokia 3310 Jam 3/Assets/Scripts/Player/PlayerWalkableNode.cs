using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkableNode : MonoBehaviour
{
    //A label attached to all nodes that are walkable.
    //Generate the WorldToTile dictionary using these nodes.

    void Start()
    {
        //ADD NODE TO PLAYER POSITION DICTIONARY
        //POSITION IS SENT IN TERMS OF CELL POSITION
        //Vector3Int nodeTilePos = Tilemap.Instance.playerTileMap.WorldToCell(transform.position);
        //GameManager.Instance.playerPosition.Add(gameObject.name, nodeTilePos);
    }
}
