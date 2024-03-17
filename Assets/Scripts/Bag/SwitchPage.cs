using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPage : MonoBehaviour
{
    [SerializeField] private GameObject weaponPage;
    [SerializeField] private GameObject essentialPage;
    [SerializeField] private GameObject weaponBtn;
    [SerializeField] private GameObject essentialBtn;

    private void Start()
    {
        weaponBtn.GetComponent<Button>().enabled = false;
        essentialBtn.GetComponent<Button>().enabled = true;
    }

    public void PageOnClicked(GameObject page)
    {
        weaponPage.SetActive(false);
        essentialPage.SetActive(false);
        page.SetActive(true);

        var temp1 = weaponBtn.GetComponent<Image>().color;
        weaponBtn.GetComponent<Image>().color = essentialBtn.GetComponent<Image>().color;
        essentialBtn.GetComponent<Image>().color = temp1;

        var temp2 = weaponBtn.transform.GetChild(0).GetComponent<Text>().color;
        weaponBtn.transform.GetChild(0).GetComponent<Text>().color = essentialBtn.transform.GetChild(0).GetComponent<Text>().color;
        essentialBtn.transform.GetChild(0).GetComponent<Text>().color = temp2;

        InventoryManager.SetEquipBtnState(false);
        InventoryManager.SetUseBtnState(false);
        InventoryManager.CleanItemInfo();

        essentialBtn.GetComponent<Button>().enabled = !essentialBtn.GetComponent<Button>().enabled;
        weaponBtn.GetComponent<Button>().enabled = !weaponBtn.GetComponent<Button>().enabled;
    }
}
