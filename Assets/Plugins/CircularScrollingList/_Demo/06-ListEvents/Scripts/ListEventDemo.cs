using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace AirFishLab.ScrollingList.Demo
{
    public class ListEventDemo : MonoBehaviour
    {
        [SerializeField]
        public CircularScrollingList _list;
        [SerializeField]
        private Text _selectedContentText;
        [SerializeField]
        private Text _requestedContentText;
        [SerializeField]
        private Text _autoUpdatedContentText;
        public int Password { get; private set; }

        public void DisplayFocusingContent()
        {
            var contentID = _list.GetFocusingContentID();
            var centeredContent =
                (IntListContent)_list.ListBank.GetListContent(contentID);
            _requestedContentText.text =
                $"Focusing content: {centeredContent.Value}";
                Password = centeredContent.Value;
        }

        public void OnBoxSelected(ListBox listBox)
        {
            var content =
                (IntListContent)_list.ListBank.GetListContent(listBox.ContentID);
            _selectedContentText.text =
                $"Selected content ID: {listBox.ContentID}, Content: {content.Value}";
                Password = content.Value;
        }

        public void OnFocusingBoxChanged(
            ListBox prevFocusingBox, ListBox curFocusingBox)
        {
            _autoUpdatedContentText.text =
                "(Auto updated)\nFocusing content: "
                + $"{((IntListBox) curFocusingBox).Content}";
                Password = ((IntListBox) curFocusingBox).Content;
        }

        public void OnMovementEnd()
        {
            Debug.Log("Movement Ends");
        }
    }
}
