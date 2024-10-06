using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Utils.Reponse
{
    public class UserLocationDTO
    {
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
    }
}
