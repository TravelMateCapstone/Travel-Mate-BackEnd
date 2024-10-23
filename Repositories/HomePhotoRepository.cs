using BussinessObjects.Entities;
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

        public async Task<List<HomePhoto>> GetAllHomePhotosAsync()
        {
            return await _homePhotoDAO.GetAllHomePhotosAsync();
        }

        public async Task<HomePhoto> GetHomePhotoByIdAsync(int photoId)
        {
            return await _homePhotoDAO.GetHomePhotoByIdAsync(photoId);
        }

        public async Task<HomePhoto> AddHomePhotoAsync(HomePhoto newHomePhoto)
        {
            return await _homePhotoDAO.AddHomePhotoAsync(newHomePhoto);
        }

        public async Task UpdateHomePhotoAsync(HomePhoto updatedHomePhoto)
        {
            await _homePhotoDAO.UpdateHomePhotoAsync(updatedHomePhoto);
        }

        public async Task DeleteHomePhotoAsync(int photoId)
        {
            await _homePhotoDAO.DeleteHomePhotoAsync(photoId);
        }
    }

}
