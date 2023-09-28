using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject bag;

    [SerializeField] private float moveSpeed = 4f;

    private Animator animator;

    private Vector2 direction = Vector2.zero;
    private bool isBagOpen;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        ReadPlayerInput();
        SwitchAnim();
        OpenBag();
    }

    private void FixedUpdate()
    {
        Move();
    }


    void ReadPlayerInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
    }

    void SwitchAnim()
    {
        if (direction != Vector2.zero)
        {
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
        }
        animator.SetFloat("Magnitude", direction.sqrMagnitude);
    }

    void Move()
    {
        rb.MovePosition(rb.position + direction.normalized * (moveSpeed * Time.fixedDeltaTime));
    }

    void OpenBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBagOpen = !isBagOpen;
            bag.SetActive(isBagOpen);
        }
    }
}
