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
        private static readonly Dictionary<string, string> AllLevels = new Dictionary<string, string>();

        /// <summary>
        /// Dictionary that contains all level's tasks. <br />
        /// Key - name of the .pdb file <br />
        /// Value - content of the *level_name*.txt
        /// </summary>
        public static readonly Dictionary<string, string> AllTasks = new Dictionary<string, string>();

        public static LinkedList<string> LevelNamesList = new LinkedList<string>();

        public static readonly Dictionary<string, LinkedList<string>> DifficultyLevels =
            new Dictionary<string, LinkedList<string>>
            {
                ["Easy"] = new LinkedList<string>(),
                ["Medium"] = new LinkedList<string>(),
                ["Hard"] = new LinkedList<string>(),
                ["Hardcore"] = new LinkedList<string>()
            };

        public static string CurrentLevelName;

        public static LinkedListNode<string> CurrentLevelNode;

        public static void Initialize() => GetFiles();

        private static void GetFiles()
        {
            var mainDir = Application.streamingAssetsPath + "\\Molecules";
            var dirFiles = Directory.EnumerateFiles(mainDir);
            foreach (var s in dirFiles)
            {
                var fn = Path.GetFileName(s).Split('.');
                switch (fn[fn.Length - 1])
                {
                    case "pdb":
                        AllLevels.Add(fn[0], s);
                        break;
                    case "txt":
                        string line;
                        if ((line = File.ReadAllLines(s)[0]).Length > 0) AllTasks.Add(fn[0], line);
                        break;
                }
            }

            foreach (var level in DifficultyLevels.Keys)
            {
                dirFiles = Directory.EnumerateFiles($"{mainDir}\\{level}\\");
                foreach (var s in dirFiles)
                {
                    var fn = Path.GetFileName(s).Split('.');
                    switch (fn[fn.Length - 1])
                    {
                        case "pdb":
                            AllLevels[fn[0]] = s;
                            DifficultyLevels[level].AddLast(s);
                            break;
                        case "txt":
                            string line;
                            if ((line = File.ReadAllLines(s)[0]).Length > 0) /*_tasks.Add(fn[0], line);*/
                                AllTasks[fn[0]] = line;
                            break;
                    }
                }
            }

            LevelNamesList = new LinkedList<string>(AllLevels.Keys);
        }

        public static string GetLevelPath(string lvl) => AllLevels[lvl];

        public static string GetLevelTask(string lvl) => AllTasks[lvl];
    }
}