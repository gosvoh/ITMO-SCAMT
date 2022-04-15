using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.LightGbm;
using TMPro;
using UnityEngine;
using ViveSR.anipal.Eye;
using ViveSR.anipal.Lip;

namespace ITMO.Scripts
{
    public class ML : MonoBehaviour
    {
        private PredictionEngine<RealData, RealDataPredict> _predictionEngine;
        [SerializeField] private TMP_Text text;

        private int _counter;

        private readonly Dictionary<uint, string> _emotions = new Dictionary<uint, string>()
        {
            {0, "Neutral"},
            {1, "Sad"},
            {2, "Disgust"},
            {3, "Anger"},
            {4, "Surprise"},
            {5, "Fear"},
            {6, "Contempt"},
            {7, "Happiness"}
        };

        public void Awake()
        {
            var context = new MLContext();
            var model = context.Model.Load(Application.streamingAssetsPath + "/model.zip", out _);
            _predictionEngine = context.Model.CreatePredictionEngine<RealData, RealDataPredict>(model);
        }

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

            var data = RealData.ToRealData(eyeWeightings, lipWeightings);
            
            text.text = _emotions[_predictionEngine.Predict(data).emotion];
        }

        private static IList<RealData> GetData(string dirPath)
        {
            var files = Directory.GetFiles(dirPath);
            var retList = new List<RealData>();

            foreach (var file in files)
            {
                var streamReader = new StreamReader(file);
                var config = new CsvConfiguration(CultureInfo.CurrentCulture)
                {
                    Delimiter = "|"
                };
                var reader = new CsvReader(streamReader, config);
                retList.AddRange(reader.GetRecords<Data>().Cast<RealData>().ToList());
            }

            return retList;
        }
    }
}