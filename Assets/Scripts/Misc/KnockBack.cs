using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float knockBackTime = 0.2f;
    private Rigidbody2D rb;

    public bool IsKnockedBack { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float knockBackThrust)
    {
        IsKnockedBack = true;

        Vector2 difference = knockBackThrust * rb.mass *  (transform.position - damageSource.position).normalized;

        rb.AddForce(difference, ForceMode2D.Impulse);

        StartCoroutine(KnockBackRoutine());
    }

    private IEnumerator KnockBackRoutine()
    {
        yield return new WaitForSeconds(knockBackTime);

        rb.velocity = Vector2.zero;

        IsKnockedBack = false;
    }
}
