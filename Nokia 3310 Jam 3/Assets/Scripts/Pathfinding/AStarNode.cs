using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    //Credits to inScope Studios for the A* Search algorithm tutorial and implementation.

    public int F { get; set; }
    public int G { get; set; }
    public int H { get; set; }

    public AStarNode Parent { get; set; }

    public Vector3Int Position { get; set; }

    public AStarNode(Vector3Int position)
    {
        Position = position;
    }
}
