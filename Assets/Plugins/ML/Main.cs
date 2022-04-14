using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.ML;
using UnityEngine;

namespace ML
{
    public class Main : MonoBehaviour
    {
        private PredictionEngine<RealData, RealDataPredict> _predictionEngine;
        public void Awake()
        {
            var context = new MLContext();
            var model = context.Model.Load("Assets/ITMO/model.zip", out var dataViewSchema);
            _predictionEngine = context.Model.CreatePredictionEngine<RealData, RealDataPredict>(model);
        }

        private void FixedUpdate()
        {
            
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