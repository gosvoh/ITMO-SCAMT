using System;
using ITMO.Scripts.ML;
using UnityEngine;
using ViveSR.anipal.Eye;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts
{
    public class MLObject : MonoBehaviour
    {
        public static string CurrentEmotion = string.Empty;

        private void FixedUpdate()
        {
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING ||
                SRanipal_Lip_Framework.Status != SRanipal_Lip_Framework.FrameworkStatus.WORKING)
            {
                CurrentEmotion = "Not working";
                return;
            }
            
            var data = ModelInput.Transform(EyeTracker.Shapes, FaceTracker.Shapes);
            CurrentEmotion = ML.ML.Predict(data).PredictedLabel;
        }
    }
}