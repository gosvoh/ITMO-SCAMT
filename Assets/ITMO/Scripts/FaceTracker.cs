using System;
using System.Collections.Generic;
using System.Text;
using Castle.Core.Internal;
using Plugins.Narupa.Core;
using UnityEngine;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts.SRanipal
{
    public class FaceTracker : MonoBehaviour
    {
        public static Logger Logger;

        private int counter = -1;
        private Dictionary<LipShape_v2, float> _shapes;
        private bool log;

        private void Start()
        {
            if (!SRanipal_Lip_Framework.Instance.EnableLip) enabled = false;
            Server.SendEvent.AddListener(EventHandler);
        }

        private void FixedUpdate()
        {
            if (!Server.ServerConnected || Logger == null) return;

            counter++;
            if (counter % 10 != 0) return;

            if (SRanipal_Lip_Framework.Status != SRanipal_Lip_Framework.FrameworkStatus.WORKING) return;

            SRanipal_Lip_v2.GetLipWeightings(out _shapes);
            
            var sb = new StringBuilder();
            if (!log)
            {
                sb.Append("timestamp|");
                foreach (var value in Enum.GetNames(typeof(LipShape_v2))) sb.Append($"{value}|");
                sb.Remove(sb.Length - 1, 1);
                Logger.AddInfo(sb.ToString());
                sb.Clear();
                log = true;
            }
            sb.Append(DateTime.Now.ToString("HH:mm:ss.fff")).Append("|");
            foreach (var value in _shapes.Values) sb.Append($"{value}|");
            sb.Remove(sb.Length - 1, 1);
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