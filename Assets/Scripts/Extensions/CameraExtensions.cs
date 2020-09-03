using UnityEngine;

public static class CameraExtensions
{
    public static bool IsObjectVisible(this Camera @this, Renderer renderer)
    {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(@this), renderer.bounds);
    }
}
