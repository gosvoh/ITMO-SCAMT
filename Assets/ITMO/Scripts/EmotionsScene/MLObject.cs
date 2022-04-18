using ITMO.Scripts.ML;
using TMPro;
using UnityEngine;
using ViveSR.anipal.Eye;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts.EmotionsScene
{
    public class MLObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        
        private void FixedUpdate()
        {
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING ||
                SRanipal_Lip_Framework.Status != SRanipal_Lip_Framework.FrameworkStatus.WORKING)
            {
                text.text = "Not working";
                return;
            }

            SRanipal_Eye_v2.GetEyeWeightings(out var eyeWeightings);
            SRanipal_Lip_v2.GetLipWeightings(out var lipWeightings);

            var data = ModelInput.Transform(eyeWeightings, lipWeightings);

            text.text = ML.ML.Predict(data).PredictedLabel;
        }
    }
}