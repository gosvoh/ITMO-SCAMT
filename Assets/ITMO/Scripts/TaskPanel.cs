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
            if (Level.CurrentLevelName == null || !Level.AllTasks.TryGetValue(Level.CurrentLevelName, out var task))
            {
                panel.SetActive(false);
                return;
            }

            text.text = task;
            panel.SetActive(true);
        }
    }
}