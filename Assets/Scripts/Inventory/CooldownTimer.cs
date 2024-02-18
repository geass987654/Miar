using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTimer : MonoBehaviour
{
    private float cooldownTime = 5f;
    public float elapsedTime { get; private set; } = 0f;
    private bool startCooldown = false;
    private Image filledImage;

    private Action _onCoolDownEnd;

    private void Awake()
    {
        filledImage = transform.GetChild(2).GetComponent<Image>();
    }

    private void Update()
    {
        Cooldown();
    }

    public void StartCoolDown(Action onCoolDownEnd = null)
    {
        startCooldown = true;
        _onCoolDownEnd = onCoolDownEnd;
    }

    private void EndCoolDown()
    {
        _onCoolDownEnd?.Invoke();
    }

    private void Cooldown()
    {
        if (startCooldown)
        {
            filledImage.fillAmount = 1f - (elapsedTime / cooldownTime);

            elapsedTime += Time.deltaTime;

            if (elapsedTime >= cooldownTime)
            {
                filledImage.fillAmount = 0f;
                elapsedTime = 0f;
                EndCoolDown();
                startCooldown = false;
            }
        }
    }

    public void SetCooldownTime(float cooldownTime)
    {
        this.cooldownTime = cooldownTime;
    }
}
