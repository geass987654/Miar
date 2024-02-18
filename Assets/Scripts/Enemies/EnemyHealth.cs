using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private const int maxHealth = 3;
    private int currentHealth;

    private KnockBack knockBack;
    [SerializeField] private float knockBackThrust = 10f;

    private Flash flash;

    [SerializeField] private GameObject deathVFXPrefab;

    private void Awake()
    {
        currentHealth = maxHealth;
        knockBack = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;

        knockBack.GetKnockedBack(Player.Instance.transform, knockBackThrust);

        StartCoroutine(flash.FlashRoutine());

        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreDefaultMatTime());

        DetectDeath();
    }

    private void DetectDeath()
    {
        if(currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
