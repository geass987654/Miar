using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaserPrefab;
    [SerializeField] private Transform magicLaserSpawnPoint;
    
    private Animator animator;
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private AudioSource audioMagicLaser;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioMagicLaser = GetComponent<AudioSource>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        animator.SetTrigger(ATTACK_HASH);
    }
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(Player.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 0f, -angle);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, -180f, -angle);
        }
    }

    public void SpawnStaffProjectileAnimEvent()
    {
        GameObject newMigicLaser = Instantiate(magicLaserPrefab, magicLaserSpawnPoint.position, Quaternion.identity);

        newMigicLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);

        audioMagicLaser.Play();
    }
}
