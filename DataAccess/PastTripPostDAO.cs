using BusinessObjects;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class PastTripPostDAO
    {
        private readonly ApplicationDBContext _context;

        public PastTripPostDAO(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PastTripPost>> GetAllPostAsync()
        {
            return await _context.PastTripPosts
                .Include(p => p.Traveler)
                    .ThenInclude(t => t.Profiles)
                .Include(p => p.Local)
                    .ThenInclude(l => l.Profiles)
                .Include(p => p.PostPhotos)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PastTripPost>> GetAllPostOfUserAsync(int userId)
        {
            return await _context.PastTripPosts
                .Include(p => p.Traveler)
                    .ThenInclude(t => t.Profiles)
                .Include(p => p.Local)
                    .ThenInclude(l => l.Profiles)
                .Include(p => p.PostPhotos)
                .Where(p => p.LocalId == userId || p.TravelerId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<PastTripPost?> GetByIdAsync(int id)
        {
            return await _context.PastTripPosts
                .Include(p => p.Traveler)
                    .ThenInclude(t => t.Profiles)
                .Include(p => p.Local)
                    .ThenInclude(l => l.Profiles)
                .Include(p => p.PostPhotos)
                .FirstOrDefaultAsync(p => p.PastTripPostId == id);
        }

        public async Task DeleteAsync(int id)
        {
            var post = await _context.PastTripPosts.FindAsync(id);
            if (post != null)
            {
                _context.PastTripPosts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateTravelerPartAsync(PastTripPost post)
        {
            _context.PastTripPosts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLocalPartAsync(PastTripPost post)
        {
            _context.PastTripPosts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(PastTripPost post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            _context.PastTripPosts.Add(post);
            await _context.SaveChangesAsync();
        }


    }
}
