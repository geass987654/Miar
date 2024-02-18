using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    private const int damageAmount = 2;

    [SerializeField] private AudioSource audioSource_1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.GetComponent<Health>().TakeDamage(damageAmount, transform);
            
            audioSource_1.Play();
        }
    }
}
