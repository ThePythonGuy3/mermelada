using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // [SerializeField] private int _damage = 10;
    public float timeHealthToAdd = 10f;

    [SerializeField] private float _bulletMaxDistance = 5.0f;

    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _animationDuration;

    // Time to wait after movement animation is completed
    [SerializeField] private float _timeWaitEnd = 5f;

    private bool bulletCanHeal = false;

    public BulletPool BulletPool;

    #region BULLET MOVEMENT
    public void StartBulletMovement()
    {
        bulletCanHeal = false;

        Vector3 start = transform.position;
        Quaternion rotation = transform.rotation;

        Vector3 target = Helpers.CalculateTargetPosition(start, rotation, _bulletMaxDistance);

        StartCoroutine(BulletMovement(start, target));
    }

    IEnumerator BulletMovement(Vector3 start, Vector3 target)
    {
        yield return StartCoroutine(Helpers.LerpComplexPosition(transform, start, target, _animationDuration, _animationCurve));
        bulletCanHeal = true;
        yield return StartCoroutine(WaitInPosition(_timeWaitEnd));
        DestroyBullet();

        yield return null;
    }

    IEnumerator WaitInPosition(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            DestroyBullet();
        }
        else if (other.CompareTag("BubbleTank"))
        {
            Tank tank = other.gameObject.GetComponent<Tank>();
            tank.DestroyTank();
        }
        else if (other.CompareTag("Enemy"))
        {
            DestroyBullet();
            BulletAccepter a = other.GetComponent<BulletAccepter>();
            if (a != null) a.OnHit();
        }
    }

    public bool BulleteCanHeal()
    {
        return bulletCanHeal;
    }

    public void DestroyBullet()
    {
        BulletPool.ReturnBulletToPool(gameObject);
    }
}
