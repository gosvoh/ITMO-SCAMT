using NarupaXR;
using UnityEngine;

namespace ITMO.Scripts
{
    public class App : MonoBehaviour
    {
        [SerializeField] private GameObject taskPanel;
        [SerializeField] private Server server;

        public void Quit() => GetComponent<NarupaXRPrototype>().Quit();

        private void Awake()
        {
            Reference.AppInstance = this;
        }

        public void StartSim()
        {
            server.Send(Level.GetLevelPath(Level.CurrentLevel.Value));
            Level.CurrentLevel = Level.LevelList.First;
            Level.CurrentLevelName = Level.CurrentLevel.Value;
            server.Connect();
        }

        public void Disconnect()
        {
            server.Disconnect();
            taskPanel.SetActive(false);
        }

        private void OnApplicationQuit() => Quit();
    }
}