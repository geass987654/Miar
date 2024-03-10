using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private readonly int weaponSlotIndex = 0;
    private PlayerControls playerControls;
    private WeaponInfo weaponInfo;

    public CooldownTimer WeaponTimer { get; private set; }
    //private CooldownTimer[] weaponTimers;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();

        WeaponTimer = transform.GetChild(weaponSlotIndex).GetComponent<CooldownTimer>();

        //weaponTimers = new CooldownTimer[3];

        //for (int i = 0; i < weaponTimers.Length; i++)
        //{
        //    weaponTimers[i] = transform.GetChild(i).GetComponent<CooldownTimer>();
        //}
    }

    private void Start()
    {
        //playerControls.Inventory.KeyBoard.performed += context => ToggleActiveSlot((int)context.ReadValue<float>());
        //playerControls.Inventory.Change.performed += _ => ChangeWeapon();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void EquipStartingWeapon()
    {
        //ToggleActiveHighlight(0);
        //activeSlotIndex = 0;

        //ChangeActiveWeapon();
    }

    public void ChangeWeapon()
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        weaponInfo = transform.GetChild(weaponSlotIndex).GetComponent<InventorySlot>().GetWeaponInfo();

        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.NullWeapon();
            return;
        }

        GameObject weaponToSpawn = weaponInfo.weaponPrefab;

        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);

        WeaponTimer.SetCooldownTime(weaponInfo.weaponCooldown);

        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        newWeapon.transform.SetParent(ActiveWeapon.Instance.transform);

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }

    //private void ToggleActiveSlot(int numValue)
    //{
    //    if (Health.Instance.IsDead)
    //    {
    //        return;
    //    }

    //    ToggleActiveHighlight(numValue - 1);
    //}

    //private void ToggleActiveHighlight(int indexNum)
    //{
    //    if (Health.Instance.IsDead)
    //    {
    //        return;
    //    }

    //    activeSlotIndex = indexNum;

    //    foreach (Transform inventorySlot in this.transform)
    //    {
    //        inventorySlot.GetChild(0).gameObject.SetActive(false);
    //    }

    //    transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

    //    ChangeActiveWeapon();
    //}

    //private void ChangeActiveWeapon()
    //{
    //    if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
    //    {
    //        Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
    //    }

    //    Transform childTransform = transform.GetChild(activeSlotIndex);
    //    InventorySlot inventorySlot = childTransform.GetComponent<InventorySlot>();
    //    WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();

    //    //ActiveWeapon.Instance.SetCooldownTimer(weaponTimer);
    //    //ActiveWeapon.Instance.SetCooldownTimer(weaponTimers[activeSlotIndex]);

    //    if (weaponInfo == null)
    //    {
    //        ActiveWeapon.Instance.NullWeapon();
    //        return;
    //    }

    //    GameObject weaponToSpawn = weaponInfo.weaponPrefab;

    //    GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);

    //    ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

    //    newWeapon.transform.SetParent(ActiveWeapon.Instance.transform);

    //    ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    //}
}
