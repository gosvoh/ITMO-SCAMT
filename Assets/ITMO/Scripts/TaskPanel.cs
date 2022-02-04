using System;
using UnityEngine;
using UnityEngine.UI;

namespace ITMO.Scripts
{
    public class TaskPanel : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private GameObject panel;

        private void Update()
        {
            var task = Level.AllTasks?[Level.CurrentLevelName];
            if (task == null) panel.SetActive(false);
            text.text = task;
            panel.SetActive(true);
        }
    }
}