using UnityEngine;
using NavMeshPlus.Extensions;

public class EnemyMover : MonoBehaviour
{
    private AgentOverride2d _agentOverride2d;

    private Transform _player;
    [SerializeField] private float _minPlayerDistance = 3;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _agentOverride2d = GetComponent<AgentOverride2d>();
    }

    void Update()
    {
        Vector3 normalArrow = transform.position - _player.position;
        normalArrow.Normalize();
        normalArrow *= _minPlayerDistance;

        if (_agentOverride2d.Agent.isOnNavMesh)
        {
            _agentOverride2d.Agent.destination = _player.position + normalArrow;
        }
    }
}
