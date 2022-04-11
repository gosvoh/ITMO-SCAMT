using System;
using Tobii.G2OM;
using UnityEngine;

namespace ITMO.Scripts
{
    public class Info : MonoBehaviour, IGazeFocusable
    {
        public int Index { set; get; }
        public GameObject Obj { set; get; }

        public void GazeFocusChanged(bool hasFocus)
        {
            if (!hasFocus) return;
            if (!Server.ServerConnected || EyeInteraction.Logger == null) return;
            if (Index == EyeInteraction.LastID) Debug.LogWarning("Index = EyeInteraction.LastID");
            EyeInteraction.LastID = Index;
            EyeInteraction.EyeGazeChangedCounter++;
            EyeInteraction.Logger.AddInfo($"Tobii|{DateTime.Now:HH:mm:ss.fff}|{Obj.transform.position}|{Index}");
            EyeInteraction.Logger.WriteInfo();
        }
    }
}