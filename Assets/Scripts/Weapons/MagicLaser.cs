using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float laserRange;

    [SerializeField] private float laserGrowTime = 2f;

    private CapsuleCollider2D capsuleCollider2D;

    private bool isGrowing = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }

    private void LaserFaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direction = transform.position - mousePos;

        transform.right = -direction;
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;

        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;

            float linearTime = timePassed / laserGrowTime;

            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearTime), 1f);

            transform.GetComponent<CapsuleCollider2D>().size = new Vector2(Mathf.Lerp(1f, laserRange, linearTime), capsuleCollider2D.size.y);

            transform.GetComponent<CapsuleCollider2D>().offset = new Vector2(Mathf.Lerp(1f, laserRange, linearTime) * 0.5f, capsuleCollider2D.offset.y);

            yield return null;
        }

        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.GetComponent<Indestructible>())
        {
            isGrowing = false;
        }
    }

    public void UpdateLaserRange(float laserRange)
    {
        this.laserRange = laserRange;

        StartCoroutine(IncreaseLaserLengthRoutine());
    }
}
