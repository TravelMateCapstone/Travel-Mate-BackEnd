using BussinessObjects.Entities;
using BussinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class HomePhotoDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public HomePhotoDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HomePhoto>> GetAllHomePhotosAsync()
        {
            return await _dbContext.HomePhotos.Include(h => h.ApplicationUser).ToListAsync();
        }

        public async Task<HomePhoto> GetHomePhotoByIdAsync(int photoId)
        {
            return await _dbContext.HomePhotos.Include(h => h.ApplicationUser).FirstOrDefaultAsync(h => h.PhotoId == photoId);
        }

        public async Task<HomePhoto> AddHomePhotoAsync(HomePhoto newHomePhoto)
        {
            _dbContext.HomePhotos.Add(newHomePhoto);
            await _dbContext.SaveChangesAsync();
            return newHomePhoto;
        }

        public async Task UpdateHomePhotoAsync(HomePhoto updatedHomePhoto)
        {
            _dbContext.HomePhotos.Update(updatedHomePhoto);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteHomePhotoAsync(int photoId)
        {
            var homePhoto = await GetHomePhotoByIdAsync(photoId);
            if (homePhoto != null)
            {
                _dbContext.HomePhotos.Remove(homePhoto);
                await _dbContext.SaveChangesAsync();
            }
        }
    }


}
