
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Utils.Request
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Họ và Tên.")]
        //[RegularExpression(@"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơưƯĂĨŨĂÂĂĐêùăđãêươỳýỵỷỹ\s]+$",
        //                    ErrorMessage = "Họ tên chỉ chứa ký tự chữ và dấu cách, không chứa ký tự đặc biệt.")]
        [RegularExpression(@"^[\p{L} ]+$", ErrorMessage = "Họ tên chỉ chứa ký tự chữ và dấu cách, không chứa chữ số hoặc ký tự đặc biệt.")]
        public string FullName { get; set; }

    }
}
