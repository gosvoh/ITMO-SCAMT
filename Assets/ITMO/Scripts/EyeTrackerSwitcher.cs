using System;
using Tobii.XR;
using UnityEngine;
using ViveSR.anipal.Eye;

namespace ITMO.Scripts
{
    public class EyeTrackerSwitcher : MonoBehaviour
    {
        public static bool TobiiEnabled = false;
        private bool _tobiiEnabled = false;

        private void FixedUpdate()
        {
            if (TobiiEnabled) EnableTobii();
            else DisableTobii();
        }

        private void DisableTobii()
        {
            if (!_tobiiEnabled) return;
            TobiiXR.Stop();
            SRanipal_Eye_Framework.Instance.EnableEye = true;
            SRanipal_Eye_Framework.Instance.StartFramework();
            Debug.LogWarning("Tobii disabled");
            _tobiiEnabled = false;
        }

        private void EnableTobii()
        {
            if (_tobiiEnabled) return;
            SRanipal_Eye_Framework.Instance.EnableEye = false;
            SRanipal_Eye_Framework.Instance.StopFramework();
            var success = TobiiXR.Start(new TobiiXR_Settings());
            Debug.LogWarning("Tobii enabled? " + success);
            _tobiiEnabled = true;
        }
    }
}