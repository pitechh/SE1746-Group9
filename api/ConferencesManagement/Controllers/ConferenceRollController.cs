using Microsoft.AspNetCore.Mvc;
using ConferencesManagementService;
using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementAPI.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceRolesController : ControllerBase
    {
        private readonly ConferenceRoleService _conferenceRoleService;

        public ConferenceRolesController(ConferenceRoleService conferenceRoleService)
        {
            _conferenceRoleService = conferenceRoleService;
        }

        // API: Lấy danh sách vai trò hội thảo
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _conferenceRoleService.GetAllConferenceRolesAsync();
                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Get Successfully",
                    data = roles
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        // API: Lấy thông tin vai trò theo ID
        [HttpPost("get-by-id")]
        public async Task<IActionResult> GetRoleById([FromQuery] int id)
        {
            try
            {
                var role = await _conferenceRoleService.GetConferenceRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Role not found"
                    });
                }
                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        // API: Tạo vai trò mới
        [HttpPost("create")]
        [AuthorizeAdmin]
        public async Task<IActionResult> CreateRole([FromBody] AddConferenceRoleRequestDTO roleDTO)
        {
            try
            {
                await _conferenceRoleService.AddConferenceRoleAsync(roleDTO);
                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Role created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        // API: Cập nhật vai trò
        [HttpPost("update")]
        [AuthorizeAdmin]
        public async Task<IActionResult> UpdateRole([FromQuery] int id, [FromBody] UpdateConferenceRoleRequestDTO roleDTO)
        {
            try
            {
                bool updated = await _conferenceRoleService.UpdateConferenceRoleAsync(id, roleDTO);
                if (!updated)
                {
                    return NotFound(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Role not found"
                    });
                }

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Role updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        // API: Xóa vai trò
        [HttpPost("delete")]
        [AuthorizeAdmin]
        public async Task<IActionResult> DeleteRole([FromQuery] int id)
        {
            try
            {
                bool deleted = await _conferenceRoleService.DeleteConferenceRoleAsync(id);
                if (!deleted)
                {
                    return NotFound(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Role not found"
                    });
                }

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Role deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}
