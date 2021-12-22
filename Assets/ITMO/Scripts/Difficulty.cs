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
                    server.SendAndConnect(Level.GetLevel(Level.Levels["1"].First.Value));
                    Level.CurrentLevelName = Level.Levels["1"].First.Value;
                    break;
                case 1:
                    server.SendAndConnect(Level.GetLevel(Level.Levels["2"].First.Value));
                    Level.CurrentLevelName = Level.Levels["2"].First.Value;
                    break;
                case 2:
                    server.SendAndConnect(Level.GetLevel(Level.Levels["3"].First.Value));
                    Level.CurrentLevelName = Level.Levels["3"].First.Value;
                    break;
                case 3:
                    server.SendAndConnect(Level.GetLevel(Level.Levels["4"].First.Value));
                    Level.CurrentLevelName = Level.Levels["4"].First.Value;
                    break;
                default:
                    throw new NullReferenceException("Неверный выбор уровня сложности!");
            }
        }
    }
}