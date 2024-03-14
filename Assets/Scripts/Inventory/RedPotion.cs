using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPotion : MonoBehaviour
{
    private PlayerControls playerControls;
    private CooldownTimer potionTimer;
    private RedPotion redPotion;
    private readonly int healingAmount = 2;
    private readonly float healCD = 5f;
    private bool isHealing = false;
    
    private void Awake()
    {
        playerControls = new PlayerControls();
        potionTimer = GetComponent<CooldownTimer>();
        redPotion = GetComponent<RedPotion>();
    }

    private void Start()
    {
        playerControls.Item.UseItem.performed += _ => Drink();
        redPotion.enabled = false;
    }

    private void OnEnable()
    {
        playerControls.Enable();
        potionTimer.SetCooldownTime(healCD);
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void Drink()
    {
        if (Health.Instance.IsDead || !redPotion.enabled)
        {
            return;
        }

        if (!isHealing && !Health.Instance.IsFullHealth())
        {
            isHealing = true;
            InventoryManager.SetUseBtnComponent(false);
            Health.Instance.Heal(healingAmount);
            potionTimer.StartCoolDown(SetIsHealing);
        }
    }

    public void SetIsHealing()
    {
        InventoryManager.SetUseBtnComponent(true);
        isHealing = false;
    }
}
