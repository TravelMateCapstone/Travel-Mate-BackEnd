using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IHomePhotoRepository
    {
        Task<List<HomePhoto>> GetAllHomePhotosAsync();
        Task<HomePhoto> GetHomePhotoByIdAsync(int photoId);
        Task<HomePhoto> AddHomePhotoAsync(HomePhoto newHomePhoto);
        Task UpdateHomePhotoAsync(HomePhoto updatedHomePhoto);
        Task DeleteHomePhotoAsync(int photoId);
    }

}
