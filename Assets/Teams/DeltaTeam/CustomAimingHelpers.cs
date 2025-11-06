using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;

public static class CustomAimingHelpers
{
    
    public static float ComputeSteeringOrient(SpaceShipView spaceship, Vector2 directionnalVector, float overshootFactor = 1.2f)
    {
        float deltaAngle = Vector2.SignedAngle(spaceship.Velocity, directionnalVector);
        deltaAngle *= overshootFactor;
        deltaAngle = Mathf.Clamp(deltaAngle, -170, 170);
        float velocityOrientation = Vector2.SignedAngle(Vector2.right, spaceship.Velocity);
        return velocityOrientation + deltaAngle;
    }

    public static Vector2 ClampPositionToGridSize(Vector2 position)
    {
        position.x = Mathf.Clamp(position.x, -10, 10);
        position.y = Mathf.Clamp(position.y, -6, 6);
        return position;
    }

    public static bool IsTargetBehindSpaceship(SpaceShipView spaceShip, Vector2 targetPosition)
    {
        Vector2 right;
        float x = Vector2.right.x;
        float y = Vector2.right.y;
        float radOritentation = spaceShip.Orientation * Mathf.Deg2Rad;
        right.x = x * Mathf.Cos(radOritentation) - y * Mathf.Sin(radOritentation);
        right.y = y * Mathf.Cos(radOritentation) + x * Mathf.Sin(radOritentation);

        if (Vector2.Dot(right, spaceShip.Position - targetPosition) > 0)
        {
            return true;
        }
        return false;
    }
}
