using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    public int nbOfFloors = 3;
    public int floorSqrSize = 4;
    public int nbOfEnemiesRoom = 8;
    public int nbOfRewardRoom = 2;

    public static readonly Vector3[] Directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

    public Room[,] Rooms;

    private LayerMask _floorMask;
    private LayerMask _wallMask;

    public struct Location
    {
        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x;
        public int y;
    }

    private void Start()
    {
        Rooms = new Room[floorSqrSize, floorSqrSize];

        Location spawnLoc = GetBaseRoom(RoomType.Spawn);
        Rooms[spawnLoc.x, spawnLoc.y] = new Room(RoomType.Spawn);

        Location bossLoc = GetBaseRoom(RoomType.Boss);
        Rooms[bossLoc.x, bossLoc.y] = new Room(RoomType.Boss);


    }

    public Location GetBaseRoom(RoomType type)
    {
        if (type != RoomType.Spawn || type != RoomType.Boss)
            throw new System.Exception("Wrong RoomType asked");

        int wLocation = Random.Range(0, floorSqrSize);

        return new Location(
            wLocation,
            type == RoomType.Spawn ? 0 : floorSqrSize 
            );
    }

    public bool GenerateNextRoom(Location location)
    {
        List<Location> avLocation = GetAvailableDirections(location);
        Location? nextLocation = null;
        if (avLocation.Count > 0)
            nextLocation = avLocation[Random.Range(0, avLocation.Count)];

        

        if (nextLocation != null)
             return GenerateNextRoom(nextLocation.Value);
        else
            return false;
    }

    public void RandomiseRoom(ref Room room)
    {

    }

    public List<Location> GetAvailableDirections(Location loc, bool includeBtm = false)
    {
        List<Location> availableDir = new List<Location>();

        if (loc.y + 1 < floorSqrSize && Rooms[loc.x + 1, loc.y] == null)
            availableDir.Add(new Location(loc.x + 1, loc.y));
        if (loc.y - 1 >= 0 && Rooms[loc.x - 1, loc.y] == null)
            availableDir.Add(new Location(loc.x - 1, loc.y));
        if (loc.y + 1 < floorSqrSize && Rooms[loc.x, loc.y + 1] == null)
            availableDir.Add(new Location(loc.x, loc.y + 1));
        if (loc.y - 1 >= 0 && Rooms[loc.x, loc.y - 1] == null && includeBtm)
            availableDir.Add(new Location(loc.x, loc.y - 1));

        return availableDir;
    }
}
