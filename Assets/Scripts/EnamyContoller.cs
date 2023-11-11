using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnamyContoller : MonoBehaviour
{
    public float ViewingDistance = 10f;

    public float AttcDistation = 2f;

    public GameObject AttackPoint;

    public float AttackRange = 0.7f;

    public LayerMask PlayerLayer;

    public int AttackCountDownSeconds = 1;

    public int health = 30;

    public ParticleSystem DamageParticle;

    private bool EnableAttack = true;
    private Transform _Target;
    private NavMeshAgent _Agent;
    private Animator _Animator;
    private GameManager _GameManager;

    private float DistanceToPlayer;

    private void Start()
    {
        _Target = GameManager.ManagerInstance.Player.transform;
        _Agent = GetComponent<NavMeshAgent>();
        _Animator = GetComponent<Animator>();
        _GameManager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        DistanceToPlayer = Vector3.Distance(_Target.position, transform.position);

        if(DistanceToPlayer <= ViewingDistance)
        {
            _Agent.SetDestination(_Target.position);
            transform.LookAt(_Target.position);

            if (DistanceToPlayer <= AttcDistation && EnableAttack) StartCoroutine(AttackCountDown());
        }

        if(health <= 0 ) Death();
    }

    public void DealDamage(int Count)
    {
        health -= Count;
        DamageParticle.Play();
    }

    private IEnumerator AttackCountDown()
    {
        EnableAttack = false;
        yield return new WaitForSeconds(AttackCountDownSeconds);

        Attack();
    }

    private void Attack()
    {
        Collider[] HitedColliders = Physics.OverlapSphere(AttackPoint.transform.position,
            AttackRange, PlayerLayer);

        EnableAttack = true;
        
        foreach(Collider HitedCollider in HitedColliders)
        {
            _GameManager.DamagePlayer(10);
            Debug.Log("HP: " + _GameManager.health);
        }
    }

    private void Death()
    {
        DamageParticle.transform.parent = null;
        DamageParticle.Play();

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AttackPoint.transform.position, AttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ViewingDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttcDistation);
    }
}
