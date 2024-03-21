using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSamePage : MonoBehaviour
{
    [SerializeField] private GameObject weaponPage;
    [SerializeField] private GameObject essentialPage;
    [SerializeField] private GameObject weaponBtn;
    [SerializeField] private GameObject essentialBtn;
    [SerializeField] private GameObject weaponInheritedPage;
    [SerializeField] private GameObject essestialInheritedPage;
    [SerializeField] private GameObject weaponInheritedBtn;
    [SerializeField] private GameObject essestialInheritedBtn;

    private Color32 activeImageColor = new Color32(166, 24, 4, 255);
    private Color32 inactiveImageColor = new Color32(210, 204, 186, 255);

    private void Start()
    {
        weaponBtn.GetComponent<Button>().enabled = false;
        essentialBtn.GetComponent<Button>().enabled = true;
        weaponInheritedBtn.GetComponent<Button>().enabled = false;
        essestialInheritedBtn.GetComponent<Button>().enabled = true;
    }

    public void PageOnClicked(GameObject page)
    {
        weaponPage.SetActive(false);
        essentialPage.SetActive(false);
        weaponInheritedPage.SetActive(false);
        essestialInheritedPage.SetActive(false);

        string name = page.name;

        if(name == "Weapon")
        {
            weaponPage.SetActive(true);
            weaponInheritedPage.SetActive(true);

            weaponBtn.GetComponent<Image>().color = activeImageColor;
            weaponInheritedBtn.GetComponent<Image>().color = activeImageColor;
            weaponBtn.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            weaponInheritedBtn.transform.GetChild(0).GetComponent<Text>().color = Color.white;

            essentialBtn.GetComponent<Image>().color = inactiveImageColor;
            essestialInheritedBtn.GetComponent<Image>().color = inactiveImageColor;
            essentialBtn.transform.GetChild(0).GetComponent<Text>().color = Color.black;
            essestialInheritedBtn.transform.GetChild(0).GetComponent<Text>().color = Color.black;

            weaponBtn.GetComponent<Button>().enabled = false;
            essentialBtn.GetComponent<Button>().enabled = true;
            weaponInheritedBtn.GetComponent<Button>().enabled = false;
            essestialInheritedBtn.GetComponent<Button>().enabled = true;
        }
        else if(name == "Essential")
        {
            essentialPage.SetActive(true);
            essestialInheritedPage.SetActive(true);

            weaponBtn.GetComponent<Image>().color = inactiveImageColor;
            weaponInheritedBtn.GetComponent<Image>().color = inactiveImageColor;
            weaponBtn.transform.GetChild(0).GetComponent<Text>().color = Color.black;
            weaponInheritedBtn.transform.GetChild(0).GetComponent<Text>().color = Color.black;

            essentialBtn.GetComponent<Image>().color = activeImageColor;
            essestialInheritedBtn.GetComponent<Image>().color = activeImageColor;
            essentialBtn.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            essestialInheritedBtn.transform.GetChild(0).GetComponent<Text>().color = Color.white;

            weaponBtn.GetComponent<Button>().enabled = true;
            essentialBtn.GetComponent<Button>().enabled = false;
            weaponInheritedBtn.GetComponent<Button>().enabled = true;
            essestialInheritedBtn.GetComponent<Button>().enabled = false;
        }

        InventoryManager.CleanItemInfo();
    }
}
