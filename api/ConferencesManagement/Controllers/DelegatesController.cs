using Microsoft.AspNetCore.Mvc;
using ConferencesManagementService;
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementAPI.Utils;
using ConferencesManagementAPI.Data.DTO;
using AutoMapper;
using ConferencesManagement.Utils;
using Microsoft.AspNetCore.Authorization;
using static ConferencesManagementAPI.Data.DTO.DelegatesDTO;

namespace ConferencesManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DelegatesController : ControllerBase
    {
        private readonly DelegateService _delegateService;
        private readonly IMapper _mapper;

        public DelegatesController(DelegateService delegateService, IMapper mapper)
        {
            _delegateService = delegateService;
            _mapper = mapper;
        }

        [HttpGet("get-all")]
        [AuthorizeUser]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var delegates = await _delegateService.GetAllDelegatesAsync();
                delegates.ToList().ForEach(d => d.PassportNumber = "");
                var responseData = _mapper.Map<List<GetDelegatesResponseDTO>>(delegates);
                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Fetched successfully",
                    data = responseData
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

        [HttpPost("get-by-id")]
        [AuthorizeUser]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                var delegateData = await _delegateService.GetDelegateByIdAsync(id);
                if (delegateData == null)
                    return NotFound(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Delegate not found"
                    });
                delegateData.PasswordHash = "";
                delegateData.PassportNumber = "";
                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Fetched successfully",
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

        [HttpPost("create")]
        [AuthorizeAdmin]
        public async Task<IActionResult> Create([FromBody] AddDelegatesRequestDTO addDelegatesRequestDTO)
        {
            try
            {
                var delegates = await _delegateService.GetDelegateByEmailAsync(addDelegatesRequestDTO.Email);
                if (delegates != null)
                {
                    return BadRequest(new GeneralResponseDTO { isSuccess = false, Message = "Delegates with email already existed" });
                }
                var result = await _delegateService.AddDelegateAsync(addDelegatesRequestDTO);
                if (!result.isSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
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


        [HttpGet("get-by-email")]
        [AuthorizeUser]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            try
            {
                var delegates = string.IsNullOrEmpty(email) ? await _delegateService.GetAllDelegatesAsync()
                                                            : await _delegateService.GetDelegateByEmailStartWithAsync(email);
                if (delegates == null)
                {
                    return BadRequest(new GeneralResponseDTO { isSuccess = false, Message = "Delegates with email not existed" });
                }

                return Ok(new GeneralResponseDTO { isSuccess = true, Message = "Get successfully", data = delegates });
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


        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateDelegatesRequestDTO updatedDelegate)
        {
            try
            {
                if (updatedDelegate == null)
                    return BadRequest(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Invalid data"
                    });

                bool updated = await _delegateService.UpdateDelegateAsync(updatedDelegate);
                if (!updated)
                    return NotFound(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Delegate not found"
                    });

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Updated successfully"
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

        [HttpPost("delete")]
        [AuthorizeAdmin]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                bool deleted = await _delegateService.DeleteDelegateAsync(id);
                if (!deleted)
                    return NotFound(new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Delegate not found"
                    });

                return Ok(new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Deleted successfully"
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
    }
}
