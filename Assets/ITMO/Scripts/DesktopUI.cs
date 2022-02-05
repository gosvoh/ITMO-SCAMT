using System.Collections.Generic;
using System.Linq;
using NarupaIMD.UI;
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
        private string levelToShow = "Choose level";
        private string[] list;
        private Vector2 scrollViewVector = Vector2.zero;
        private bool showDropdown;

        private void Awake()
        {
            Level.Initialize();
            list = Level.LevelNamesList.ToArray();
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(16, 16, 192, 512));

            if (!Server.ServerConnected)
            {
                if (GUILayout.Button("Start"))
                {
                    if (levelToShow.Equals("Choose level"))
                    {
                        var level = Level.DifficultyLevels.Keys.First();
                        Server.Send(Level.DifficultyLevels[level].First());
                        Level.CurrentLevelName = level;
                        Level.CurrentLevelNode = Level.LevelNamesList.Find(level);
                    }
                    else
                    {
                        Server.Send(Level.GetLevelPath(levelToShow));
                        Level.CurrentLevelName = levelToShow;
                        Level.CurrentLevelNode = Level.LevelNamesList.Find(levelToShow);
                    }

                    app.GetComponent<Server>().Connect();
                }

                if (GUILayout.Button("Connect")) app.GetComponent<Server>().Connect();
                if (GUILayout.Button("Exit")) app.GetComponent<App>().Quit();
                if (GUILayout.Button(levelToShow)) showDropdown = true;

                if (showDropdown)
                {
                    scrollViewVector = GUILayout.BeginScrollView(scrollViewVector, GUILayout.MaxHeight(200));

                    foreach (var s in list)
                    {
                        if (!GUILayout.Button(s)) continue;
                        showDropdown = false;
                        levelToShow = s;
                    }

                    GUILayout.EndScrollView();
                }

                answers.Clear();
            }

            else
            {
                if (Level.CurrentLevelName != null)
                {
                    GUILayout.Box(Level.CurrentLevelName);
                    if (Level.AllTasks.TryGetValue(Level.CurrentLevelName, out var task)) GUILayout.Box(task);
                }

                // answer = GUILayout.Toggle(answer, "Ответ верный?");

                if (Level.CurrentLevelName != null)
                    if (GUILayout.Button(Level.CurrentLevelNode.Previous == null
                            ? "Disconnect"
                            : "Back to " + Level.CurrentLevelNode.Previous.Value))
                    {
                        if (Level.CurrentLevelNode.Previous != null)
                        {
                            Level.CurrentLevelNode = Level.CurrentLevelNode.Previous;
                            Level.CurrentLevelName = Level.CurrentLevelNode.Value;
                            Server.Send(Level.GetLevelPath(Level.CurrentLevelName));
                        }
                        else
                        {
                            manager.GotoScene(scene);
                            app.GetComponent<App>().Disconnect();
                        }
                    }

                if (Level.CurrentLevelName != null)
                    if (GUILayout.Button(Level.CurrentLevelNode.Next == null
                            ? "Disconnect"
                            : "Next to " + Level.CurrentLevelNode.Next.Value))
                    {
                        // if (Level.CurrentLevelName != null && answers.ContainsKey(Level.CurrentLevelName))
                        //     answers[Level.CurrentLevelName] = answer;
                        // else answers.Add(Level.CurrentLevelName, answer);
                        if (Level.CurrentLevelNode.Next != null)
                        {
                            Level.CurrentLevelNode = Level.CurrentLevelNode.Next;
                            Level.CurrentLevelName = Level.CurrentLevelNode.Value;
                            Server.Send(Level.GetLevelPath(Level.CurrentLevelName));
                        }
                        else
                        {
                            manager.GotoScene(scene);
                            app.GetComponent<App>().Disconnect();
                        }
                    }

                if (GUILayout.Button("Disconnect"))
                {
                    manager.GotoScene(scene);
                    app.GetComponent<App>().Disconnect();
                }
            }

            GUILayout.EndArea();

            // if (!Server.ServerConnected) return;
            // GUI.Label(new Rect(16 * 2 + 192, 16, 192, 100), $"Radius {EyeInteraction.VisibilityRadius}");
            // EyeInteraction.VisibilityRadius = (int) GUI.HorizontalSlider(new Rect(16 * 2 + 192, 16 * 2, 192, 100),
            //     EyeInteraction.VisibilityRadius, 0.0F, 100.0F);
        }

        // private void GetAnswers()
        // {
        //     GUILayout.BeginArea(new Rect(16, 16, 192, 512));
        //
        //     var rect = new Rect(20, 20, 100, 100);
        //     rect = GUI.Window(0, rect, func, "Ответы");
        //
        //     GUILayout.EndArea();
        // }
        //
        // private void func(int id)
        // {
        //     GUILayout.TextArea("wft");
        //     GUILayout.TextArea("wft");
        //     GUILayout.TextArea("wft");
        //     if (GUILayout.Button("Ок"))
        //     {
        //         manager.GotoScene(scene);
        //         app.GetComponent<App>().Disconnect();
        //     }
        // }
    }
}