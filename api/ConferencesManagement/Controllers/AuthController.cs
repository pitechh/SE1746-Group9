using AutoMapper;
using ConferencesManagement.Utils;
using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementAPI.Utils;
using ConferencesManagementService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static ConferencesManagementAPI.Data.DTO.DelegatesDTO;

namespace ConferencesManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly DelegateService _delegateService;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;


        public AuthController(IAuthService authService, ILogger<AuthController> logger, DelegateService delegateService, IMapper mapper)
        {
            _authService = authService;
            _logger = logger;
            _delegateService = delegateService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequestDTO request)
        {
            try
            {
                var response = await _authService.AuthenticateAsync(request);
                if (response == null)
                    return Unauthorized(new { message = "Invalid email or password" });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Login: {ex.Message}");
                return StatusCode(500, new { message = "Internal Server Error" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDelegatesDTO delegates)
        {
            try
            {
                var responseDTO = await _authService.RegisterDelegateAsync(delegates);

                if (!responseDTO.isSuccess)
                    return BadRequest(responseDTO);

                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Register: {ex.Message}");
                return StatusCode(500, new { message = "Internal Server Error" });
            }
        }

        [HttpGet("profile")]
        [AuthorizeUser]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (userId == null)
                {
                    return Unauthorized(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Invalid token"
                    });
                }

                var delegateData = await _delegateService.GetDelegateByIdAsync(userId.Value);
                var delegateResponseData = _mapper.Map<GetDelegatesResponseDTO>(delegateData);
                if (delegateData == null)
                {
                    return NotFound(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "User not found"
                    });
                }

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    data = delegateResponseData
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        

        [HttpPost("change-password")]
        [AuthorizeUser]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (userId == null)
                {
                    return Unauthorized(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Invalid token"
                    });
                }

                var delegateData = await _delegateService.GetDelegateByIdAsync(userId.Value);
                if (delegateData == null)
                {
                    return NotFound(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "User not found"
                    });
                }

                // Kiểm tra mật khẩu hiện tại
                if (!PasswordHasher.VerifyPassword(changePasswordDTO.CurrentPassword, delegateData.PasswordHash))
                {
                    return BadRequest(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Current password is incorrect"
                    });
                }

                if (changePasswordDTO.NewPassword != changePasswordDTO.ConfirmPassword)
                {
                    return BadRequest(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "New password and confirm password is not match"
                    });
                }


                var updateDelegate = _mapper.Map<UpdateDelegatesRequestDTO>(changePasswordDTO);
                updateDelegate.Id = delegateData.Id;
                await _delegateService.UpdateDelegateAsync(updateDelegate);

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    data = delegateData
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        [HttpPost("update-profile")]
        [AuthorizeUser]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateDelegatesRequestDTO updateRequest)
        {
            try
            {
                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (userId == null)
                {
                    return Unauthorized(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Invalid token"
                    });
                }

                var existingDelegate = await _delegateService.GetDelegateByIdAsync(userId.Value);
                if (existingDelegate == null)
                {
                    return NotFound(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "User not found"
                    });
                }

                await _delegateService.UpdateDelegateAsync(updateRequest);

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Profile updated successfully",
                    data = existingDelegate
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateProfile: {ex.Message}");
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = "Internal Server Error"
                });
            }
        }


    }
}
