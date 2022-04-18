using System;
using System.IO;
using Microsoft.ML;
using TMPro;
using UnityEngine;
using ViveSR.anipal.Eye;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts.ML
{
    public class ML
    {
        private static readonly string MlNetModelPath = Path.GetFullPath(Application.streamingAssetsPath + "\\MLModel.zip");
        private static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine =
            new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input) => PredictEngine.Value.Predict(input);

        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            var mlModel = mlContext.Model.Load(MlNetModelPath, out _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }
    }
}