using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    private Animator animator;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private AudioSource audioGunshot;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioGunshot = GetComponent<AudioSource>();
    }

    public void Attack()
    {
        animator.SetTrigger(FIRE_HASH);

        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);

        newArrow.GetComponent<Projectile>().UpdateWeaponInfo(weaponInfo);

        audioGunshot.Play();
    }
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
