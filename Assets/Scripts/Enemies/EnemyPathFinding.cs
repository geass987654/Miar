using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private KnockBack knockBack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();
    }

    private void FixedUpdate()
    {
        if (knockBack.IsKnockedBack)
        {
            return;
        }
        rb.MovePosition(rb.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPos)
    {
        moveDirection = targetPos;
    }

    public void StopMoving()
    {
        moveDirection = Vector2.zero;
    }
}
