using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace AirFishLab.ScrollingList.Demo
{
    public class eventButton : MonoBehaviour
    {
        [SerializeField] private ListEventDemo _list1;

        [SerializeField]private string topage1;
        [SerializeField]private string topage2;
        [SerializeField]private string topage3;
        [SerializeField]private string topage4;

        public Button btn1;
        public Sprite normalImage1;
        public Sprite changeImage1;
        public Button btn2;
        public Sprite normalImage2;
        public Sprite changeImage2;
        public Button btn3;
        public Sprite normalImage3;
        public Sprite changeImage3;
        public Button btn4;
        public Sprite normalImage4;
        public Sprite changeImage4;

        private void Start(){
            UIChildController._ins.switchUIPage_Child(topage1);
            _list1._list.InitializeMembers();
            btn1.image.sprite = changeImage1;
        }

        public void ChangePage()
        {
            int topageCheld = _list1.Password;
            btn1.image.sprite = normalImage1;
            btn2.image.sprite = normalImage2;
            btn3.image.sprite = normalImage3;
            btn4.image.sprite = normalImage4;
            
            if(topageCheld == 1)
            {
                UIChildController._ins.switchUIPage_Child(topage1);
                btn1.image.sprite = changeImage1;
            }
            else if(topageCheld == 2)
            {
                UIChildController._ins.switchUIPage_Child(topage2);btn2.image.sprite = changeImage2;
            }
            else if(topageCheld == 3)
            {
                UIChildController._ins.switchUIPage_Child(topage3);btn3.image.sprite = changeImage3;
            }
            else if(topageCheld == 4)
            {
                UIChildController._ins.switchUIPage_Child(topage4);btn4.image.sprite = changeImage4;
            }
        }

    }
}
