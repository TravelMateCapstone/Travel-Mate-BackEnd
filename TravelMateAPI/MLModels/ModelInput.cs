using Microsoft.ML.Data;

namespace TravelMateAPI.MLModels
{
    public class ModelInput
    {
        [LoadColumn(0)] // Cột đầu tiên trong file CSV
        public string Query { get; set; }

        [LoadColumn(1)] // Cột thứ hai trong file CSV
        public string Location { get; set; }
    }
}
