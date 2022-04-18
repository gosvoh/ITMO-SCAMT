using Microsoft.ML.Data;

namespace ITMO.Scripts.ML
{
    public class ModelOutput
    {
        [ColumnName(@"PredictedLabel")] public string PredictedLabel { get; set; }

        [ColumnName(@"Score")] public float[] Score { get; set; }
    }
}