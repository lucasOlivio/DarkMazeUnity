using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DM
{
    public class MummyAI : EnemyAI
    {
        public GridBasedMovement movement;

        void Update()
        {
            if (target == null) return;

            Movement.Direction direction = Movement.Direction.Center;
            float distance = float.MaxValue;

            // Test the 4 cardinal points to check which one is valid and closer to the player then move mummy to it
            foreach (Movement.Direction currentDirection in Movement.Directions)
            {
                if (!movement.TestMovePoint(currentDirection))
                    continue;

                Vector3 futurePosition = movement.movePoint.position + Movement.GetMovement(currentDirection);
                float currentDistance = Vector3.Distance(futurePosition, target.movePoint.position);

                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    direction = currentDirection;
                }
            }

            movement.ChangeMovePoint(direction);
        }
    }
}
