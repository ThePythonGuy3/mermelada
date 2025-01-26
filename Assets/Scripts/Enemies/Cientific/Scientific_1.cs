using System;
using System.Collections;
using UnityEngine;

public class Scientific_1 : EnemyController
{
    [SerializeField] private ScientificBullet _scientificBullet;
    private Animator _scientistAnimatior;
    private EnemyMover _enemyMover;
    private ScientificAnimationManager _animationManager;

    private float _attackTimer;

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
        _scientistAnimatior = gameObject.GetComponentInChildren<Animator>();
        _animationManager = gameObject.GetComponentInChildren<ScientificAnimationManager>();
    }

    void Update()
    {
        if (_enemyMover.isInRange)
        {
            _scientistAnimatior.SetBool("isMoving", false);

            if (_attackTimer > attackList[0].timerSeconds)
            {
                _attackTimer = 0;
                _animationManager.AnimationAttackHaveStarted();
                _scientistAnimatior.SetBool("isShooting", true);

                StartCoroutine(WaitUntilAnimationFinishes());
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
        yield return new WaitUntil(() => !_animationManager.IsAnimationAttackRunning());

        attackList[0].Run();
        _scientistAnimatior.SetBool("isShooting", false);
    }

    private void Shoot()
    {
        Vector3 pos = player.transform.position;

        float angle = Mathf.Atan2(transform.position.y - pos.y, transform.position.x - pos.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        Instantiate(_scientificBullet, transform.position, rotation, transform);
    }
}
