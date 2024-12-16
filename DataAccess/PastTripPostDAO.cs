using BusinessObjects;
using BusinessObjects.Entities;
using MongoDB.Driver;

namespace DataAccess
{
    public class PastTripPostDAO
    {
        private readonly IMongoCollection<PastTripPost> _mongoContext;
        public PastTripPostDAO(ApplicationDBContext context, MongoDbContext mongoContext)
        {
            _mongoContext = mongoContext.GetCollection<PastTripPost>("PastTripPosts");
        }

        //get all post related to user
        public async Task<List<PastTripPost>> GetAllPostsAsync(int userId)
        {
            return (await _mongoContext.Find(t => t.TravelerId == userId || t.LocalId == userId).ToListAsync()).OrderByDescending(p => p.CreatedAt).ToList();
        }
        public async Task<PastTripPost?> GetPostByIdAsync(string id)
        {
            return await _mongoContext.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        //traveler creator post
        public async Task AddPostAsync(PastTripPost post)
        {
            await _mongoContext.InsertOneAsync(post);
        }

        //traveler, local update comment
        public async Task UpdatePostAsync(string id, PastTripPost post)
        {
            await _mongoContext.ReplaceOneAsync(p => p.Id == id, post);
        }

        //traveler delete post
        public async Task DeletePostAsync(string id)
        {
            await _mongoContext.DeleteOneAsync(p => p.Id == id);
        }

    }
}
