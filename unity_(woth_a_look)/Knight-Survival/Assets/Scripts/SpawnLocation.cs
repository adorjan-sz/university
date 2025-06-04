using System;
using UnityEngine;

class SpawnLocation {
    public enum SpawnDirection
    {
        NORTH,
        WEST,
        EAST,
        SOUTH
    }

    public static SpawnDirection GetRandomDirection() {
        Array directions =  Enum.GetValues(typeof(SpawnDirection));

       return (SpawnDirection) directions.GetValue(UnityEngine.Random.Range(0, directions.Length));
    
    }

    public static SpawnDirection[] GetDirections() { 
    
        return (SpawnDirection[])Enum.GetValues(typeof(SpawnDirection));
    }


}

