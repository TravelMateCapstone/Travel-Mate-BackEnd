using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Utils.Response
{
    public class ApplicationUserDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public ProfileDTO Profile { get; set; }
        public List<string> Roles { get; set; }
    }

    public class ProfileDTO
    {
        public int ProfileId { get; set; }
        public string Address { get; set; }
    }

}
