using UnityEngine;

public class LightBulletAccepter : BulletAccepter
{
    [SerializeField] private LightController light;

    public override void OnHit()
    {
        light.Hit();
    }
}
