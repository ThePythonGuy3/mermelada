using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Tilemaps;


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

        Vector3 target = CalculateTargetPosition(start, rotation, _bulletMaxDistance);

        StartCoroutine(BulletMovement(start, target));
    }

    private Vector3 CalculateTargetPosition(Vector3 position, Quaternion rotation, float distance)
    {
        Vector3 newPosition = Vector3.zero;

        float deg = rotation.eulerAngles.z + 90;
        float rad = deg * Mathf.Deg2Rad;

        newPosition.x = position.x + _bulletMaxDistance * Mathf.Cos(rad);
        newPosition.y = position.y + _bulletMaxDistance * Mathf.Sin(rad);

        return newPosition;
    }

    IEnumerator BulletMovement(Vector3 start, Vector3 target)
    {
        yield return StartCoroutine(LerpPosition(start, target, _animationDuration));
        bulletCanHeal = true;
        yield return StartCoroutine(WaitInPosition(_timeWaitEnd));
        DestroyBullet();

        yield return null;
    }

    IEnumerator LerpPosition(Vector3 start, Vector3 target, float lerpDuration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            float animationEvaluated = _animationCurve.Evaluate(timeElapsed / lerpDuration);
            transform.position = Vector3.Lerp(start, target, animationEvaluated);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = target;
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
        /*else if (other.CompareTag(""))
        {
            // TODO
        }
        /*else if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            DestroyBullet();
            // other.GetComponent<Character>().LoseHealth(_damage);
        }*/
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
