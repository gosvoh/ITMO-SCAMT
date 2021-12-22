using System;
using System.Collections.Generic;
using System.Linq;
using NarupaIMD.UI;
using NarupaXR;
using UnityEngine;
using UnityEngine.UI;

namespace ITMO.Scripts
{
    public class DesktopUI : MonoBehaviour
    {
        [SerializeField] private GameObject app;
        [SerializeField] private UserInterfaceManager manager;
        [SerializeField] private GameObject scene;

        private Dictionary<string, bool> answers;
        private bool answer = false;

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(16, 16, 192, 512));

            if (!Server.ServerConnected)
            {
                if (GUILayout.Button("Начать")) app.GetComponent<App>().StartSim();
                if (GUILayout.Button("Выход")) app.GetComponent<App>().Quit();
                if (answers != null) answers = null;
            }

            else
            {
                if (answers == null) answers = new Dictionary<string, bool>();

                GUILayout.Box(Level.CurrentLevelName);
                GUILayout.Box(Level.GetLevelTask(Level.CurrentLevelName));

                // answer = GUILayout.Toggle(answer, "Ответ верный?");

                if (GUILayout.Button("Назад"))
                {
                    if (Level.CurrentLevel.Previous != null)
                    {
                        Level.CurrentLevel = Level.CurrentLevel.Previous;
                        Level.CurrentLevelName = Level.CurrentLevel.Value;
                        app.GetComponent<Server>().Send(Level.GetLevel(Level.CurrentLevelName));
                    }
                    else
                    {
                        manager.GotoScene(scene);
                        app.GetComponent<App>().Disconnect();
                    }
                }

                if (GUILayout.Button("Вперед"))
                {
                    answers.Add(Level.CurrentLevelName, answer);
                    if (Level.CurrentLevel.Next != null)
                    {
                        Level.CurrentLevel = Level.CurrentLevel.Next;
                        Level.CurrentLevelName = Level.CurrentLevel.Value;
                        app.GetComponent<Server>().Send(Level.GetLevel(Level.CurrentLevelName));
                    }
                    else
                    {
                        manager.GotoScene(scene);
                        app.GetComponent<App>().Disconnect();
                    }
                }

                if (GUILayout.Button("Выход"))
                {
                    manager.GotoScene(scene);
                    app.GetComponent<App>().Disconnect();
                }
            }

            GUILayout.EndArea();
        }

        private void GetAnswers()
        {
            GUILayout.BeginArea(new Rect(16, 16, 192, 512));
            
            var rect = new Rect(20, 20, 100, 100);
            rect = GUI.Window(0, rect, func, "Ответы");
            
            GUILayout.EndArea();
        }

        private void func(int id)
        {
            GUILayout.TextArea("wft");
            GUILayout.TextArea("wft");
            GUILayout.TextArea("wft");
            if (GUILayout.Button("Ок"))
            {
                manager.GotoScene(scene);
                app.GetComponent<App>().Disconnect();
            }
        }
    }
}