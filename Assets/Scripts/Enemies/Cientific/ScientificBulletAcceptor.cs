using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ScientificBulletAcceptor : BulletAccepter
{
    [SerializeField] private GameObject _scientistSprite;
    [SerializeField] private GameObject _scientistShadow;

    private Animator _scientistAnimatior;

    private bool alive = true;

    private void Awake()
    {
        _scientistAnimatior = _scientistSprite.GetComponent<Animator>();
    }

    public override void OnHit()
    {
        if (!alive) return;

        alive = false;

        GetComponent<BoxCollider2D>().enabled = false;

        GameObject.FindFirstObjectByType<Manager>().Kill();

        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<Scientific_1>().enabled = false;

        _scientistShadow.SetActive(true);
        _scientistSprite.GetComponent<SpriteMover>().enabled = true;

        _scientistAnimatior.SetBool("hasDead", true);

        StartCoroutine(DestroyScientific());
    }

    IEnumerator DestroyScientific()
    {
        yield return new WaitForSeconds(4);

        Destroy(gameObject);
    }
}
