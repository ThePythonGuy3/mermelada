using NavMeshPlus.Extensions;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform _player;
    private AgentOverride2d _agentOverride2d;

    public Attack[] attackList;

    /**
     * If set to false, the enemy will be one shoted by the player.
     */
    public bool enemyHaveHealth;

    /**
     * If Its true, this variables are important
     */
    public float enemyMaximumHealth;
    private float _enemyCurrentHealth;

    private void Awake()
    {
        _enemyCurrentHealth = enemyMaximumHealth;
    }

    public void ReceiveAttack(float damage = 0)
    {
        if (enemyHaveHealth)
        {
            _enemyCurrentHealth -= damage;
            if (_enemyCurrentHealth > 0) return;
        }

        Die();
    }

    public Vector3 GetPlayerPosition()
    {
        return _player.position;
    }

    #region ENEMY STATES
    public void Die()
    {
        // manager.kill()
    }
    #endregion
}
