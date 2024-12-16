using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.ML;

namespace TravelMateAPI.MLModels
{
    public class ModelPredictor
    {
        //private readonly MLContext _mlContext;
        //private readonly PredictionEngine<ModelInput, ModelOutput> _predictionEngine;

        //public ModelPredictor()
        //{
        //    _mlContext = new MLContext();

        //    // Tải mô hình đã được huấn luyện
        //    var model = _mlContext.Model.Load("MLModels/model.zip", out _);
        //    _predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);
        //}

        //public string Predict(string query)
        //{
        //    var input = new ModelInput { Query = query };
        //    var prediction = _predictionEngine.Predict(input);
        //    return prediction.PredictedLocation;
        //}

        //public static string PredictLocation(string userQuery)
        //{
        //    // Tạo MLContext
        //    var mlContext = new MLContext();

        //    // Đường dẫn model
        //    string basePath = Path.Combine(AppContext.BaseDirectory, "MLModels");
        //    string modelPath = Path.Combine(basePath, "model.zip");

        //    // Kiểm tra nếu model tồn tại
        //    if (!File.Exists(modelPath))
        //    {
        //        throw new FileNotFoundException($"Could not find model at {modelPath}. Please train the model first.");
        //    }

        //    // Load mô hình
        //    var model = mlContext.Model.Load(modelPath, out var schema);

        //    // Tạo PredictionEngine
        //    var predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);

        //    // Thực hiện dự đoán
        //    var prediction = predictionEngine.Predict(new ModelInput { Query = userQuery });

        //    return prediction.PredictedLabel;
        //}

        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly ApplicationDBContext _dbContext;
        private readonly BlobService _blobService;

        //private readonly string _modelPath = Path.Combine(AppContext.BaseDirectory, "./MLModels/model.zip");
        //private readonly string _modelPath = "./MLModels/model.zip";
        private readonly string _modelFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "./MLModels/model.zip");

        public ModelPredictor(ApplicationDBContext dbContext, BlobService blobService)
        {
            _mlContext = new MLContext();
            _dbContext = dbContext;
            _blobService = blobService;

            // Load mô hình
            //_model = _mlContext.Model.Load(_modelPath, out var modelInputSchema);
            

            // Tải mô hình từ Blob Storage
            blobService.DownloadBlobAsync("model.zip", _modelFilePath).Wait();
            using var stream = File.OpenRead(_modelFilePath);
            _model = _mlContext.Model.Load(stream, out _);

        }


        //public List<string> PredictLocation(string query)
        public string PredictLocation(string query)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_model);

            // Dự đoán kết quả location
            var prediction = predictionEngine.Predict(new ModelInput { Query = query });

            var predictedLocation = prediction.PredictedLabel;

            return predictedLocation;

            //// Trả về danh sách các địa điểm có độ tương đồng cao nhất
            //return prediction.PredictedLabels.Take(2).ToList(); // Lấy 2 địa điểm phù hợp nhất

            //Trả về danh sách các địa điểm dự đoán
            //return new List<string> { prediction.PredictedLabels };


        }

        //public Location GetLocationDetails(string predictedLocation)
        public List<Location> GetLocationsDetails(string predictedLocations)
        {
            //// Tìm kiếm địa điểm trong bảng Location
            //var locations = _dbContext.Locations
            //    .FirstOrDefault(l => l.LocationName.Equals(predictedLocation, StringComparison.OrdinalIgnoreCase));


            //// Thực hiện so sánh không phân biệt chữ hoa chữ thường
            //var locations = _dbContext.Locations
            //    .FirstOrDefault(l => l.LocationName.ToLower() == predictedLocation.ToLower());

            // Tìm kiếm các địa điểm trong cơ sở dữ liệu
            var locations = _dbContext.Locations
                .Where(l => predictedLocations.Contains(l.LocationName))
                .ToList();

            return locations;
        }
    }
}
