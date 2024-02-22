using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private GameObject particleVFXPrefab;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;
    private Vector2 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        Indestructible indestructible = collision.GetComponent<Indestructible>();
        Health health = collision.GetComponent<Health>();

        if(!collision.isTrigger && (indestructible || enemyHealth || health))
        {
            if((health && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile))
            {
                health?.TakeDamage(1, transform);
                Instantiate(particleVFXPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if(!collision.isTrigger && indestructible)
            {
                Instantiate(particleVFXPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector2.right * (moveSpeed * Time.deltaTime));
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void DetectFireDistance()
    {
        if (Vector2.Distance(transform.position, startPos) > projectileRange)
        {
            Destroy(gameObject);
        }
    }
}
