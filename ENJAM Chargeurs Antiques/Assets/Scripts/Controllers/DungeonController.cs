using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DungeonController : MonoBehaviour
{
    public int nbOfFloors = 3;
    public int floorSqrSize = 4;

    public int EnemiesRooms = 8;
    public int RemainingEnemiesRooms;

    public int BRewardRooms = 1;
    public int RemainingBRewardRooms;
    public int ARewardRooms = 1;
    public int RemainingARewardRooms;

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

    public void Start()
    {
        RemainingEnemiesRooms = EnemiesRooms;
        RemainingARewardRooms = ARewardRooms;
        RemainingBRewardRooms = BRewardRooms;

        Rooms = new Room[floorSqrSize, floorSqrSize];

        Location spawnLoc = GetBaseRoom(RoomType.Spawn);
        Rooms[spawnLoc.x, spawnLoc.y] = new Room(RoomType.Spawn);

        Location bossLoc = GetBaseRoom(RoomType.Boss);
        Rooms[bossLoc.x, bossLoc.y] = new Room(RoomType.Boss);

        GenerateNextRoom(spawnLoc);

        for (int i = 0; i < RemainingARewardRooms + RemainingBRewardRooms + RemainingEnemiesRooms; i++)
        {

        }

        // To randomize rooms
        for (int i = 0; i < floorSqrSize; i++)
        {
            for (int j = 0; j < floorSqrSize; j++)
            {

            }
        }
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

        if(avLocation.Any(room => Rooms[room.x, room.y].Type == RoomType.Boss)) {
            return false;
        }

        Location? nextLocation = null;
        if (avLocation.Count > 0)
            nextLocation = avLocation[Random.Range(0, avLocation.Count)];

        RoomType? nextType = GetAvailableType();
        if (!nextType.HasValue) {
            return false;
        }

        Rooms[nextLocation.Value.x, nextLocation.Value.y] = new Room(nextType.Value);

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

        if (loc.y + 1 < floorSqrSize && Rooms[loc.x + 1, loc.y] == null || Rooms[loc.x + 1, loc.y].Type == RoomType.Boss)
            availableDir.Add(new Location(loc.x + 1, loc.y));

        if (loc.y - 1 >= 0 && Rooms[loc.x - 1, loc.y] == null || Rooms[loc.x + 1, loc.y].Type == RoomType.Boss)
            availableDir.Add(new Location(loc.x - 1, loc.y));

        if (loc.y + 1 < floorSqrSize && Rooms[loc.x, loc.y + 1] == null || Rooms[loc.x + 1, loc.y].Type == RoomType.Boss)
            availableDir.Add(new Location(loc.x, loc.y + 1));

        if (loc.y - 1 >= 0 && Rooms[loc.x, loc.y - 1] == null && includeBtm)
            availableDir.Add(new Location(loc.x, loc.y - 1));

        return availableDir;
    }

    public RoomType? GetAvailableType()
    {
        RoomType? returnedType = null;

        if (RemainingBRewardRooms == 0 && RemainingARewardRooms == 0 && RemainingEnemiesRooms == 0)
            return returnedType;

        while (returnedType == null)
        {
            switch (Random.Range(0, 3))
            {
                case 1:
                    RemainingBRewardRooms--;
                    returnedType = RoomType.BuffReward;
                    break;
                case 2:
                    RemainingARewardRooms--;
                    returnedType = RoomType.AbilityReward;
                    break;
                case 3:
                    RemainingEnemiesRooms--;
                    returnedType = RoomType.Enemies;
                    break;
            }
        }

        return returnedType;
    }
}
