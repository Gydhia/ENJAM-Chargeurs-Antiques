using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Room : MonoBehaviour
{
    public RoomType Type;
    public List<Vector3> Orientations;
    public GameObject RoomPrefab;
    
    public int Enemies;
    

    public Room(RoomType Type)
    {
        this.Type = Type;
    }

    public bool IsRoomCompleted()
    {
        return true;
    }
}
