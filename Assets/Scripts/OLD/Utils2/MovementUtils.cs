using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    public enum Direction
    {
        North,
        East,
        South,
        West,
        Center
    }

    /// <summary>
    /// Calculates the movement vector corresponding to the specified cardinal direction.
    /// </summary>
    /// <param name="direction">The direction to move in (North, East, South, or West).</param>
    public static Vector3 GetMovement(Direction direction)
    {
        return direction switch
        {
            Direction.North => new Vector3(0, 1, 0),
            Direction.East => new Vector3(1, 0, 0),
            Direction.South => new Vector3(0, -1, 0),
            Direction.West => new Vector3(-1, 0, 0),
            Direction.Center => Vector3.zero,
            _ => Vector3.zero
        };
    }
    
    // Cached directions values for easier iteration
    public static readonly Direction[] Directions = (Direction[])System.Enum.GetValues(typeof(Direction));
    
    public static float DirectionToRotation(Movement.Direction direction)
    {
        return direction switch
        {
            Direction.North => 0f,
            Direction.East => -90f,
            Direction.South => 180f,
            Direction.West => 90f,
            _ => 0f
        };
    }
}
