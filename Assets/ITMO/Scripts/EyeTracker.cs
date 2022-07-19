using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ViveSR.anipal.Eye;

namespace ITMO.Scripts
{
    public class EyeTracker : MonoBehaviour
    {
        private Logger _logger;
        private int _counter = -1;

        public static Dictionary<EyeShape_v2, float> Shapes;

        private void Awake()
        {
            Server.SendEvent.AddListener(EventHandler);
            Server.ConnectionEvent.AddListener(ConnectionHandler);
        }
        
        private void EventHandler()
        {
            if (!Server.ServerConnected) return;

            _logger.AddInfo(
                $"Level - {Level.CurrentLevelName}; Time spent in seconds - {Reference.Stopwatch.Elapsed.TotalSeconds}");
            _logger.WriteInfo();
        }

        private void ConnectionHandler()
        {
            _logger = new Logger("_eyeTracker");
            var sb = new StringBuilder();
            sb.Append("timestamp|");
            foreach (var value in Enum.GetNames(typeof(EyeShape_v2))) sb.Append(value).Append('|');
            sb.Append("l_pupil_diameter|r_pupil_diameter");
            _logger.AddInfo(sb.ToString());
        }

        private void FixedUpdate()
        {
            if (!Server.ServerConnected || _logger == null) return;
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING) return;

            if (_counter++ % Reference.TrackersTick != 0) return;
            
            SRanipal_Eye_v2.GetEyeWeightings(out Shapes);

            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("HH:mm:ss.fff")).Append("|");
            foreach (var value in Shapes.Values) sb.Append(value).Append('|');
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.LEFT, out var lDiam);
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.RIGHT, out var rDiam);
            sb.Append(lDiam).Append('|').Append(rDiam);
            _logger.AddInfo(sb.ToString());
            _logger.WriteInfo();
        }
    }
}