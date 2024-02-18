using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Singleton<Player>
{
    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.zero;
    private Animator animator;
    [SerializeField] private float moveSpeed;

    private KnockBack knockBack;

    public GameObject playerBag;                //顯示的背包UI

    private bool isBagOpen = false;
    public bool isFreezed = false;
    public bool isHurt = false;

    [SerializeField] private Transform weaponCollider;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        knockBack = GetComponent<KnockBack>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        if (!isFreezed)
        {
            //ReadPlayerInput();
            PlayerInput();
            SwitchAnim();
            OpenBag();
        }
    }

    private void FixedUpdate()
    {
        if (knockBack.IsKnockedBack)
        {
            return;
        }

        Move();
    }

    private void PlayerInput()
    {
        direction = playerControls.Movement.Move.ReadValue<Vector2>().normalized;
    }

    //void ReadPlayerInput()
    //{
    //    direction.x = Input.GetAxisRaw("Horizontal");
    //    direction.y = Input.GetAxisRaw("Vertical");
    //}

    private void SwitchAnim()
    {
        if (direction != Vector2.zero)
        {
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", direction.y);
        }

        animator.SetFloat("magnitude", direction.sqrMagnitude);
    }

    private void Move()
    {
        rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime));
    }

    //private void AdjustFacingDirection()
    //{
    //    Vector3 mousePos = Input.mousePosition;
    //    Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

    //    if(mousePos.x < playerScreenPoint.x)
    //    {
    //        spriteRenderer.flipX = true;
    //    }
    //    else
    //    {
    //        spriteRenderer.flipX= false;
    //    }
    //}

    private void OpenBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            //當背包被打叉關掉，isBagOpen = false，簡化為以下程式碼
            isBagOpen = playerBag.activeSelf;

            isBagOpen = !isBagOpen;
            playerBag.SetActive(isBagOpen);
        }
    }

    public void SetDirection(Vector2 vector2)
    {
        direction = vector2;
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }
}
