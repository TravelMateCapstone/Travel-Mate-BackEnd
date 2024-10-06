using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Utils.Reponse
{
    public class UserActivityDTO
    {
        public int UserId { get; set; }
        public int ActivityId { get; set; }
        public string? ActivityName { get; set; }
    }
}
