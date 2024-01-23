using UnityEngine;
using System;

public static class DistanceCalculator
{
    public static float CalculateDistance(Vector3 origin, Vector3 destination)
    {
        origin.y = 0f;
        origin.z = 0f;
        destination.y = 0f;
        destination.z = 0f;

        float clampedXDistance  = origin.x - destination.x;
     
        return Math.Abs(clampedXDistance);
    }
}
