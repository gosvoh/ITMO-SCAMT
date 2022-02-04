using UnityEngine;
using UnityEngine.UI;

namespace ITMO.Scripts
{
    public class Difficulty : MonoBehaviour
    {
        [SerializeField] private Server server;

        public void ChooseDifficultyAndRun()
        {
            var dropdown = GameObject.Find("Dropdown").GetComponent<Dropdown>();
            var difficulty = dropdown.options[dropdown.value].text;
            Server.Send(Level.GetLevelPath(Level.DifficultyLevels[difficulty].First.Value));
            Level.CurrentLevelName = Level.DifficultyLevels[difficulty].First.Value;
            server.Connect();
        }
    }
}