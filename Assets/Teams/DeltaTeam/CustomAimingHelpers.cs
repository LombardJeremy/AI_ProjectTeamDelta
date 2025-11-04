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
}
