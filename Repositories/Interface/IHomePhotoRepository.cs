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
        Task<List<HomePhoto>> GetPhotosByHomeIdAsync(int userHomeId);
        Task<HomePhoto> AddHomePhotoAsync(HomePhoto newHomePhoto);
        Task<List<HomePhoto>> GetPhotosByUserIdAsync(int userId);
        Task<HomePhoto> AddHomePhotoByUserIdAsync(int userId, string photoUrl);
        // Thêm phương thức để lấy ảnh theo ID
        Task<HomePhoto> GetPhotoByIdAsync(int photoId);
        // Thêm phương thức để xóa ảnh theo ID
        Task DeleteHomePhotoAsync(int photoId);
    }

}
