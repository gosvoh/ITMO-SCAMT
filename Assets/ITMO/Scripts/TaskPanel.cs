using System;
using System.Text;
using Castle.Core.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace ITMO.Scripts
{
    public class TaskPanel : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private GameObject panel;
        
        private static AudioSource _audioSource;
        private static bool _panelUpdated;
        
        public static int TipLvl;
        public static int TipGazeCounter = 250;
        public static int TipTimeSeconds = 20;

        private string _str;
        private float _timer;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            Server.SendEvent.AddListener(EventHandler);
        }

        private void EventHandler()
        {
            TipLvl = 0;
            _timer = 0;
            _str = string.Empty;
            _panelUpdated = true;
        }

        private void OnEnable() => _panelUpdated = true;

        private void FixedUpdate()
        {
            if (Level.CurrentLevelName == null)
            {
                panel.SetActive(false);
                return;
            }

            _timer += Time.fixedDeltaTime;

            if (TipLvl != 1 && (EyeInteraction.EyeGazeChangedCounter == TipGazeCounter ||
                                (int) _timer == TipTimeSeconds * 1))
            {
                NextTip();
            }

            if (TipLvl != 2 && (EyeInteraction.EyeGazeChangedCounter == TipGazeCounter * 2 ||
                                (int) _timer == TipTimeSeconds * 2))
            {
                NextTip();
            }

            if (TipLvl != 3 && (EyeInteraction.EyeGazeChangedCounter == TipGazeCounter * 3 ||
                                (int) _timer == TipTimeSeconds * 3))
            {
                NextTip();
            }

            UpdateTask();

            text.text = _str;
            panel.SetActive(true);
        }

        private void UpdateTask()
        {
            if (!Level.GetLevelTask(Level.CurrentLevelName, out var task) || !_panelUpdated)
                return;

            if (!TryGetNextTip(out var tip))
            {
                if (_str.IsNullOrEmpty()) _str = task;
                _panelUpdated = false;
                return;
            }

            var sb = new StringBuilder();
            sb.Append(task).Append("\n\n").Append(tip);

            _str = sb.ToString();

            _panelUpdated = false;
        }

        public static void NextTip()
        {
            if (TipLvl >= 3) return;
            TipLvl++;
            _panelUpdated = true;
            _audioSource.Play();
        }

        private static bool TryGetNextTip(out string tip) => Level.GetLevelTip(Level.CurrentLevelName, TipLvl, out tip);
    }
}