using Microsoft.AspNetCore.Mvc;
using ConferencesManagementService;
using ConferencesManagementAPI.Data.DTO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ConferencesManagementAPI.Utils;

namespace ConferencesManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceHostingRegistrationController : ControllerBase
    {
        private readonly ConferenceHostingRegistrationService _hostingRegistrationService;
        private readonly ConferenceService _conferenceService;
        private readonly DelegateConferenceRoleService _delegateConferenceRoleService;

        public ConferenceHostingRegistrationController(ConferenceHostingRegistrationService hostingRegistrationService, 
                                                       ConferenceService conferenceService,
                                                       DelegateConferenceRoleService delegateConferenceRoleService)
        {
            _hostingRegistrationService = hostingRegistrationService;
            _conferenceService = conferenceService;
            _delegateConferenceRoleService = delegateConferenceRoleService;
        }

        // Lấy danh sách tất cả đơn đăng ký tổ chức hội thảo
        [HttpGet("get-all")]
        [AuthorizeAdmin]
        public async Task<IActionResult> GetAllRegistrations()
        {
            try
            {
                var data = await _hostingRegistrationService.GetAllRegistrationsAsync();
                if (!data.isSuccess) return BadRequest(data);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                });
            }
        }

        // Lấy đơn đăng ký theo ID
        [HttpGet("get-by-id")]
        [AuthorizeAdmin]
        public async Task<IActionResult> GetRegistrationById([FromQuery] int id)
        {
            try
            {
                var result = await _hostingRegistrationService.GetRegistrationByIdAsync(id);
                if (result.data == null)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                });
            }
        }

        // Lấy đơn đăng ký theo ID
        [HttpGet("get-by-delegateId")]
        [AuthorizeAdmin]
        public async Task<IActionResult> GetRegistrationByDelegateId([FromQuery] int id)
        {
            try
            {
                var result = await _hostingRegistrationService.GetRegistrationsByDelegateIdAsync(id);
                if (result.data == null)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                });
            }
        }

        [HttpGet("get-my-registrations")]
        [AuthorizeUser]
        public async Task<IActionResult> GetMyRegistrations()
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

                var data = await _hostingRegistrationService.GetRegistrationsByDelegateIdAsync(userId.Value);
                if (!data.isSuccess)
                {
                    return BadRequest(data);
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                });
            }
        }


        // Lấy danh sách đơn đăng ký theo ID người đăng ký
        [HttpGet("get-by-delegate")]
        [AuthorizeAdmin]
        public async Task<IActionResult> GetRegistrationsByDelegateId([FromQuery] int delegateId)
        {
            try
            {
                var data = await _hostingRegistrationService.GetRegistrationsByDelegateIdAsync(delegateId);
                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Lấy danh sách đơn đăng ký theo người đăng ký thành công",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                });
            }
        }

        [HttpPost("create")]
        [AuthorizeUser]
        public async Task<IActionResult> CreateRegistration([FromBody] CreateConferenceHostingRegistrationDTO request)
        {
            try
            {
                if (request.EndDate <= request.StartDate)
                {
                    return BadRequest(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc"
                    });
                }

                var userId = JwtHelper.GetUserIdFromToken(HttpContext);
                if (userId == null)
                {
                    return Unauthorized(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Invalid token"
                    });
                }
                request.RegisterId = userId.Value;

                var result = await _hostingRegistrationService.CreateRegistrationAsync(request);
                if (!result.isSuccess)
                {
                    return BadRequest(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Không thể tạo đơn đăng ký"
                    });
                }

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Đơn đăng ký đã được tạo thành công"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                });
            }
        }

        //  Duyệt đơn đăng ký
        [HttpPost("approve")]
        [AuthorizeAdmin]
        public async Task<IActionResult> ApproveRegistration([FromQuery] int id)
        {
            try
            {
                var result = await _hostingRegistrationService.ApproveRegistrationAsync(id);
                //_registrationService
                if (!result.isSuccess)
                {
                    return BadRequest(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Không thể duyệt đơn đăng ký"
                    });
                }

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Đơn đăng ký đã được duyệt thành công"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                });
            }
        }

        //  Từ chối đơn đăng ký
        [HttpPost("deny")]
        [AuthorizeAdmin]
        public async Task<IActionResult> DenyRegistration([FromQuery] int id)
        {
            try
            {
                var result = await _hostingRegistrationService.DenyRegistrationAsync(id);
                if (!result.isSuccess)
                {
                    return BadRequest(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Không thể từ chối đơn đăng ký"
                    });
                }

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Đơn đăng ký đã bị từ chối"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                });
            }
        }

        //  Xóa đơn đăng ký
        [HttpPost("delete")]
        [AuthorizeAdmin]
        public async Task<IActionResult> DeleteRegistration([FromQuery] int id)
        {
            try
            {
                var result = await _hostingRegistrationService.DeleteRegistrationAsync(id);
                if (!result.isSuccess)
                {
                    return BadRequest(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Không thể xóa đơn đăng ký"
                    });
                }

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Đơn đăng ký đã được xóa thành công"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                });
            }
        }

        //  Xóa đơn đăng ký
        [HttpPost("delegate-delete")]
        [AuthorizeUser]
        public async Task<IActionResult> DelegateDeleteRegistration([FromQuery] int id)
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

                var getResult = await _hostingRegistrationService.GetByDelegateIdAndRegistrationIdAsync(userId.Value, id);
                if (!getResult.isSuccess) return BadRequest(getResult);

                var result = await _hostingRegistrationService.DeleteRegistrationAsync(id);
                if (!result.isSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"Lỗi hệ thống: {ex.Message}"
                });
            }
        }
    }
}
