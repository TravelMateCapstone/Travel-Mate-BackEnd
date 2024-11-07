using BusinessObjects;
using BusinessObjects.Entities;
using DataAccess;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class HomePhotoRepository : IHomePhotoRepository
    {
        private readonly HomePhotoDAO _homePhotoDAO;

        public HomePhotoRepository(HomePhotoDAO homePhotoDAO)
        {
            _homePhotoDAO = homePhotoDAO;
        }

        // Lấy danh sách ảnh theo UserHomeId
        public async Task<List<HomePhoto>> GetPhotosByHomeIdAsync(int userHomeId)
        {
            return await _homePhotoDAO.GetPhotosByHomeIdAsync(userHomeId);
        }

        // Thêm ảnh mới
        public async Task<HomePhoto> AddHomePhotoAsync(HomePhoto newHomePhoto)
        {
            return await _homePhotoDAO.AddHomePhotoAsync(newHomePhoto);
        }

        // Lấy danh sách ảnh theo UserId
        public async Task<List<HomePhoto>> GetPhotosByUserIdAsync(int userId)
        {
            return await _homePhotoDAO.GetPhotosByUserIdAsync(userId);
        }

        // Thêm ảnh mới theo UserId
        public async Task<HomePhoto> AddHomePhotoByUserIdAsync(int userId, string photoUrl)
        {
            return await _homePhotoDAO.AddHomePhotoByUserIdAsync(userId, photoUrl);
        }
        public async Task<HomePhoto> GetPhotoByIdAsync(int photoId)
        {
            return await _homePhotoDAO.GetPhotoByIdAsync(photoId);
        }

        public async Task DeleteHomePhotoAsync(int photoId)
        {
            await _homePhotoDAO.DeleteHomePhotoAsync(photoId);
        }
        public async Task<List<HomePhoto>> AddHomePhotosAsync(List<HomePhoto> newHomePhotos)
        {
            return await _homePhotoDAO.AddHomePhotosAsync(newHomePhotos);
        }
    }



}
