using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts
{
    public class FaceTracker : MonoBehaviour
    {
        public static Dictionary<LipShape_v2, float> Shapes;

        private static Logger _logger;
        private int _counter = -1;

        private void Start()
        {
            if (!SRanipal_Lip_Framework.Instance.EnableLip) enabled = false;
            Server.SendEvent.AddListener(EventHandler);
        }
        
        private static void EventHandler()
        {
            if (_logger == null)
            {
                _logger = new Logger("_faceTracker");
                var sb = new StringBuilder();
                sb.Append("timestamp|");
                foreach (var value in Enum.GetNames(typeof(LipShape_v2))) sb.Append(value).Append('|');
                sb.Remove(sb.Length - 1, 1);
                _logger.AddInfo(sb.ToString());
            }
            
            if (!Server.ServerConnected) return;

            _logger.AddInfo(
                $"Level - {Level.CurrentLevelName}; Time spent in seconds - {Reference.Stopwatch.Elapsed.TotalSeconds}");
            _logger.WriteInfo();
        }

        private void FixedUpdate()
        {
            if (!Server.ServerConnected || _logger == null) return;

            _counter++;
            if (_counter % 10 != 0) return;

            if (SRanipal_Lip_Framework.Status != SRanipal_Lip_Framework.FrameworkStatus.WORKING) return;

            SRanipal_Lip_v2.GetLipWeightings(out Shapes);

            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("HH:mm:ss.fff")).Append("|");
            foreach (var value in Shapes.Values) sb.Append(value).Append('|');
            sb.Remove(sb.Length - 1, 1);
            _logger.AddInfo(sb.ToString());
            _logger.WriteInfo();
        }
    }
}