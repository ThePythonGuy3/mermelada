using System;

public class Easing
{
    public static float EaseCamera(float transition)
    {
        return -((float) Math.Cos(Math.PI * transition) - 1f) / 2f;
    }
}
