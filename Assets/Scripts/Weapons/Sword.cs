using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    private Animator animator;
    private Transform swordCollider;
    [SerializeField] private WeaponInfo weaponInfo;

    private AudioSource audioSmash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSmash = GetComponent<AudioSource>();
    }

    private void Start()
    {
        swordCollider = Player.Instance.GetWeaponCollider();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        swordCollider.gameObject.SetActive(true);

        audioSmash.Play();
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(Player.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if(mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 0f, -angle + 45);
            swordCollider.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 180f, -angle + 45);
            swordCollider.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private void AfterAttackAnimEvent()
    {
        swordCollider.gameObject.SetActive(false);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
