using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace AirFishLab.ScrollingList.Demo
{
    public class eventquest : MonoBehaviour
    {
        [SerializeField] private ListEventDemo _list1;
        [SerializeField] private ListEventDemo _list2;
        [SerializeField] private ListEventDemo _list3;
        [SerializeField] private ListEventDemo _list4;

        public GameObject imageAni;
        public Animator imageani;
        public Canvas parentcanvas;
        public Canvas Lock;
        public Canvas Locked;
        public Canvas topic;
        [SerializeField] private Image question;

        public string UnlockedAnimator = "unlocked";

        [SerializeField] private AudioSource bingo_audio;

        public void bingo_Audio()
        {
            bingo_audio.Play();
        }

        public void initlocked()
        {
            Lock.enabled = false;
            Locked.enabled = false;
            topic.enabled = true;
            imageAni.SetActive(false);
            _list1._list.InitializeMembers();
            _list2._list.InitializeMembers();
            _list3._list.InitializeMembers();
            _list4._list.InitializeMembers();
            
        }


        public string stringPassword { get; private set; }
        public void OnMovementEnd()
        {
            CheckPassword();
        }

         private void CheckPassword()
        {
            int password1 = _list1.Password -2;
            int password2 = _list2.Password -2;
            int password3 = _list3.Password -2;
            int password4 = _list4.Password -2;

            if(password1 == -1)password1=9;
            if(password2 == -1)password2=9;
            if(password3 == -1)password3=9;
            if(password4 == -1)password4=9;
            string password = password1.ToString() + password2.ToString() +password3.ToString() +password4.ToString();

            Debug.Log(password);
            stringPassword = password;
        }

         private void Start()
        {
            initlocked();
        }

        private void Update(){
            if(parentcanvas.GetComponent<Canvas>().enabled){
                imageAni.SetActive(true);
            }
        }

        public void Unlocked(){
            imageani.SetBool(UnlockedAnimator,true);
            Locked.enabled = false;
            topic.enabled = false;
        }
        public void canvasclose(){
            parentcanvas.enabled = false;
        }
        public void SetQuestion(Sprite questionPicture)
        {
            question.sprite = questionPicture;
        }
        
    }
}
