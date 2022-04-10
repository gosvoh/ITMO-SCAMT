using System;
using Tobii.XR;
using UnityEngine;
using ViveSR.anipal.Eye;

namespace ITMO.Scripts
{
    public class FaceTrackerSwitcher : MonoBehaviour
    {
        public static bool TobiiEnabled = false;

        private void FixedUpdate()
        {
            if (TobiiEnabled) EnableTobii();
            else DisableTobii();
        }

        private void DisableTobii()
        {
            TobiiXR.Stop();
            SRanipal_Eye_Framework.Instance.StartFramework();
            Debug.Log("Tobii disabled");
        }

        private void EnableTobii()
        {
            SRanipal_Eye_Framework.Instance.StopFramework();
            TobiiXR.Start(new TobiiXR_Settings(EyeInteraction.PrefabLayer));
            Debug.Log("Tobii enabled");
        }
    }
}