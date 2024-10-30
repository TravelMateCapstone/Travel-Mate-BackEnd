using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using BusinessObjects;
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

        // Lấy danh sách ảnh theo UserHomeId
        public async Task<List<HomePhoto>> GetPhotosByHomeIdAsync(int userHomeId)
        {
            return await _dbContext.HomePhotos
                .Where(photo => photo.UserHomeId == userHomeId)
                .ToListAsync();
        }

        // Thêm ảnh mới
        public async Task<HomePhoto> AddHomePhotoAsync(HomePhoto newHomePhoto)
        {
            _dbContext.HomePhotos.Add(newHomePhoto);
            await _dbContext.SaveChangesAsync();
            return newHomePhoto;
        }

        // Lấy danh sách ảnh theo UserId
        public async Task<List<HomePhoto>> GetPhotosByUserIdAsync(int userId)
        {
            var userHome = await _dbContext.UserHomes.FirstOrDefaultAsync(uh => uh.UserId == userId);
            if (userHome != null)
            {
                return await GetPhotosByHomeIdAsync(userHome.UserHomeId);
            }
            return new List<HomePhoto>();
        }

        // Thêm ảnh mới theo UserId
        public async Task<HomePhoto> AddHomePhotoByUserIdAsync(int userId, string photoUrl)
        {
            // Tìm UserHome dựa vào UserId
            var userHome = await _dbContext.UserHomes.FirstOrDefaultAsync(uh => uh.UserId == userId);

            if (userHome == null)
            {
                throw new Exception($"No UserHome found for UserId {userId}");
            }

            // Tạo HomePhoto mới
            var newHomePhoto = new HomePhoto
            {
                UserHomeId = userHome.UserHomeId,
                HomePhotoUrl = photoUrl
            };

            _dbContext.HomePhotos.Add(newHomePhoto);
            await _dbContext.SaveChangesAsync();

            return newHomePhoto;
        }
        // Lấy ảnh theo ID
        public async Task<HomePhoto> GetPhotoByIdAsync(int photoId)
        {
            return await _dbContext.HomePhotos.FindAsync(photoId);
        }

        // Xóa ảnh theo ID
        public async Task DeleteHomePhotoAsync(int photoId)
        {
            var photo = await GetPhotoByIdAsync(photoId);
            if (photo != null)
            {
                _dbContext.HomePhotos.Remove(photo);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Photo with ID {photoId} not found.");
            }
        }
    }


}
