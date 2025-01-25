using UnityEngine;
using System.Collections;


public class Helpers
{
    /// <summary>
    /// Calculates a target position based on an initial position, rotation, and distance parameters.
    /// </summary>
    /// 
    /// <param name="position">The starting position.</param>
    /// <param name="rotation">The rotation (used to determine the direction).</param>
    /// <param name="distance">The distance to the target position.</param>
    /// 
    /// <returns>A new position in the direction of the rotation at the specified max distance.</returns>
    public static Vector3 CalculateTargetPosition(Vector3 position, Quaternion rotation, float distance)
    {
        Vector3 newPosition = Vector3.zero;

        float deg = rotation.eulerAngles.z + 90;
        float rad = deg * Mathf.Deg2Rad;

        newPosition.x = position.x + distance * Mathf.Cos(rad);
        newPosition.y = position.y + distance * Mathf.Sin(rad);

        return newPosition;
    }

    /// <summary>
    /// Smoothly moves a Transform from a start position to a target position over a given duration using an animation curve.
    /// </summary>
    /// 
    /// <param name="transform">The Transform to move.</param>
    /// <param name="start">The starting position.</param>
    /// <param name="target">The target position.</param>
    /// <param name="lerpDuration">The total duration of the movement in seconds.</param>
    /// <param name="curve">The AnimationCurve to control the interpolation.</param>
    public static IEnumerator LerpComplexPosition(Transform transform, Vector3 start, Vector3 target, float lerpDuration, AnimationCurve curve)
    {
        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            float animationEvaluated = curve.Evaluate(timeElapsed / lerpDuration);
            transform.position = Vector3.Lerp(start, target, animationEvaluated);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = target;
    }
}
