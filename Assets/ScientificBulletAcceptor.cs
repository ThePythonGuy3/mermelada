using UnityEngine;
using UnityEngine.AI;

public class ScientificBulletAcceptor : BulletAccepter
{
    public override void OnHit()
    {
        GameObject.FindFirstObjectByType<Manager>().Kill();
        GameObject.FindFirstObjectByType<NavMeshAgent>().enabled = false;

        Destroy(gameObject);
    }
}
