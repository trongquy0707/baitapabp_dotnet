using System.ComponentModel.DataAnnotations;

namespace baitapabp.Services.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage =  "Tên đăng nhập không được để trống")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginResultDto
    {
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }

    public class RegisterResultDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
