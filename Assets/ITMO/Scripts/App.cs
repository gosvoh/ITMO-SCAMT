using System;
using NarupaXR;
using UnityEngine;

namespace ITMO.Scripts
{
    public class App : MonoBehaviour
    {
        [SerializeField] private GameObject taskPanel;
        [SerializeField] private Server server;

        public void Quit() => GetComponent<NarupaXRPrototype>().Quit();

        public void Disconnect()
        {
            server.Disconnect();
            Level.CurrentLevelNode = null;
            Level.CurrentLevelName = null;
            taskPanel.SetActive(false);
        }

        private void OnApplicationQuit() => Quit();
    }
}