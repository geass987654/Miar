using UnityEngine;

public class UIBtnController : MonoBehaviour
{
    [SerializeField] private KeyCode key_input;
    [SerializeField] private string to_page;

    void Update()
    {
        if (Input.GetKeyDown(key_input))
        {
            UIMainController._ins.switchUIPage(to_page);
        }
    }

    public void toPage(string pageName)
    {
        UIMainController._ins.switchUIPage(pageName);
    }

    public void toPage_Child(string pageName)
    {
        UIChildController._ins.switchUIPage_Child(pageName);
    }
}
