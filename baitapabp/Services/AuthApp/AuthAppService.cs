using baitapabp.Services.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp;

namespace baitapabp.Services.AuthApp
{
    public class AuthAppService : ApplicationService
    {
        private readonly IdentityUserManager _userManager;
        private readonly IConfiguration _config;

        public AuthAppService(IdentityUserManager userManager, IConfiguration config)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<LoginResultDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
            {
                throw new UserFriendlyException("Tài khoản không tồn tại.");
            }

            var check = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!check)
            {
                throw new UserFriendlyException("Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = await GenerateJwtTokenAsync(user, roles);
            return new LoginResultDto
            {
                Token = token.Token,
                ExpireAt = token.ExpireAt,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList()
            };
        }

        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> RegisterAsync(Volo.Abp.Account.RegisterDto dto)
        {
            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                throw new UserFriendlyException("Tên đăng nhập đã tồn tại.");
            if (!string.IsNullOrEmpty(dto.EmailAddress) && await _userManager.FindByEmailAsync(dto.EmailAddress) != null)
                throw new UserFriendlyException("Email đã được sử dụng.");

            var user = new IdentityUser(GuidGenerator.Create(), dto.UserName, dto.EmailAddress);
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Employee");
            }
            return result;
        }

        private async Task<(string Token, DateTime ExpireAt)> GenerateJwtTokenAsync(IdentityUser user, IList<string> roles)
        {
          var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var keyValue = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(keyValue) || keyValue.Length < 32)
                throw new Exception("JWT Key must be at least 32 characters long!");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expireAt = DateTime.UtcNow.AddHours(Convert.ToDouble(_config["Jwt:ExpireHours"] ?? "2"));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expireAt,
                signingCredentials: creds
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return (tokenStr, expireAt);
        }
    }

}
