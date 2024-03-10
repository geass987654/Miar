using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick_img : MonoBehaviour
{
    public Sprite normalImage;
    public Sprite clickedImage;

    private Button button;
    private static Button lastClickedButton;

    void Start()
    {
        button = GetComponent<Button>();

        normalImage = button.image.sprite;

        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (button == lastClickedButton)
        {
            return; 
        }
        else if (lastClickedButton != null)
        {
            lastClickedButton.image.sprite = lastClickedButton.GetComponent<ButtonClick_img>().normalImage;
        }

        button.image.sprite = clickedImage;
        lastClickedButton = button;

    }
}