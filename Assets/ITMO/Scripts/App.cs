using System;
using NarupaIMD;
using NarupaXR;
using UnityEngine;

namespace ITMO.Scripts
{
    public class App : MonoBehaviour
    {

        [SerializeField] private GameObject taskPanel;
        
        public void Quit() => GetComponent<NarupaXRPrototype>().Quit();


        public void StartSim()
        {
            Level.CurrentLevel = Level.LevelList.First;
            GetComponent<Server>().Send(Level.GetLevel(Level.CurrentLevel.Value));
            Level.CurrentLevelName = Level.CurrentLevel.Value;
            GetComponent<Server>().Connect();
        }

        public void Disconnect()
        {
            GetComponent<Server>().Disconnect();
            taskPanel.SetActive(false);
        }

        private void OnApplicationQuit() => Quit();
    }
}