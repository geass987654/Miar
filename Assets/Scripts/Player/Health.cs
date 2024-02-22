using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private const int maxHealth = 5;
    private int currentHealth;
    private bool canTakeDamage = true;
    private float damageRecoveryTime = 1f;
    [SerializeField] private GameObject HealthPointBar;
    private GameObject[] Hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private GameObject gameOver;

    private KnockBack knockBack;
    [SerializeField] private float knockBackThrust = 10f;

    private Flash flash;

    private void Awake()
    {
        Hearts = new GameObject[maxHealth];

        for(int i = 0; i < HealthPointBar.transform.childCount; i++)
        {
            Hearts[i] = HealthPointBar.transform.GetChild(i).gameObject;
        }

        knockBack = transform.GetComponent<KnockBack>();

        flash = transform.GetComponent<Flash>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyAI enemyAI = collision.transform.GetComponent<EnemyAI>();

        if (enemyAI != null && canTakeDamage)
        {
            TakeDamage(1, collision.transform);
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
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
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

    public void Heal(int healAmount)
    {
        for (int i = 0; i < healAmount; i++)
        {
            if (currentHealth >= maxHealth)
            {
                break;
            }

            Hearts[currentHealth].GetComponent<Image>().sprite = fullHeart;
            //Debug.Log("currentHealth = " + currentHealth);
            currentHealth++;
        }
    }
}
