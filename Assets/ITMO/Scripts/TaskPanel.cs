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
            try
            {
                string task = Level.GetLevelTask(Level.CurrentLevelName);
                text.text = task;
                panel.SetActive(true);
            }
            catch (Exception e)
            {
                panel.SetActive(false);
            }
        }
    }
}