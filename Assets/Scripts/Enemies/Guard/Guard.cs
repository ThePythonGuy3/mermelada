using System;
using System.Collections;
using UnityEngine;

public class Guard : EnemyController
{
    private Animator _guardAnimatior;
    private EnemyMover _enemyMover;
    private GuardAnimationManager _animationManager;

    private float _attackTimer;
    [SerializeField] private float _damage;

    private void Awake()
    {
        attackList = new Attack[1];

        attackList[0] = new Attack();
        attackList[0].isTrigger = false;
        attackList[0].timerSeconds = 3;
        attackList[0].Run = () =>
        {
            HandAttack();
        };

        _enemyMover = gameObject.GetComponent<EnemyMover>();
        _guardAnimatior = gameObject.GetComponentInChildren<Animator>();
        _animationManager = gameObject.GetComponentInChildren<GuardAnimationManager>();
    }

    void Update()
    {
        if (_enemyMover.isInRange)
        {
            _guardAnimatior.SetBool("isMoving", false);

            if (_attackTimer > attackList[0].timerSeconds)
            {
                _attackTimer = 0;
                _animationManager.AnimationAttackHaveStarted();
                _guardAnimatior.SetBool("isAttacking", true);

                StartCoroutine(WaitUntilAnimationFinishes());
            }
        }
        else
        {
            _guardAnimatior.SetBool("isMoving", true);
        }

        _attackTimer += Time.deltaTime;

    }

    IEnumerator WaitUntilAnimationFinishes()
    {
        attackList[0].Run();
        yield return new WaitUntil(() => !_animationManager.IsAnimationAttackRunning());
        _guardAnimatior.SetBool("isAttacking", false);
    }

    private void HandAttack()
    {
        player.GetComponent<Player>()._playerHealth.AddToMaxTimeHealth(- _damage);
    }
}
