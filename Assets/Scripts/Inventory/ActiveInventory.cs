using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndex = 0;

    private PlayerControls playerControls;

    private CooldownTimer[] weaponTimers;

    private void Awake()
    {
        playerControls = new PlayerControls();

        weaponTimers = new CooldownTimer[3];
    }

    private void Start()
    {
        playerControls.Inventory.KeyBoard.performed += context => ToggleActiveSlot((int)context.ReadValue<float>());

        for (int i = 0; i < weaponTimers.Length; i++)
        {
            weaponTimers[i] = transform.GetChild(i).GetComponent<CooldownTimer>();
        }

        ToggleActiveHighlight(0);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndex = indexNum;

        foreach(Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        Transform childTransform = transform.GetChild(activeSlotIndex);
        InventorySlot inventorySlot = childTransform.GetComponent<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();

        ActiveWeapon.Instance.SetCooldownTimer(weaponTimers[activeSlotIndex]);

        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.NullWeapon();
            return;
        }

        GameObject weaponToSpawn = weaponInfo.weaponPrefab;

        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);

        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        newWeapon.transform.SetParent(ActiveWeapon.Instance.transform);

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
