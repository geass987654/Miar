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

    [SerializeField] private GameObject playerBag;                //顯示的背包UI
    [SerializeField] private Transform weaponCollider;

    private bool isBagOpen = false;
    public bool isFreezed = false;

    [SerializeField] private GameObject activeInventory; //武器跟道具欄位
    [SerializeField] private GameObject canvas_UI;       //遊玩UI
    private RectTransform currentRect;
    private Vector2 originalPosition;
    private Vector2 newPosition;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls(); //初始化playerControls
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        knockBack = GetComponent<KnockBack>();

        currentRect = activeInventory.transform.GetComponent<RectTransform>();
        originalPosition = currentRect.anchoredPosition;
        newPosition = new Vector2(-900, 150);
    }

    /***********   start有修改   *************/
    private void Start()
    {
        playerControls.Movement.Move.performed += value => ReadPlayerInput(value.ReadValue<Vector2>());
        playerControls.Movement.Move.canceled += _ => direction = Vector2.zero;
        playerControls.Movement.Move.performed += _ => SwitchAnim();
        playerControls.Bag.Open.started += _ => OpenBag(); //在不傳遞參數的情況下，按下按鍵時呼叫OpenBag()

        InventoryManager.Initialize();
        InventoryManager.Clear();;
        InventoryManager.RefreshWeapons();
        InventoryManager.RefreshEssentials();

        //ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable()
    {
        playerControls.Enable(); //啟用playerControls
    }

    private void OnDisable()
    {
        playerControls.Disable(); //停用playerControls
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

    /***********   OpenBag有修改   *************/
    public void OpenBag()   //此函式在按下關閉背包按鈕時也必須呼叫，因此宣告為public
    {
        isBagOpen = !isBagOpen; //按下按鍵開啟背包；再次按下按鍵關閉背包
        playerBag.SetActive(isBagOpen);

        ActiveWeapon.Instance.canAttack = !playerBag.activeSelf; //開啟背包時無法攻擊
        ActiveInventory.Instance.canUse = !playerBag.activeSelf; //開啟背包時無法使用道具

        InventoryManager.SetEquipBtnState(false);
        InventoryManager.SetUseBtnState(false);
        InventoryManager.CleanItemInfo();

        if (isBagOpen)
        {
            activeInventory.transform.SetParent(playerBag.transform);
            //Debug.Log(currentRect.anchoredPosition);
            currentRect.anchoredPosition = newPosition;
            //Debug.Log(currentRect.anchoredPosition);
        }
        else
        {
            activeInventory.transform.SetParent(canvas_UI.transform);
            currentRect.anchoredPosition = originalPosition;
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
