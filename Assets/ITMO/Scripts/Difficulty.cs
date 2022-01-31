using System;
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
            switch (dropdown.value)
            {
                case 0:
                    server.Send(Level.GetLevelPath(Level.Levels["1"].First.Value));
                    Level.CurrentLevelName = Level.Levels["1"].First.Value;
                    server.Connect();
                    break;
                case 1:
                    server.Send(Level.GetLevelPath(Level.Levels["2"].First.Value));
                    Level.CurrentLevelName = Level.Levels["2"].First.Value;
                    server.Connect();
                    break;
                case 2:
                    server.Send(Level.GetLevelPath(Level.Levels["3"].First.Value));
                    Level.CurrentLevelName = Level.Levels["3"].First.Value;
                    server.Connect();
                    break;
                case 3:
                    server.Send(Level.GetLevelPath(Level.Levels["4"].First.Value));
                    Level.CurrentLevelName = Level.Levels["4"].First.Value;
                    server.Connect();
                    break;
                default:
                    throw new NullReferenceException("Неверный выбор уровня сложности!");
            }
        }
    }
}