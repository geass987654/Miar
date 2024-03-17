using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Chasing,
        Attacking
    }

    [SerializeField] private float changeRoamingDircTime = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private float chaseRange= 12f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private State state;
    private EnemyPathFinding enemyPathFinding;
    private Vector2 roamPosition;
    private Vector2 chasePosition;
    private float elapsedTime = 0f;
    private bool canAttack = true;

    private void Awake()
    {
        state = State.Roaming;
        enemyPathFinding = GetComponent<EnemyPathFinding>();
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch(state)
        {
            case State.Roaming:
                Roaming();
                break;

            case State.Attacking:
                Attacking();
                break;

            case State.Chasing:
                Chasing();
                break;

            default:
                break;
        }
    }

    private void Roaming()
    {
        elapsedTime += Time.deltaTime;

        enemyPathFinding.MoveTo(roamPosition);

        if (Vector2.Distance(transform.position, Player.Instance.transform.position) < chaseRange)
        {
            state = State.Chasing;
        }

        if (Vector2.Distance(transform.position, Player.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }

        if(elapsedTime > changeRoamingDircTime )
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Chasing()
    {
        chasePosition = GetChasingPosition();
        enemyPathFinding.MoveTo(chasePosition);

        if (Health.Instance.IsDead || Vector2.Distance(transform.position, Player.Instance.transform.position) > chaseRange)
        {
            state = State.Roaming;
        }
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        if (attackRange!= 0 && canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttacking)
            {
                enemyPathFinding.StopMoving();
            }
            else
            {
                enemyPathFinding.MoveTo(roamPosition);
            }

            StartCoroutine(AttackCooldownRoutine());
        }

    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        elapsedTime = 0f;
        return new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
    }

    private Vector2 GetChasingPosition()
    {
        elapsedTime = 0f;
        Vector2 direction = Player.Instance.transform.position - transform.position;
        return direction.normalized;
    }
}
