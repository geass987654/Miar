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
    private Slider healthSlider;
    private const string HEALTH_SLIDER_TEXT = "Health Slider";
    private const string SCENE_TEXT = "TestScene_0228";
    public bool IsDead { get; private set; }
    private Vector2 playerStartingPos;

    //[SerializeField] private GameObject HealthPointBar;
    //[SerializeField] private Sprite fullHeart;
    //[SerializeField] private Sprite emptyHeart;
    //[SerializeField] private GameObject gameOver;
    //private GameObject[] Hearts;



    protected override void Awake()
    {
        base.Awake();

        //Hearts = new GameObject[maxHealth];

        //for(int i = 0; i < HealthPointBar.transform.childCount; i++)
        //{
        //    Hearts[i] = HealthPointBar.transform.GetChild(i).gameObject;
        //}

        knockBack = transform.GetComponent<KnockBack>();

        flash = transform.GetComponent<Flash>();
    }

    private void Start()
    {
        IsDead = false;
        playerStartingPos = Player.Instance.transform.position;
        currentHealth = maxHealth;
        UpdateHealthSlider();
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
                UpdateHealthSlider();
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
        UpdateHealthSlider();
        CheckPlayerDeath();
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if(healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
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

    private void PlayerDeath()
    {
        Invoke("ReloadScene", 2f);
        Instantiate(playerDeathVFXPrefab, transform.position, Quaternion.identity);
        Player.Instance.gameObject.SetActive(false);
    }

    private void ReloadScene()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SCENE_TEXT);
    }

    //public void TakeDamage(int damageAmount, Transform damageSource)
    //{
    //    for(int i = 0; i < damageAmount; i++)
    //    {
    //        if(currentHealth <= 0)
    //        {
    //            Time.timeScale = 0;

    //            if (!gameOver.activeSelf)
    //            {
    //                gameOver.SetActive(true);
    //            }

    //            break;
    //        }

    //        Hearts[currentHealth - 1].GetComponent<Image>().sprite = emptyHeart;
    //        //Debug.Log("currentHealth = " + currentHealth);
    //        currentHealth--;
    //    }

    //    knockBack.GetKnockedBack(damageSource.transform, knockBackThrust);

    //    StartCoroutine(flash.FlashRoutine());
    //}

    //public void Heal(int healAmount)
    //{
    //    for (int i = 0; i < healAmount; i++)
    //    {
    //        if (currentHealth >= maxHealth)
    //        {
    //            break;
    //        }

    //        Hearts[currentHealth].GetComponent<Image>().sprite = fullHeart;
    //        //Debug.Log("currentHealth = " + currentHealth);
    //        currentHealth++;
    //    }
    //}
}
