using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BusinessObjects
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            ////var connectionString = configuration.GetConnectionString("MongoDbConnection");
            //var mongoClient = new MongoClient("mongodb+srv://hdangtran:Tranhaidang2@@travelmate.pxwv7.mongodb.net/?retryWrites=true&w=majority&appName=TravelMate");


            const string connectionUri = "mongodb+srv://hdangtran:Tranhaidang@travelmate.pxwv7.mongodb.net/?retryWrites=true&w=majority&appName=TravelMate";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            // Set the ServerApi field of the settings object to set the version of the Stable API on the client
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            // Create a new client and connect to the server
            var client = new MongoClient(settings);

            _database = client.GetDatabase("TravelMate");
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
