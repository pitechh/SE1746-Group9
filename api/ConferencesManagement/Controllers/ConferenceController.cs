using Microsoft.AspNetCore.Mvc;
using ConferencesManagementService;
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementAPI.Utils;
using AutoMapper;
using ConferencesManagementAPI.Data.DTO;
using ConferencesManagement.Utils;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferencesController : ControllerBase
    {
        private readonly ConferenceService _conferenceService;
        private readonly IMapper _mapper;

        public ConferencesController(ConferenceService conferenceService, IMapper mapper)
        {
            _conferenceService = conferenceService;
            _mapper = mapper;
        }

        // Lấy danh sách hội thảo 
        [HttpGet("get-all")]
        [AuthorizeUser]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var conferences = await _conferenceService.GetAllConferencesAsync();
                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Fetched successfully",
                    data = conferences
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralResponseDTO { isSuccess = false, Message = $"Error: {ex.Message}" });
            }
        }

        // Lấy hội thảo theo ID
        [HttpGet("get-by-id")]
        [AuthorizeUser]
        public async Task<IActionResult> GetById([FromQuery] int id)
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
                var conference = await _conferenceService.GetConferenceByIdAsync(id);
                if (conference == null)
                    return NotFound(new GeneralResponseDTO { isSuccess = false, Message = "Conference not found" });

                if (conference.HostById == userId)
                {
                    conference.HostByMe = true;
                }

                return Ok(new GeneralResponseDTO { isSuccess = true, Message = "Fetched successfully", data = conference });
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralResponseDTO { isSuccess = false, Message = $"Error: {ex.Message}" });
            }
        }

        // Lấy hội thảo theo ID
        [HttpGet("get-by-name")]
        [AuthorizeUser]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            try
            {
                var conference = await _conferenceService.GetConferenceByName(name);
                if (conference == null)
                    return NotFound(new GeneralResponseDTO { isSuccess = false, Message = "Conference not found" });

                return Ok(new GeneralResponseDTO { isSuccess = true, Message = "Fetched successfully", data = conference });
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralResponseDTO { isSuccess = false, Message = $"Error: {ex.Message}" });
            }
        }

        // Tạo mới hội thảo
        [HttpPost("create")]
        [AuthorizeAdmin]
        public async Task<IActionResult> Create([FromBody] AddConferenceRequestDTO addConferenceRequestDTO)
        {
            try
            {
                if (addConferenceRequestDTO.HostBy == null)
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

                    addConferenceRequestDTO.HostBy = userId;
                }

                await _conferenceService.AddConferenceAsync(addConferenceRequestDTO);

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Created successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralResponseDTO { isSuccess = false, Message = $"Error: {ex.Message}" });
            }
        }

        // Cập nhật hội thảo 
        [HttpPost("update")]
        [AuthorizeAdmin]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] UpdateConferenceRequestDTO updateConferenceRequestDTO)
        {
            try
            {
                bool updated = await _conferenceService.UpdateConferenceAsync(id, updateConferenceRequestDTO);
                if (!updated)
                    return NotFound(new GeneralResponseDTO { isSuccess = false, Message = "Conference not found" });

                return Ok(new GeneralResponseDTO { isSuccess = true, Message = "Updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralResponseDTO { isSuccess = false, Message = $"Error: {ex.Message}" });
            }
        }

        // Xóa hội thảo 
        [HttpPost("delete")]
        [AuthorizeAdmin]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                bool deleted = await _conferenceService.DeleteConferenceAsync(id);
                if (!deleted)
                    return NotFound(new GeneralResponseDTO { isSuccess = false, Message = "Conference not found" });

                return Ok(new GeneralResponseDTO { isSuccess = true, Message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralResponseDTO { isSuccess = false, Message = $"Error: {ex.Message}" });
            }
        }
    }
}
