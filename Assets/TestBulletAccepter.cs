using UnityEngine;

public class TestBulletAccepter : BulletAccepter
{
    public override void OnHit()
    {
        GameObject.FindFirstObjectByType<Manager>().Kill();

        Destroy(gameObject);
    }
}
