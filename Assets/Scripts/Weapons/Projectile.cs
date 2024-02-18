using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f;

    [SerializeField] private GameObject particleVFXPrefab;

    private WeaponInfo weaponInfo;
    private Vector3 startPos;

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

        if(!collision.isTrigger && (indestructible || enemyHealth))
        {
            Instantiate(particleVFXPrefab, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * (moveSpeed * Time.deltaTime));
    }

    public void UpdateWeaponInfo(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPos) > weaponInfo.weaponRange)
        {
            Destroy(gameObject);
        }
    }
}
