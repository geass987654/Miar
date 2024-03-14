using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePotion : MonoBehaviour
{
    private PlayerControls playerControls;
    private CooldownTimer potionTimer;
    private BluePotion bluePotion;
    private Timer timer;
    private readonly float addTimeCD = 10f;
    private bool isAddingTime = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
        potionTimer = GetComponent<CooldownTimer>();
        bluePotion = GetComponent<BluePotion>();
        timer = GameObject.Find("Timer").GetComponent<Timer>();
    }

    private void Start()
    {
        playerControls.Item.UseItem.performed += _ => Drink();
        bluePotion.enabled = false;
    }

    private void OnEnable()
    {
        playerControls.Enable();
        potionTimer.SetCooldownTime(addTimeCD);
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void Drink()
    {
        if (Health.Instance.IsDead || !bluePotion.enabled)
        {
            return;
        }

        if (!isAddingTime)
        {
            isAddingTime = true;
            InventoryManager.SetUseBtnComponent(false);
            timer.MoreTime();
            potionTimer.StartCoolDown(SetIsPausing);
        }
    }

    public void SetIsPausing()
    {
        InventoryManager.SetUseBtnComponent(true);
        isAddingTime = false;
    }
}
