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

        private const string ChooseMsg = "Choose level";
        private const string EmptyMsg = "Empty list of levels";

        private Dictionary<string, bool> _answers;
        private bool _answer;
        
        private string _levelToShow = ChooseMsg;
        private string[] _list;
        private Vector2 _scrollViewVector = Vector2.zero;
        private bool _showDropdown;

        private void Awake()
        {
            Level.Initialize();
            _list = Level.LevelNamesList.ToArray();
            Server.ConnectEvent.AddListener(ConnectEventHandler);
            Server.SendEvent.AddListener(SendEventHandler);
        }

        private void SendEventHandler()
        {
            _answer = false;
        }

        private void ConnectEventHandler()
        {
            _answers = new Dictionary<string, bool>();
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(16, 16, 192, 512));

            if (!Server.ServerConnected)
            {
                if (GUILayout.Button("Start"))
                {
                    if (_levelToShow.Equals(ChooseMsg))
                    {
                        Level.CurrentLevelNode = Level.LevelNamesList.First;
                        Level.CurrentLevelName = Level.CurrentLevelNode.Value;
                        Server.Send(Level.GetLevelPath(Level.CurrentLevelName));
                    }
                    else
                    {
                        Server.Send(Level.GetLevelPath(_levelToShow));
                        Level.CurrentLevelName = _levelToShow;
                        Level.CurrentLevelNode = Level.LevelNamesList.Find(_levelToShow);
                    }

                    app.GetComponent<Server>().Connect();
                }

                if (GUILayout.Button("Connect")) app.GetComponent<Server>().Connect();
                if (GUILayout.Button("Exit")) app.GetComponent<App>().Quit();
                if (GUILayout.Button(_levelToShow)) _showDropdown = !_showDropdown;

                if (_showDropdown)
                {
                    Level.Initialize();
                    
                    _scrollViewVector = GUILayout.BeginScrollView(_scrollViewVector, GUILayout.MaxHeight(200));

                    if (_list.Length == 0) GUILayout.Box(EmptyMsg);

                    foreach (var s in _list)
                    {
                        if (!GUILayout.Button(s)) continue;
                        _showDropdown = false;
                        _levelToShow = s;
                    }

                    GUILayout.EndScrollView();
                }
            }

            else
            {
                if (Level.CurrentLevelName != null)
                {
                    GUILayout.Box(Level.CurrentLevelName);
                    if (Level.GetLevelTask(Level.CurrentLevelName, out var task)) GUILayout.Box(task);
                }

                // _answer = GUILayout.Toggle(_answer, "Ответ верный?");

                if (Level.CurrentLevelName != null)
                {
                    if (GUILayout.Button(Level.CurrentLevelNode?.Previous == null
                            ? "Disconnect"
                            : "Back to " + Level.CurrentLevelNode.Previous.Value))
                    {
                        if (Level.CurrentLevelNode?.Previous != null)
                        {
                            Level.CurrentLevelNode = Level.CurrentLevelNode.Previous;
                            Level.CurrentLevelName = Level.CurrentLevelNode.Value;
                            Server.Send(Level.GetLevelPath(Level.CurrentLevelName));
                        }
                        else DisconnectAndReturn();
                    }
                    
                    if (GUILayout.Button(Level.CurrentLevelNode?.Next == null
                            ? "Disconnect"
                            : "Next to " + Level.CurrentLevelNode.Next.Value))
                    {
                        if (Level.CurrentLevelName != null) _answers[Level.CurrentLevelName] = _answer;
                        if (Level.CurrentLevelNode?.Next != null)
                        {
                            Level.CurrentLevelNode = Level.CurrentLevelNode.Next;
                            Level.CurrentLevelName = Level.CurrentLevelNode.Value;
                            Server.Send(Level.GetLevelPath(Level.CurrentLevelName));
                        }
                        else DisconnectAndReturn();
                    }
                }

                if (GUILayout.Button("Disconnect")) DisconnectAndReturn();
            }

            EyeTrackerSwitcher.TobiiEnabled = GUILayout.Toggle(EyeTrackerSwitcher.TobiiEnabled, "Enable Tobii");
            
            GUILayout.EndArea();
        }

        private void DisconnectAndReturn()
        {
            // Debug.Log(_answers);
            _levelToShow = ChooseMsg;
            manager.GotoScene(scene);
            app.GetComponent<App>().Disconnect();
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