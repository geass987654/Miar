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

    [SerializeField] private GameObject playerBag;                //��ܪ��I�]UI
    [SerializeField] private Transform weaponCollider;

    private bool isBagOpen = false;
    public bool isFreezed = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls(); //��l��playerControls
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        playerControls.Movement.Move.performed += value => ReadPlayerInput(value.ReadValue<Vector2>()); //���J���ܰʮɡAŪ���ë�����direction
        playerControls.Movement.Move.canceled += _ => direction = Vector2.zero; //�P�}����ɧ�direction�]���s�V�q

        playerControls.Movement.Move.performed += _ => SwitchAnim();

        playerControls.Bag.Open.started += _ => OpenBag(); //�b���ǻ��Ѽƪ����p�U�A���U����ɩI�sOpenBag()

        //ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable()
    {
        playerControls.Enable(); //�ҥ�playerControls
    }

    private void OnDisable()
    {
        playerControls.Disable(); //����playerControls
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ReadPlayerInput(Vector2 direction)
    {
        if (isFreezed)
        {
            return;
        }

        this.direction = direction;
    }

    private void SwitchAnim()
    {
        if (isFreezed)
        {
            return;
        }

        if (direction != Vector2.zero)
        {
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", direction.y);
        }

        animator.SetFloat("magnitude", direction.sqrMagnitude);
    }

    private void Move()
    {
        if (knockBack.IsKnockedBack || Health.Instance.IsDead)
        {
            return;
        }

        rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime));
    }

    public void OpenBag()   //���禡�b���U�����I�]���s�ɤ]�����I�s�A�]���ŧi��public
    {
        isBagOpen = !isBagOpen; //���U����}�ҭI�]�F�A�����U���������I�]
        playerBag.SetActive(isBagOpen);

        ActiveWeapon.Instance.canAttack = !playerBag.activeSelf; //�}�ҭI�]�ɵL�k����

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
