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

        private static Dictionary<EyeShape_v2, float> _shapes;

        private void Start()
        {
            if (!SRanipal_Eye_Framework.Instance.EnableEye) enabled = false;
            Server.SendEvent.AddListener(EventHandler);
            Server.ConnectEvent.AddListener(ConnectEventHandler);
        }

        private static void ConnectEventHandler()
        {
            Logger = new Logger("_eyeTracker");
            var sb = new StringBuilder();
            sb.Append("timestamp|");
            foreach (var value in Enum.GetNames(typeof(EyeShape_v2))) sb.Append(value).Append('|');
            sb.Append("l_pupil_diameter|r_pupil_diameter");
            Logger.AddInfo(sb.ToString());
        }

        private void FixedUpdate()
        {
            if (!Server.ServerConnected || Logger == null) return;

            counter++;
            if (counter % 10 != 0) return;

            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

            SRanipal_Eye_v2.GetEyeWeightings(out _shapes);

            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("HH:mm:ss.fff")).Append("|");
            foreach (var value in _shapes.Values) sb.Append(value).Append('|');
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.LEFT, out var lDiam);
            SRanipal_Eye_v2.GetPupilDiameter(EyeIndex.RIGHT, out var rDiam);
            sb.Append(lDiam).Append('|').Append(rDiam);
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