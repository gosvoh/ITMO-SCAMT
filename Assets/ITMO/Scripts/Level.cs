using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ITMO.Scripts
{
    public class Level : MonoBehaviour
    {
        private static Dictionary<string, string> _levels;
        private static Dictionary<string, string> _tasks;
        public static LinkedList<string> LevelList;
        public static Dictionary<string, LinkedList<string>> Levels;
        public static string CurrentLevelName;

        public static LinkedListNode<string> CurrentLevel;

        private void Awake()
        {
            GetFiles();
        }

        private static void GetFiles()
        {
            _levels = new Dictionary<string, string>();
            _tasks = new Dictionary<string, string>();
            Levels = new Dictionary<string, LinkedList<string>>();
            var dir = Directory.EnumerateFiles(Application.streamingAssetsPath + "\\Molecules\\");
            foreach (var s in dir)
            {
                var fn = Path.GetFileName(s).Split('.');
                switch (fn[fn.Length - 1])
                {
                    case "pdb":
                        _levels.Add(fn[0], s);
                        break;
                    case "txt":
                        string line;
                        if ((line = File.ReadAllLines(s)[0]).Length > 0) _tasks.Add(fn[0], line);
                        break;
                }
            }

            foreach (var s in _levels.Keys)
            {
                var dif = s.Split('_')[0];
                if (!Levels.ContainsKey(dif)) Levels.Add(dif, new LinkedList<string>());
                Levels[dif].AddLast(s);
            }

            LevelList = new LinkedList<string>(_levels.Keys);
            CurrentLevelName = LevelList.First.Value;
            CurrentLevel = LevelList.First;
        }

        public static string GetLevelPath(string lvl)
        {
            return _levels[lvl];
        }

        public static string GetLevelTask(string lvl)
        {
            return _tasks[lvl];
        }
    }
}