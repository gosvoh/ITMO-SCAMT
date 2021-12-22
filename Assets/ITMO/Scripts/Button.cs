using TMPro;
using UnityEngine;

namespace ITMO.Scripts
{
    public class Button : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Server server;

        public void SetText(string text) => this.text.text = text;
        

        public void OnClick()
        {
            server.Send(Level.GetLevel(text.text));
            Level.CurrentLevelName = text.text;
            server.Connect();
        }
    }
}