using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using ConferencesManagementAPI.Data.DTO;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ConferencesManagementAPI.DAO.Repositories;

namespace ConferencesManagementService
{
    public class ConferenceRoleService
    {
        private readonly ConferenceRoleRepositories _conferenceRoleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ConferenceRoleService> _logger;

        public ConferenceRoleService(ConferenceRoleRepositories conferenceRoleRepository, IMapper mapper, ILogger<ConferenceRoleService> logger)
        {
            _conferenceRoleRepository = conferenceRoleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // Lấy danh sách vai trò hội thảo
        public async Task<List<ConferenceRoleResponseDTO>> GetAllConferenceRolesAsync()
        {
            try
            {
                var roles = await _conferenceRoleRepository.GetAllAsync();
                return _mapper.Map<List<ConferenceRoleResponseDTO>>(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllConferenceRolesAsync: {ex.Message}");
                return new List<ConferenceRoleResponseDTO>();
            }
        }

        // Lấy vai trò hội thảo theo ID
        public async Task<ConferenceRoleResponseDTO?> GetConferenceRoleByIdAsync(int id)
        {
            try
            {
                var role = await _conferenceRoleRepository.GetByIdAsync(id);
                return role != null ? _mapper.Map<ConferenceRoleResponseDTO>(role) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetConferenceRoleByIdAsync: {ex.Message}");
                return null;
            }
        }

        // Tạo mới vai trò hội thảo
        public async Task<bool> AddConferenceRoleAsync(AddConferenceRoleRequestDTO addConferenceRoleRequestDTO)
        {
            try
            {
                var role = _mapper.Map<ConferenceRole>(addConferenceRoleRequestDTO);
                await _conferenceRoleRepository.AddAsync(role);
                await _conferenceRoleRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddConferenceRoleAsync: {ex.Message}");
                return false;
            }
        }

        // Cập nhật vai trò hội thảo
        public async Task<bool> UpdateConferenceRoleAsync(int id, UpdateConferenceRoleRequestDTO updateConferenceRoleRequestDTO)
        {
            try
            {
                var existingRole = await _conferenceRoleRepository.GetByIdAsync(id);
                if (existingRole == null) return false;

                _mapper.Map(updateConferenceRoleRequestDTO, existingRole);
                await _conferenceRoleRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateConferenceRoleAsync: {ex.Message}");
                return false;
            }
        }

        // Xóa vai trò hội thảo
        public async Task<bool> DeleteConferenceRoleAsync(int id)
        {
            try
            {
                var role = await _conferenceRoleRepository.GetByIdAsync(id);
                if (role == null) return false;

                _conferenceRoleRepository.Remove(role);
                await _conferenceRoleRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteConferenceRoleAsync: {ex.Message}");
                return false;
            }
        }
    }
}
