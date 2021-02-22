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
        PowerUp,
        Start
    }

    [SerializeField]private Block _gameObstacle = Block.Wall;

    public Block GameObstacle
    {
        get
        {
            return _gameObstacle;
        }
    }

}
