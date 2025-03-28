using ConferencesManagement.Constants;
using ConferencesManagement.Utils;

using ConferencesManagementAPI.DAO.Repositories;
using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.IRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConferencesManagementService
{
    public class AuthService : IAuthService
    {
        private readonly DelegatesRepositories _delegateRepository;
        private readonly IConfiguration _configuration;

        public AuthService(DelegatesRepositories delegateRepository, IConfiguration configuration)
        {
            _delegateRepository = delegateRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO?> AuthenticateAsync(AuthRequestDTO request)
        {
            try
            {
                var user = await _delegateRepository.GetDelegatesByEmail(request.Email);

                if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
                    return null;

                var token = GenerateJwtToken(user);
                return new AuthResponseDTO { Token = token, Expiration = DateTime.UtcNow.AddHours(2) };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] AuthenticateAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<GeneralResponseDTO> RegisterDelegateAsync(RegisterDelegatesDTO registerDelegates)
        {
            try
            {
                // Kiểm tra email đã tồn tại chưa
                var existingUser = await _delegateRepository.FirstOrDefaultAsync(a => a.Email == registerDelegates.Email);
                if (existingUser != null)
                    return new GeneralResponseDTO { isSuccess = false, Message = "Email already exists" };

                var newDelegates = new Delegates
                {
                    FullName = registerDelegates.FullName,
                    Email = registerDelegates.Email,
                    Address = registerDelegates.Address,
                    Biography = registerDelegates.Biography,
                    CreatedAt = DateTime.UtcNow,
                    DateOfBirth = registerDelegates.DateOfBirth,
                    Phone = registerDelegates.Phone,
                    PasswordHash = PasswordHasher.HashPassword(registerDelegates.Password),
                    Gender = registerDelegates.Gender,
                    Nationality = registerDelegates.Nationality,
                    Organization = registerDelegates.Organization,
                    IsConfirmed = true,
                    Position = registerDelegates.Position,
                    PassportNumber = registerDelegates.PassportNumber
                };

                await _delegateRepository.AddAsync(newDelegates);
                await _delegateRepository.SaveChangesAsync();
                return new GeneralResponseDTO { isSuccess = true, Message = "Register successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] RegisterDelegateAsync: {ex.Message}");
                return new GeneralResponseDTO { isSuccess = false, Message = "An error occurred during registration." };
            }
        }

        private string GenerateJwtToken(Delegates user)
        {
            try
            {
                var secretKey = _configuration["JwtSettings:SecretKey"];
                if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
                    throw new Exception("JWT Secret Key must be at least 32 characters long.");

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                bool isAdmin = user.Roles.Any(a => a.Id == RoleConstants.ADMIN_ROLE);
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", user.Id.ToString()),
                    new Claim("name", user.FullName),
                    new Claim("isAdmin", isAdmin.ToString()),
                };

                var token = new JwtSecurityToken(
                    _configuration["JwtSettings:Issuer"],
                    _configuration["JwtSettings:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] GenerateJwtToken: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
