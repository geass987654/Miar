using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilePerBurst;
    [SerializeField] [Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private float restTime = 1f;
    [Tooltip("Stagger has to be enabled for oscillate to work properly.")]
    [SerializeField] private bool stagger;
    [SerializeField] private bool oscillate;

    private bool isShooting = false;

    private void OnValidate()
    {
        if (oscillate) { stagger = true; }
        if (!oscillate) { stagger = false; }
        if (burstCount < 1){ burstCount = 1; }
        if (projectilePerBurst < 1) { projectilePerBurst = 1; }
        if (startingDistance < 0.1f) { startingDistance = 0.1f; };
        if (timeBetweenBursts < 0.1f) { timeBetweenBursts = 0.1f; };
        if (restTime < 0.1f) { restTime = 0.1f; };
        if (angleSpread == 0f) { projectilePerBurst = 1; };
        if (bulletMoveSpeed <= 0f) { bulletMoveSpeed = 0.1f; };
    }

    public void Attack()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, endAngle, currentAngle, angleStep;
        float timeBetweenProjectiles = 0f;

        TargetConeOfInfluence(out startAngle, out endAngle, out currentAngle, out angleStep);
        if (stagger)
        {
            timeBetweenProjectiles = timeBetweenBursts / projectilePerBurst;
        }

        for (int i = 0; i < burstCount; i++)
        {
            if (!oscillate)
            {
                TargetConeOfInfluence(out startAngle, out endAngle, out currentAngle, out angleStep);
            }

            if(oscillate && i % 2 != 1)
            {
                TargetConeOfInfluence(out startAngle, out endAngle, out currentAngle, out angleStep);
            }
            else if(oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1f;
            }

            for (int j = 0; j < projectilePerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if(newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;

                if (stagger)
                {
                    yield return new WaitForSeconds(timeBetweenProjectiles);
                }
            }

            currentAngle = startAngle;

            if (!stagger)
            {
                yield return new WaitForSeconds(timeBetweenBursts);
            }
        }

        yield return new WaitForSeconds(restTime);

        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float endAngle, out float currentAngle, out float angleStep)
    {
        Vector2 targetDirection = Player.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0f;

        if (angleSpread != 0f)
        {
            angleStep = angleSpread / (projectilePerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);

        return pos;
    }
}
