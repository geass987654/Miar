using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitchPages : MonoBehaviour
{
    [SerializeField] private Toggle toggleEquipment, toggleEssential, toggleChip;
    [SerializeField] private GameObject equipment, essential, chip;

    private void Awake()
    {
        toggleEquipment.onValueChanged.AddListener(isOn => SwitchToPage("equipment"));
        toggleEssential.onValueChanged.AddListener(isOn => SwitchToPage("essential"));
        toggleChip.onValueChanged.AddListener(isOn => SwitchToPage("chip"));
    }

    private void SwitchToPage(string name)
    {
        equipment.gameObject.SetActive(false);
        essential.gameObject.SetActive(false);
        chip.gameObject.SetActive(false);

        switch (name)
        {
            case "equipment":
                equipment.gameObject.SetActive(true);
                break;

            case "essential":
                essential.gameObject.SetActive(true);
                break;

            case "chip":
                chip.gameObject.SetActive(true);
                break;

            default:
                Debug.Log("switch to pages wrong!");
                break;
        }
    }
}
