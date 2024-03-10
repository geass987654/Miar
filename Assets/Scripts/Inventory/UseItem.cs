using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    private PlayerControls playerControls;
    private CooldownTimer[] itemTimers;
    private Timer timer;
    private readonly int healingAmount = 2;
    private readonly float healCD = 5f;
    private readonly float addTimeCD = 30f;
    private bool isHealing = false, isAddingTime = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
        itemTimers = new CooldownTimer[2];
        timer = GameObject.Find("Timer").GetComponent<Timer>();
    }

    private void Start()
    {
        playerControls.Item.Use.performed += context => Use((int)context.ReadValue<float>());

        for(int i = 0; i < itemTimers.Length; i++)
        {
            itemTimers[i] = transform.GetChild(i + 2).GetComponent<CooldownTimer>();
        }

        itemTimers[0].SetCooldownTime(healCD);
        itemTimers[1].SetCooldownTime(addTimeCD);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Use(int numValue)
    {
        if (Health.Instance.IsDead)
        {
            return;
        }

        int itemIndex = numValue - 4;

        if (itemIndex < 1 && !isHealing)
        {
            isHealing = true;
            Health.Instance.Heal(healingAmount);
            Debug.Log("回復2點血量");
            itemTimers[0].StartCoolDown(SetIsHealing);
        }
        else if (itemIndex == 1 && !isAddingTime)
        {
            isAddingTime = true;
            timer.MoreTime();
            Debug.Log("時間增加2分鐘");
            itemTimers[1].StartCoolDown(SetIsPausing);
        }
    }

    public void SetIsHealing()
    {
        isHealing = false;
    }
    public void SetIsPausing()
    {
        isAddingTime = false;
    }
}
