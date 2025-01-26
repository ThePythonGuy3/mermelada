using UnityEngine;
using NavMeshPlus.Extensions;

public class EnemyMover : MonoBehaviour
{
    private AgentOverride2d agentOverride2d;

    [SerializeField] public GameObject player;
    [SerializeField] private float minPlayerDistance = 3;

    public bool isInRange;

    void Start()
    {
        agentOverride2d = GetComponent<AgentOverride2d>();
    }

    void Update()
    {
        Vector3 normalArrow = transform.position - player.transform.position;
        normalArrow.Normalize();
        normalArrow *= minPlayerDistance;

        if (agentOverride2d.Agent.isOnNavMesh)
        {
            agentOverride2d.Agent.destination = player.transform.position + normalArrow;
        }

        if ((int) Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) <= (int) minPlayerDistance)
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
        }
    }
}
