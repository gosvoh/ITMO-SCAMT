using System;
using System.IO;
using Microsoft.ML;
using TMPro;
using UnityEngine;
using ViveSR.anipal.Eye;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts.ML
{
    public class ML : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private readonly string _mlNetModelPath = Path.GetFullPath(Application.streamingAssetsPath + "\\MLModel.zip");
        private Lazy<PredictionEngine<ModelInput, ModelOutput>> _predictEngine;

        private int _counter;

        public void Awake() => _predictEngine =
            new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

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

            text.text = Predict(data).PredictedLabel;
        }

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        private ModelOutput Predict(ModelInput input) => _predictEngine.Value.Predict(input);

        private PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            var mlModel = mlContext.Model.Load(_mlNetModelPath, out _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }
    }
}