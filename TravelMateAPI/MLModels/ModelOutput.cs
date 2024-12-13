using Microsoft.ML.Data;

namespace TravelMateAPI.MLModels
{
    public class ModelOutput
    {
        public string PredictedLabel { get; set; } // Địa điểm dự đoán

        //[ColumnName("PredictedLabels")]
        //public string PredictedLabels { get; set; } // Cột đầu ra từ mô hình
    }
}
