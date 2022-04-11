using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts
{
    public class FaceTracker : MonoBehaviour
    {
        public static Logger Logger;

        private int counter = -1;
        private Dictionary<LipShape_v2, float> shapes;
        private bool logHeaderSet;

        private void Start()
        {
            if (!SRanipal_Lip_Framework.Instance.EnableLip) enabled = false;
            Server.SendEvent.AddListener(EventHandler);
            Server.ConnectEvent.AddListener(ConnectEventHandler);
        }
        
        private static void EventHandler()
        {
            if (Logger == null) return;
            Logger.AddInfo(
                $"Level - {Level.CurrentLevelName}; Time spent in seconds - {Reference.Stopwatch.Elapsed.TotalSeconds}");
            Logger.WriteInfo();
        }
        
        private static void ConnectEventHandler()
        {
            Logger = new Logger("_faceTracker");
            var sb = new StringBuilder();
            sb.Append("timestamp|");
            foreach (var value in Enum.GetNames(typeof(LipShape_v2))) sb.Append(value).Append('|');
            sb.Remove(sb.Length - 1, 1);
            Logger.AddInfo(sb.ToString());
        }

        private void FixedUpdate()
        {
            if (!Server.ServerConnected || Logger == null) return;

            counter++;
            if (counter % 10 != 0) return;

            if (SRanipal_Lip_Framework.Status != SRanipal_Lip_Framework.FrameworkStatus.WORKING) return;

            SRanipal_Lip_v2.GetLipWeightings(out shapes);

            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("HH:mm:ss.fff")).Append("|");
            foreach (var value in shapes.Values) sb.Append(value).Append('|');
            sb.Remove(sb.Length - 1, 1);
            Logger.AddInfo(sb.ToString());
            Logger.WriteInfo();
        }
    }
}