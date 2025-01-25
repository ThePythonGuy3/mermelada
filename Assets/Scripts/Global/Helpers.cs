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
