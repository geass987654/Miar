using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public GameObject playerBag;                //顯示的背包UI
    private Animator animator;

    //public string collisionTag = "";
    //public bool isCollision = false;

    [SerializeField] private float moveSpeed;
    private Vector2 direction = Vector2.zero;
    private bool isBagOpen = false;

    public bool isFreezed = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //textFile = dialogueUI.GetComponent<DialogueSystem>().textFile;
    }

    private void Update()
    {
        if (!isFreezed)
        {
            ReadPlayerInput();
            SwitchAnim();
            OpenBag();
            //Interaction();
        }
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

    public void SetDirection(Vector2 vector2)
    {
        direction = vector2;
    }
    /*
    void Interaction()
    {
        if (isCollision && Input.GetKeyDown(KeyCode.F))
        {
            if (collisionTag == "NPC")
            {
                dialogueUI.GetComponent<DialogueSystem>().GetTextFromFile()
                dialogueUI.SetActive(true);
            }
            else if(collisionTag == "Lock")
            {
                lockUI.SetActive(true);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.CompareTag("NPC") || collision.transform.gameObject.CompareTag("Lock"))
        {
            isCollision = true;
            collisionTag = collision.transform.gameObject.tag;
            collision.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.gameObject.CompareTag("NPC") || collision.transform.gameObject.CompareTag("Lock"))
        {
            isCollision = false;
            collisionTag = "";
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    */
}
