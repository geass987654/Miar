using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 4f;

    private Animator animator;
    private AnimatorClipInfo[] clipInfo;
    private Vector2 direction = Vector2.zero;
    private string animationName;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        ReadPlayerInput();
        AdjustIdleDirection();
    }

    private void FixedUpdate()
    {
        Move();
    }


    void ReadPlayerInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Magnitude", direction.sqrMagnitude);
    }

    void AdjustIdleDirection()
    {
        animationName = GetCurrentClipName();
        switch (animationName)
        {
            case "Run_front":
                animator.SetInteger("Direction", 0);
                break;

            case "Run_back":
                animator.SetInteger("Direction", 1);
                break;

            case "Run_left":
                animator.SetInteger("Direction", 2);
                break;

            case "Run_right":
                animator.SetInteger("Direction", 3);
                break;

            default:
                break;
        }
    }

    private string GetCurrentClipName()
    {
        clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        return clipInfo[0].clip.name;
    }

    void Move()
    {
        rb.MovePosition(rb.position + direction.normalized * (moveSpeed * Time.fixedDeltaTime));
    }
}
