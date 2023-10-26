using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public GameObject playerBag;  //顯示的背包UI
    private Animator animator;

    [SerializeField] private float moveSpeed;
    private Vector2 direction = Vector2.zero;
    private bool isBagOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", direction.y);
        }

        animator.SetFloat("magnitude", direction.sqrMagnitude);
    }

    void Move()
    {
        rb.MovePosition(rb.position + direction.normalized *  (moveSpeed * Time.fixedDeltaTime));
    }

    void OpenBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            //當背包被打叉關掉，isBagOpen = false，簡化為以下程式碼
            isBagOpen = playerBag.activeSelf;

            isBagOpen = !isBagOpen;
            playerBag.SetActive(isBagOpen);
        }
    }
}
