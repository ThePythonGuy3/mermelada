using UnityEngine;
using NavMeshPlus.Extensions;

public class EnemyMover : MonoBehaviour
{
    private AgentOverride2d agentOverride2d;

    [SerializeField] private GameObject player;
    [SerializeField] private float minPlayerDistance = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agentOverride2d = GetComponent<AgentOverride2d>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 normalArrow = transform.position - player.transform.position;
        normalArrow.Normalize();
        normalArrow *= minPlayerDistance;

        if (agentOverride2d.Agent.isOnNavMesh) agentOverride2d.Agent.destination = player.transform.position + normalArrow;
    }
}
