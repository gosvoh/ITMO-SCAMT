using System.Collections.Generic;
using NarupaIMD.UI;
using NarupaXR.Interaction;
using UnityEngine;

namespace ITMO.Scripts
{
    public class DesktopUI : MonoBehaviour
    {
        [SerializeField] private GameObject app;
        [SerializeField] private UserInterfaceManager manager;
        [SerializeField] private GameObject scene;
        private readonly bool answer = false;

        private readonly Dictionary<string, bool> answers = new Dictionary<string, bool>();

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(16, 16, 192, 512));

            if (!Server.ServerConnected)
            {
                if (GUILayout.Button("Начать")) app.GetComponent<App>().StartSim();
                if (GUILayout.Button("Присоединиться")) app.GetComponent<Server>().Connect();
                if (GUILayout.Button("Выход")) app.GetComponent<App>().Quit();
                answers.Clear();
            }

            else
            {
                GUILayout.Box(Level.CurrentLevelName);
                GUILayout.Box(Level.GetLevelTask(Level.CurrentLevelName));

                // answer = GUILayout.Toggle(answer, "Ответ верный?");

                if (GUILayout.Button("Назад"))
                {
                    if (Level.CurrentLevel.Previous != null)
                    {
                        Level.CurrentLevel = Level.CurrentLevel.Previous;
                        Level.CurrentLevelName = Level.CurrentLevel.Value;
                        app.GetComponent<Server>().Send(Level.GetLevelPath(Level.CurrentLevelName));
                    }
                    else
                    {
                        manager.GotoScene(scene);
                        app.GetComponent<App>().Disconnect();
                    }
                }

                if (GUILayout.Button("Вперед"))
                {
                    if (answers.ContainsKey(Level.CurrentLevelName)) answers[Level.CurrentLevelName] = answer;
                    else answers.Add(Level.CurrentLevelName, answer);
                    if (Level.CurrentLevel.Next != null)
                    {
                        Level.CurrentLevel = Level.CurrentLevel.Next;
                        Level.CurrentLevelName = Level.CurrentLevel.Value;
                        app.GetComponent<Server>().Send(Level.GetLevelPath(Level.CurrentLevelName));
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

            if (!Server.ServerConnected) return;
            GUI.Label(new Rect(16 * 2 + 192, 16, 192, 100), $"Радиус {EyeInteraction.VisibilityRadius}");
            EyeInteraction.VisibilityRadius = (int) GUI.HorizontalSlider(new Rect(16 * 2 + 192, 16 * 2, 192, 100),
                EyeInteraction.VisibilityRadius, 0.0F, 100.0F);
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