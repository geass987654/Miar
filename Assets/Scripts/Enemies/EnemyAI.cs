using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming
    }

    private State state;
    private EnemyPathFinding enemyPathFinding;

    private void Awake()
    {
        state = State.Roaming;
        enemyPathFinding = GetComponent<EnemyPathFinding>();
    }

    private void Start()
    {
        StartCoroutine(RoamingRoutine());
    }

    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            Vector2 roamingPos = GetRoamingPosition();

            enemyPathFinding.MoveTo(roamingPos);

            yield return new WaitForSeconds(1f);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
    }
}
