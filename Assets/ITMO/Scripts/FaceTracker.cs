using System;
using System.Collections.Generic;
using System.Text;
using Plugins.Narupa.Core;
using UnityEngine;
using ViveSR;
using ViveSR.anipal;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts.SRanipal
{
    public class FaceTracker : MonoBehaviour
    {
        private Dictionary<LipShape_v2, float> lipWeightings;
        private Dictionary<LipShape_v2, float> oldLipWeightings;
        private int counter = -1;

        public enum FrameworkStatus
        {
            STOP,
            START,
            WORKING,
            ERROR
        }

        public static FrameworkStatus Status { get; protected set; }

        private void Start()
        {
            var result = SRanipal_API.Initial(SRanipal_Lip_v2.ANIPAL_TYPE_LIP_V2, IntPtr.Zero);
            if (result == Error.WORK)
            {
                Debug.Log("[SRanipal] Initial Version 2 Lip : " + result);
                Status = FrameworkStatus.WORKING;
            }
            else
            {
                Debug.LogError("[SRanipal] Initial Version 2 Lip : " + result);
                Status = FrameworkStatus.ERROR;
            }
        }

        private void FixedUpdate()
        {
            if (Status != FrameworkStatus.WORKING || !Server.ServerConnected || Reference.FaceLogger == null) return;

            counter++;
            if (counter % 5 != 0) return;
            
            SRanipal_Lip_v2.GetLipWeightings(out lipWeightings);
            if (oldLipWeightings == null)
            {
                oldLipWeightings = new Dictionary<LipShape_v2, float>(lipWeightings);
                return;
            }

            var sb = new StringBuilder();
            foreach (var (key, value) in lipWeightings)
            {
                if (value < 0.01f || Math.Abs(oldLipWeightings[key] - value) < 0.01f) continue;
                sb.Append(Enum.GetName(typeof(LipShape_v2), key))
                    .Append(" - ")
                    .Append(value)
                    .Append("; ");
            }
            if (sb.Length == 0) return;
            Reference.FaceLogger.AddInfo(sb.ToString());
            oldLipWeightings = new Dictionary<LipShape_v2, float>(lipWeightings);
        }

        private void OnDestroy()
        {
            if (Status != FrameworkStatus.STOP)
            {
                var result = SRanipal_API.Release(SRanipal_Lip_v2.ANIPAL_TYPE_LIP_V2);
                if (result == Error.WORK) Debug.Log("[SRanipal] Release Version 2 Lip : " + result);
                else Debug.LogError("[SRanipal] Release Version 2 Lip : " + result);
                Reference.FaceLogger.WriteInfo();
            }
            else
                Debug.Log("[SRanipal] Stop Framework : module not on");

            Status = FrameworkStatus.STOP;
        }
    }
}