using BusinessObjects;
using BusinessObjects.Entities;
using MongoDB.Driver;

namespace DataAccess
{
    public class ExtraFormDetailDAO
    {
        private readonly ApplicationDBContext _sqlContext;
        private readonly MongoDbContext _mongoContext;

        public ExtraFormDetailDAO(ApplicationDBContext context, MongoDbContext mongoContext)
        {
            _sqlContext = context;
            _mongoContext = mongoContext;
        }

        public async Task<IEnumerable<LocalExtraDetailForm>> GetAlls()
        {
            var collection = _mongoContext.GetCollection<LocalExtraDetailForm>("ExtraDetailForms");
            return collection.Find(_ => true).ToList();
        }

        public async Task<IEnumerable<TravelerExtraDetailForm>> GetAllRequests(int userId)
        {
            var collection = _mongoContext.GetCollection<TravelerExtraDetailForm>("TravelerExtraDetailForms");
            var filter = Builders<TravelerExtraDetailForm>.Filter.And(
        Builders<TravelerExtraDetailForm>.Filter.Or(
            Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.CreateById, userId),
            Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.TravelerId, userId)
        ),
        Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.RequestStatus, false));
            return await collection.Find(filter).ToListAsync();
        }

        public async Task<TravelerExtraDetailForm> GetRequest(string formId)
        {
            var collection = _mongoContext.GetCollection<TravelerExtraDetailForm>("TravelerExtraDetailForms");
            var filter = Builders<TravelerExtraDetailForm>.Filter.And(
                Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.Id, formId), // Filter by formId
                Builders<TravelerExtraDetailForm>.Filter.Or(
                    Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.RequestStatus, null),
                    Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.RequestStatus, false)
                )
            );

            return await collection.Find(filter).FirstOrDefaultAsync(); // Use FirstOrDefaultAsync for a single item
        }

        public async Task<TravelerExtraDetailForm> GetChat(string formId)
        {
            var collection = _mongoContext.GetCollection<TravelerExtraDetailForm>("TravelerExtraDetailForms");
            var filter = Builders<TravelerExtraDetailForm>.Filter.And(
                Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.Id, formId), // Filter by formId
                Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.RequestStatus, true));

            return await collection.Find(filter).FirstOrDefaultAsync(); // Use FirstOrDefaultAsync for a single item
        }

        public async Task<IEnumerable<TravelerExtraDetailForm>> GetAllChats(int userId)
        {
            var collection = _mongoContext.GetCollection<TravelerExtraDetailForm>("TravelerExtraDetailForms");
            var filter = Builders<TravelerExtraDetailForm>.Filter.And(
        Builders<TravelerExtraDetailForm>.Filter.Or(
            Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.CreateById, userId),
            Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.TravelerId, userId)
        ),
        Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.RequestStatus, true));
            return await collection.Find(filter).ToListAsync();
        }

        public async Task<LocalExtraDetailForm> GetById(string formId)
        {
            var collection = _mongoContext.GetCollection<LocalExtraDetailForm>("ExtraDetailForms");
            return collection.Find(form => form.Id == formId).FirstOrDefault();
        }

        public async Task<LocalExtraDetailForm> GetByUserId(int userId)
        {
            var collection = _mongoContext.GetCollection<LocalExtraDetailForm>("ExtraDetailForms");
            return collection.Find(form => form.CreateById == userId).FirstOrDefault();
        }

        public async Task ProcessRequest(TravelerExtraDetailForm form)
        {
            var collection = _mongoContext.GetCollection<TravelerExtraDetailForm>("TravelerExtraDetailForms");
            var filter = Builders<TravelerExtraDetailForm>.Filter.Eq(f => f.Id, form.Id);

            await collection.ReplaceOneAsync(filter, form);
        }

        public async Task AddAsync(LocalExtraDetailForm form)
        {
            var collection = _mongoContext.GetCollection<LocalExtraDetailForm>("ExtraDetailForms");
            collection.InsertOne(form);
        }

        public async Task UpdateAsync(string formId, LocalExtraDetailForm updatedForm)
        {
            var collection = _mongoContext.GetCollection<LocalExtraDetailForm>("ExtraDetailForms");
            var filter = Builders<LocalExtraDetailForm>.Filter.Eq(f => f.Id, formId);

            await collection.ReplaceOneAsync(filter, updatedForm);
        }

        // TRAVELER

        public async Task<IEnumerable<TravelerExtraDetailForm>> GetAllTravelerFormsAsync()
        {
            var collection = _mongoContext.GetCollection<TravelerExtraDetailForm>("TravelerExtraDetailForms");
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task<TravelerExtraDetailForm> GetTravelerFormByIdAsync(int localId, int travelerId)
        {
            var collection = _mongoContext.GetCollection<TravelerExtraDetailForm>("TravelerExtraDetailForms");
            return await collection.Find(form => form.CreateById == localId && form.TravelerId == travelerId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TravelerExtraDetailForm>> GetTravelerFormsByTravelerIdAsync(int travelerId)
        {
            var collection = _mongoContext.GetCollection<TravelerExtraDetailForm>("TravelerExtraDetailForms");
            return await collection.Find(form => form.TravelerId == travelerId).ToListAsync();
        }

        public async Task AddTravelerFormAsync(TravelerExtraDetailForm form)
        {
            var collection = _mongoContext.GetCollection<TravelerExtraDetailForm>("TravelerExtraDetailForms");
            await collection.InsertOneAsync(form);
        }

        public async Task UpdateTravelerFormAsync(int localId, int travelerId, TravelerExtraDetailForm updatedForm)
        {
            var collection = _mongoContext.GetCollection<TravelerExtraDetailForm>("TravelerExtraDetailForms");

            // Construct the filter to match the specific form
            var filter = Builders<TravelerExtraDetailForm>.Filter.And(
                Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.CreateById, localId),
                Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.TravelerId, travelerId)
            );

            // Replace the matched document with the updated form
            var result = await collection.ReplaceOneAsync(filter, updatedForm);
        }

        public async Task DeleteTravelerFormAsync(int localId, int travelerId)
        {
            var collection = _mongoContext.GetCollection<TravelerExtraDetailForm>("TravelerExtraDetailForms");
            var filter = Builders<TravelerExtraDetailForm>.Filter.And(
                Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.CreateById, localId),
                Builders<TravelerExtraDetailForm>.Filter.Eq(form => form.TravelerId, travelerId)
            );
            await collection.DeleteOneAsync(filter);
        }

    }
}