using System;
using NarupaXR;
using UnityEngine;

namespace ITMO.Scripts
{
    public class App : MonoBehaviour
    {
        [SerializeField] private Server server;

        private void Awake()
        {
            server = GetComponent<Server>();
        }

        public void Quit() => GetComponent<NarupaXRPrototype>().Quit();

        public void Disconnect()
        {
            server.Disconnect();
            Level.CurrentLevelNode = null;
            Level.CurrentLevelName = null;
        }

        private void OnApplicationQuit() => Quit();
    }
}