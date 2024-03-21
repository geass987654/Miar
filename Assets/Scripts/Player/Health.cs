using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : Singleton<Health>
{
    [SerializeField] private float knockBackThrust = 10f;
    [SerializeField] private GameObject playerDeathVFXPrefab;

    private KnockBack knockBack;
    private Flash flash;
    private const int maxHealth = 5;
    private int currentHealth;
    private bool canTakeDamage = true;
    private float damageRecoveryTime = 0.5f;
    public bool IsDead { get; private set; }

    [SerializeField] private GameObject HealthPointBar;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    private GameObject[] Hearts;
    //[SerializeField] private GameObject gameOver;



    protected override void Awake()
    {
        base.Awake();

        knockBack = transform.GetComponent<KnockBack>();
        flash = transform.GetComponent<Flash>();
        Hearts = new GameObject[maxHealth];

        for (int i = 0; i < HealthPointBar.transform.childCount; i++)
        {
            Hearts[i] = HealthPointBar.transform.GetChild(i).gameObject;
        }
    }

    private void Start()
    {
        IsDead = false;
        currentHealth = maxHealth;
        UpdateHealthPointBar();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyAI enemyAI = collision.transform.GetComponent<EnemyAI>();

        if (enemyAI != null && canTakeDamage)
        {
            TakeDamage(1, collision.transform);
        }
    }

    public bool IsFullHealth()
    {
        if(currentHealth == maxHealth)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Heal(int healingAmount)
    {
        if (IsDead)
        {
            return;
        }

        for(int i = 0; i < healingAmount; i++)
        {
            if(currentHealth < maxHealth)
            {
                currentHealth += 1;
                UpdateHealthPointBar();
            }
            else
            {
                return;
            }
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage)
        {
            return;
        }

        knockBack.GetKnockedBack(hitTransform, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthPointBar();
        CheckPlayerDeath();
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthPointBar()
    {
        foreach(Transform heart in HealthPointBar.transform)
        {
            heart.GetComponent<Image>().sprite = emptyHeart;
        }

        for(int i = 0; i < currentHealth; i++)
        {
            Hearts[i].GetComponent<Image>().sprite = fullHeart;
        }
    }

    private void CheckPlayerDeath()
    {
        if(currentHealth <= 0 && !IsDead)
        {
            IsDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;
            Debug.Log("Player Death");
            PlayerDeath();
        }
    }

    public void PlayerDeath()
    {
        if(InheritanceBox.Instance != null)
        {
            InheritanceBox.Instance.SetCanInherit(true);
        }

        Invoke("LoadScene", 2f);
        Instantiate(playerDeathVFXPrefab, transform.position, Quaternion.identity);
        Player.Instance.gameObject.SetActive(false);
    }

    private void LoadScene()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
