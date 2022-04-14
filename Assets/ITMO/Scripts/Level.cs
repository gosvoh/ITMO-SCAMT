using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ITMO.Scripts
{
    public static class Level
    {
        /// <summary>
        /// Dictionary that contains all levels. <br />
        /// Key - name of the .pdb file <br />
        /// Value - path to the .pdb file
        /// </summary>
        private static Dictionary<string, string> _allLevels;

        /// <summary>
        /// Dictionary that contains all level's tasks. <br />
        /// Key - name of the .pdb file <br />
        /// Value - content of the *level_name*.txt
        /// </summary>
        private static Dictionary<string, string> _allTasks;

        private static Dictionary<int, Dictionary<string, string>> _allTasksTip;

        public static LinkedList<string> LevelNamesList;

        public static Dictionary<string, LinkedList<string>> DifficultyLevels;

        public static string CurrentLevelName;

        public static LinkedListNode<string> CurrentLevelNode;

        public static void Initialize()
        {
            try
            {
                GetFiles();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static void GetFiles()
        {
            _allLevels = new Dictionary<string, string>();
            _allTasks = new Dictionary<string, string>();
            _allTasksTip = new Dictionary<int, Dictionary<string, string>>
            {
                [1] = new Dictionary<string, string>(),
                [2] = new Dictionary<string, string>(),
                [3] = new Dictionary<string, string>()
            };
            LevelNamesList = new LinkedList<string>();
            DifficultyLevels = new Dictionary<string, LinkedList<string>>();
            var mainDir = $"{Application.dataPath}\\..\\Molecules";
            var dirFiles = Directory.EnumerateFiles(mainDir);
            foreach (var s in dirFiles)
            {
                var fn = Path.GetFileName(s).Split('.');
                switch (fn[fn.Length - 1])
                {
                    case "pdb":
                        _allLevels.Add(fn[0], s);
                        break;
                    case "txt":
                        var lines = File.ReadAllLines(s);
                        _allTasks.Add(fn[0], lines[0]);
                        if (lines.Length > 1)
                        {
                            _allTasksTip[1][fn[0]] = lines[1];
                            _allTasksTip[2][fn[0]] = lines[2];
                            _allTasksTip[3][fn[0]] = lines[3];
                        }

                        break;
                }
            }

            var dirs = Directory.EnumerateDirectories(mainDir).Select(s => new DirectoryInfo(s).Name);
            foreach (var level in dirs)
            {
                if (!DifficultyLevels.ContainsKey(level)) DifficultyLevels[level] = new LinkedList<string>();
                dirFiles = Directory.EnumerateFiles($"{mainDir}\\{level}\\");
                foreach (var s in dirFiles)
                {
                    var fn = Path.GetFileName(s).Split('.');
                    switch (fn[fn.Length - 1])
                    {
                        case "pdb":
                            _allLevels[fn[0]] = s;
                            DifficultyLevels[level].AddLast(s);
                            break;
                        case "txt":
                            var lines = File.ReadAllLines(s);
                            _allTasks.Add(fn[0], lines[0]);
                            if (lines.Length > 1)
                            {
                                _allTasksTip[1][fn[0]] = lines[1];
                                _allTasksTip[2][fn[0]] = lines[2];
                                _allTasksTip[3][fn[0]] = lines[3];
                            }

                            break;
                    }
                }
            }

            LevelNamesList = new LinkedList<string>(_allLevels.Keys);
        }

        public static string GetLevelPath(string lvl) => _allLevels[lvl];

        public static bool GetLevelTask(string lvl, out string task) => _allTasks.TryGetValue(lvl, out task);

        public static bool GetLevelTip(string lvl, int tipLvl, out string tip)
        {
            var t = _allTasksTip.TryGetValue(tipLvl, out var dict);
            if (t) return dict.TryGetValue(lvl, out tip);
            tip = string.Empty;
            return false;

        }
    }
}