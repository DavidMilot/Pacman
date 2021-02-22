using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Obstacle : MonoBehaviour
{
    public enum Block
    {
        Empty,
        Wall,
        Door,
        House,
        PowerUp

    }

    public Block GameObstacle = Block.Wall;
}
