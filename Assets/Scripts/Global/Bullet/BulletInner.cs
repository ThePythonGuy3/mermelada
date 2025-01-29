using System.Collections;
using UnityEngine;

public class BulletInner : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            transform.parent.gameObject.GetComponent<Bullet>().DestroyBullet();
        }
    }
}
