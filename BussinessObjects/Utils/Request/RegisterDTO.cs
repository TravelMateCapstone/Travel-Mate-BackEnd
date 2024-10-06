
namespace BussinessObjects.Utils.Request
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }  // Nếu bạn muốn thu thập thêm thông tin như tên đầy đủ
    }
}
