using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ViveSR.anipal.Eye;

namespace ITMO.Scripts
{
    public class EyeTracker : MonoBehaviour
    {
        public static Logger Logger;

        private bool logHeaderSet;
        private int counter = -1;

        internal static Dictionary<EyeShape_v2, float> Shapes;

        private void Start()
        {
            if (!SRanipal_Eye_Framework.Instance.EnableEye) enabled = false;
            Server.SendEvent.AddListener(EventHandler);
        }

        private void FixedUpdate()
        {
            if (!Server.ServerConnected || Logger == null) return;

            counter++;
            if (counter % 10 != 0) return;

            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING) return;

            SRanipal_Eye_v2.GetEyeWeightings(out Shapes);
            
            var sb = new StringBuilder();
            if (!logHeaderSet)
            {
                sb.Append("timestamp|");
                foreach (var value in Enum.GetNames(typeof(EyeShape_v2))) sb.Append($"{value}|");
                sb.Append("l_pupil_diameter|r_pupil_diameter");
                Logger.AddInfo(sb.ToString());
                sb.Clear();
                logHeaderSet = true;
            }
            sb.Append(DateTime.Now.ToString("HH:mm:ss.fff")).Append("|");
            foreach (var value in Shapes.Values) sb.Append($"{value}|");
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.LEFT, out var lDiam);
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.RIGHT, out var rDiam);
            sb.Append($"{lDiam}|{rDiam}");
            Logger.AddInfo(sb.ToString());
            Logger.WriteInfo();
        }

        private static void EventHandler()
        {
            if (Logger == null) return;
            Logger.AddInfo(
                $"Level - {Level.CurrentLevelName}; Time spent in seconds - {Reference.Stopwatch.Elapsed.TotalSeconds}");
            Logger.WriteInfo();
        }
    }
}