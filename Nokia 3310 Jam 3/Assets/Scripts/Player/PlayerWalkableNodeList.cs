using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkableNodeList : MonoBehaviour
{
    //Include conversion from place of destination to tile position
    //Input will be a string of text representing the place of destination,
    //while the output will be a Vector3 containing the tile position.

    public Dictionary<string, Vector3Int> destinationToTransform = new Dictionary<string, Vector3Int>();  
}
