using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace DM
{
    public class LOMovement
    {
        public static int2 RandomPositionAround(int centerX, int centerY, int minDistance, int maxDistance)
        {
            int2 position;

            int angle = LORandom.RandomRange(0, 359);
            int distance = LORandom.RandomRange(minDistance, maxDistance);

            float angleRad = angle * MathInfo.DEG2RAD;

            float offsetX = LOMath.Cos(angleRad) * distance;
            float offsetY = LOMath.Sin(angleRad) * distance;

            int2 offset = new int2((int)offsetX, (int)offsetY);
            position = new int2(centerX + offset[0], centerY + offset[1]);

            return position;
        }

        public static float QuaternionToYRotation(Quaternion rotation)
        {
            float yRotation = rotation.eulerAngles.y;

            // Normalize to [0, 360)
            return (yRotation % 360 + 360) % 360;
        }

        public static Direction GetDirection(int ax, int ay, int bx, int by)
        {
            int dx = bx - ax;
            int dy = by - ay;

            // Normalize direction to -1, 0, or 1 for x and y
            int nx = Math.Sign(dx);
            int ny = Math.Sign(dy);

            if (nx == 1 && ny == 0) return Direction.EE;
            if (nx == 1 && ny == 1) return Direction.SE;
            if (nx == 0 && ny == 1) return Direction.SS;
            if (nx == -1 && ny == 1) return Direction.SW;
            if (nx == -1 && ny == 0) return Direction.WW;
            if (nx == -1 && ny == -1) return Direction.NW;
            if (nx == 0 && ny == -1) return Direction.NN;
            if (nx == 1 && ny == -1) return Direction.NE;

            // Point A == Point B
            return Direction.EE;
        }

        public static Direction GetDirection(float rotation)
        {
            int rounded = Mathf.RoundToInt(rotation) % 360;
            rounded = (rounded + 360) % 360;

            return rounded switch
            {
                0 => Direction.NN,
                45 => Direction.NE,
                90 => Direction.EE,
                135 => Direction.SE,
                180 => Direction.SS,
                225 => Direction.SW,
                270 => Direction.WW,
                315 => Direction.NW,
                _ => Direction.NN // Default or error case
            };
        }

        public static Direction GetDirection(Quaternion rotation)
        {
            float yRotation = QuaternionToYRotation(rotation);

            int rounded = Mathf.RoundToInt(yRotation / 45f) * 45 % 360;

            return GetDirection(rounded);
        }

        public static int2 GetMovement(Direction direction)
        {
            return direction switch
            {
                Direction.NN => new int2(0, 1),
                Direction.NE => new int2(1, 1),
                Direction.EE => new int2(1, 0),
                Direction.SE => new int2(1, -1),
                Direction.SS => new int2(0, -1),
                Direction.SW => new int2(-1, -1),
                Direction.WW => new int2(-1, 0),
                Direction.NW => new int2(-1, 1),
                _ => new int2(0, 0)
            };
        }

        public static float DirectionToRotation(Direction direction)
        {
            return direction switch
            {
                Direction.NN => 0f,
                Direction.NE => 45f,
                Direction.EE => 90f,
                Direction.SE => 135f,
                Direction.SS => 180f,
                Direction.SW => 225f,
                Direction.WW => 270f,
                Direction.NW => 315f,
                _ => 0f
            };
        }
        
        // Cached directions values for easier iteration
        public static readonly Direction[] Directions = (Direction[])System.Enum.GetValues(typeof(Direction));

        public static Quaternion GetOpenRotation(int x, int y)
        {
            foreach (Direction direction in Directions)
            {
                int2 offset = GetMovement(direction);
                int checkX = x + offset.x;
                int checkY = y - offset.y; // Inverted Y in maps construction

                if (SimulationManager.Instance.mapSystem.IsWalkable(checkX, checkY))
                {
                    float angle = DirectionToRotation(direction);
                    return Quaternion.Euler(0, 0, angle);
                }
            }

            return Quaternion.identity; // No open tiles? Keep default rotation.
        }
    }
}
