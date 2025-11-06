using UnityEngine;

public struct TargetData
{
    public Vector2 TargetPosition;
    public float TargetRadius;
    public Vector2 NextTarget;
    public bool IsSecondTargetSet;

    public TargetData(Vector2 position, float radius, Vector2 nextPosition, bool secondTarget = true)
    {
        TargetPosition = position; 
        TargetRadius = radius;
        NextTarget = nextPosition;
        IsSecondTargetSet = secondTarget;
    }
}
