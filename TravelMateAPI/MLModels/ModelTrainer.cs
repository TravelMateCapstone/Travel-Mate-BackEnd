using Microsoft.ML;

namespace TravelMateAPI.MLModels
{
    public class ModelTrainer
    {
        //private readonly MLContext _mlContext;

        //public ModelTrainer()
        //{
        //    _mlContext = new MLContext();
        //}

        //public void TrainModel()
        //{
        //    // Đường dẫn file CSV
        //    var dataPath = "./data.csv";

        //    // 1. Load dữ liệu
        //    var data = _mlContext.Data.LoadFromTextFile<ModelInput>(
        //        path: dataPath,
        //        hasHeader: true,
        //        separatorChar: ',');

        //    // 2. Xây dựng pipeline
        //    var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(ModelInput.Query))
        //        .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(ModelOutput.PredictedLocation)))
        //        .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
        //        .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLocation", "Label"));

        //    // 3. Huấn luyện mô hình
        //    var model = pipeline.Fit(data);

        //    // 4. Lưu mô hình
        //    _mlContext.Model.Save(model, data.Schema, "MLModels/model.zip");
        //}









        //public static void TrainModel()
        //{
        //    // Tạo MLContext
        //    var mlContext = new MLContext();

        //    //// Đường dẫn tệp
        //    //string basePath = Path.Combine(AppContext.BaseDirectory, "MLModels");
        //    ////string dataPath = Path.Combine(basePath, "data.csv");
        //    ////string modelPath = Path.Combine(basePath, "model.zip");
        //    //string dataPath = "./MLModels/data.csv";
        //    //string modelPath = "./MLModels/model.zip";

        //    string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MLModels");
        //    if (!Directory.Exists(basePath))
        //    {
        //        Directory.CreateDirectory(basePath);
        //    }
        //    string dataPath = Path.Combine(basePath, "data.csv");
        //    string modelPath = Path.Combine(basePath, "model.zip");


        //    // Load dữ liệu
        //    var data = mlContext.Data.LoadFromTextFile<ModelInput>(dataPath, hasHeader: true, separatorChar: ',');

        //    // Tạo pipeline
        //    var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", "Query")
        //        .Append(mlContext.Transforms.Conversion.MapValueToKey("Label", "Location"))
        //        .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
        //        .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        //    //// Xây dựng pipeline huấn luyện
        //    //var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(ModelInput.Query))
        //    //    .Append(mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(ModelInput.Location)))
        //    //    .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
        //    //    .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabels", "PredictedLabel"));

        //    // Huấn luyện mô hình
        //    var model = pipeline.Fit(data);

        //    // Lưu mô hình
        //    mlContext.Model.Save(model, data.Schema, modelPath);
        //    Console.WriteLine($"Model trained and saved to {modelPath}");
        //}









        private readonly string _dataFilePath;
        private readonly string _modelFilePath;
        private readonly BlobService _blobService;
        private readonly MLContext _mlContext;

        public ModelTrainer(BlobService blobService)
        {
            _blobService = blobService;
            string localFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MLModels");

            if (!Directory.Exists(localFolder))
            {
                Directory.CreateDirectory(localFolder);
            }

            _dataFilePath = Path.Combine(localFolder, "data.csv");
            _modelFilePath = Path.Combine(localFolder, "model.zip");

            // Tải tệp từ Blob Storage
            _blobService.DownloadBlobAsync("data.csv", _dataFilePath).Wait();

            _mlContext = new MLContext();
        }

        public void TrainModel()
        {
            IDataView dataView = _mlContext.Data.LoadFromTextFile<ModelInput>(
                path: _dataFilePath,
                hasHeader: true,
                separatorChar: ',');

            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(ModelInput.Query))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(ModelInput.Location)))
                .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var model = pipeline.Fit(dataView);

            _mlContext.Model.Save(model, dataView.Schema, _modelFilePath);

            // Tải mô hình lên Blob Storage
            _blobService.UploadBlobAsync("model.zip", _modelFilePath).Wait();
        }
    }
}
