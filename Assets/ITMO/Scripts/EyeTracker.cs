using Tobii.XR;
using UnityEngine;

namespace ITMO.Scripts
{
    [DefaultExecutionOrder(-10)]
    public class EyeTracker : MonoBehaviour
    {
        public TobiiXR_Settings settings;
        private void Awake()
        {
            // settings = new TobiiXR_Settings(true);
            TobiiXR.Start(settings);
        }
    }
}