using UnityEngine;

public class TraitorBulletAccepter : BulletAccepter
{
    public override void OnHit()
    {
        GetComponent<TraitorController>().Hit();
    }
}
