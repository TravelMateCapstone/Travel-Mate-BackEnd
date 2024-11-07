using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Utils.Request
{
    public class AddHomePhotosRequest
    {
        public int UserHomeId { get; set; } // UserHomeId được truyền vào
        public List<string> PhotoUrls { get; set; } // Danh sách URL ảnh
    }

}
