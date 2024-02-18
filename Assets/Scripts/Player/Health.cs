using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private const int maxHealth = 5;
    private int currentHealth;
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
        currentHealth = maxHealth;
        Hearts = new GameObject[maxHealth];

        for(int i = 0; i < HealthPointBar.transform.childCount; i++)
        {
            Hearts[i] = HealthPointBar.transform.GetChild(i).gameObject;
        }

        knockBack = transform.GetComponent<KnockBack>();

        flash = transform.GetComponent<Flash>();
    }

    public void TakeDamage(int damageAmount, Transform damageSource)
    {
        for(int i = 0; i < damageAmount; i++)
        {
            if(currentHealth <= 0)
            {
                Time.timeScale = 0;

                if (!gameOver.activeSelf)
                {
                    gameOver.SetActive(true);
                }

                break;
            }

            Hearts[currentHealth - 1].GetComponent<Image>().sprite = emptyHeart;
            //Debug.Log("currentHealth = " + currentHealth);
            currentHealth--;
        }

        knockBack.GetKnockedBack(damageSource.transform, knockBackThrust);

        StartCoroutine(flash.FlashRoutine());
    }

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
