using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    private PlayerControls playerControls;
    private float timeBetweenAttacks;
    private bool attackBtnDown, isAttacking = false;

    public bool canAttack = true;
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    //private CooldownTimer currentCooldownTimer;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        StopAllCoroutines();
    }

    private void Update()
    {
        Attack();
    }

    private void StartAttacking()
    {
        attackBtnDown = true;
    }

    private void StopAttacking()
    {
        attackBtnDown = false;
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;

        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;

        //currentCooldownTimer.SetCooldownTime(timeBetweenAttacks);
    }

    public void NullWeapon()
    {
        CurrentActiveWeapon = null;

        //currentCooldownTimer.SetCooldownTime(10f);
    }

    private void Attack()
    {
        if (attackBtnDown && !isAttacking && CurrentActiveWeapon && canAttack)
        {
            AttackCooldown();

            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }

    private void AttackCooldown()
    {
        isAttacking = true;

        StopAllCoroutines();

        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        //currentCooldownTimer.StartCoolDown();

        ActiveInventory.Instance.WeaponTimer.StartCoolDown();

        yield return new WaitForSeconds(timeBetweenAttacks);

        isAttacking = false;
    }

    //public void SetCooldownTimer(CooldownTimer cooldownTimer)
    //{
    //    currentCooldownTimer = cooldownTimer;
    //}
}
