using UnityEngine;
using UnityEngine.UI;

namespace ITMO.Scripts
{
    public class TaskPanel : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private GameObject panel;
        [SerializeField] private AudioSource audioSource;
        private string str;

        private void Update()
        {
            if (Level.CurrentLevelName == null || !Level.GetLevelTask(Level.CurrentLevelName, out var task))
            {
                panel.SetActive(false);
                return;
            }
            
            str = task;

            const int baseGazeCounter = 250;
            const int baseSeconds = 5;

            if ((EyeInteraction.EyeGazeChangedCounter == baseGazeCounter ||
                 (int) Reference.Stopwatch.Elapsed.TotalSeconds == baseSeconds) &&
                Level.GetLevelTip1(Level.CurrentLevelName, out var tip))
            {
                str = $"{task}\n{tip}";
                audioSource.Play();
            }

            if ((EyeInteraction.EyeGazeChangedCounter == baseGazeCounter * 2 ||
                 (int) Reference.Stopwatch.Elapsed.TotalSeconds == baseSeconds * 2) &&
                Level.GetLevelTip2(Level.CurrentLevelName, out tip))
            {
                str = $"{task}\n{tip}";
                audioSource.Play();
            }

            if ((EyeInteraction.EyeGazeChangedCounter == baseGazeCounter * 3 ||
                 (int) Reference.Stopwatch.Elapsed.TotalSeconds == baseSeconds * 3) &&
                Level.GetLevelTip3(Level.CurrentLevelName, out tip))
            {
                str = $"{task}\n{tip}";
                audioSource.Play();
            }

            text.text = str;
            panel.SetActive(true);
        }
    }
}