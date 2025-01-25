using System;
using System.Collections;
using UnityEngine;

public class Scientific_1 : EnemyController
{
    [SerializeField] private ScientificBullet _scientificBullet;
    [SerializeField] private Animator _scientistAnimatior;

    private EnemyMover _enemyMover;

    private float _attackTimer;

    private bool _isAnimationAttackRunning = false;

    private void Awake()
    {
        attackList = new Attack[1];

        attackList[0] = new Attack();
        attackList[0].isTrigger = false;
        attackList[0].timerSeconds = 3;
        attackList[0].Run = () =>
        {
            Shoot();
        };

        _enemyMover = gameObject.GetComponent<EnemyMover>();
    }

    void Update()
    {
        if (_enemyMover.isInRange)
        {
            _scientistAnimatior.SetBool("isMoving", false);

            if (_attackTimer > attackList[0].timerSeconds)
            {
                _attackTimer = 0;

                _scientistAnimatior.SetBool("isShooting", true);
                _isAnimationAttackRunning = true;

                StartCoroutine(WaitUntilAnimationFinishes());

                /*
                attackList[0].Run();
                _attackTimer = 0;

                _scientistAnimatior.SetBool("isShooting", false);*/
            }
        }
        else
        {
            _scientistAnimatior.SetBool("isMoving", true);
        }

        _attackTimer += Time.deltaTime;

    }

    IEnumerator WaitUntilAnimationFinishes()
    {
        
        yield return new WaitUntil(() => !_isAnimationAttackRunning);

        attackList[0].Run();
        _scientistAnimatior.SetBool("isShooting", false);
    }

    public void AnimationAttackHaveFinished()
    {
        Debug.Log("aaa");
        _isAnimationAttackRunning = false;
    }

    private void Shoot()
    {
        Vector3 pos = player.transform.position;

        float angle = Mathf.Atan2(transform.position.y - pos.y, transform.position.x - pos.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        Instantiate(_scientificBullet, transform.position, rotation, transform);
    }
}
