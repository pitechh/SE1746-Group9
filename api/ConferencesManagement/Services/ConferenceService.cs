using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using ConferencesManagementAPI.Data.DTO;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ConferencesManagementAPI.DAO.Repositories;

namespace ConferencesManagementService
{
    public class ConferenceService
    {
        private readonly ConferenceRepositories _conferenceRepository;
        private readonly DelegatesRepositories _delegateRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<ConferenceService> _logger;

        public ConferenceService(ConferenceRepositories conferenceRepository, IMapper mapper, ILogger<ConferenceService> logger, DelegatesRepositories delegateRepository)
        {
            _conferenceRepository = conferenceRepository;
            _mapper = mapper;
            _logger = logger;
            _delegateRepository = delegateRepository;
        }

        // Lấy danh sách hội thảo
        public async Task<List<ConferenceResponseDTO>> GetAllConferencesAsync()
        {
            try
            {
                var conferences = await _conferenceRepository.GetAllAsync();
                return _mapper.Map<List<ConferenceResponseDTO>>(conferences);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllConferencesAsync: {ex.Message}");
                return new List<ConferenceResponseDTO>();
            }
        }

        // Lấy hội thảo theo ID
        public async Task<ConferenceResponseDTO?> GetConferenceByIdAsync(int id)
        {
            try
            {
                var conference = await _conferenceRepository.GetConferenceByIdAsync(id);
                return conference != null ? _mapper.Map<ConferenceResponseDTO>(conference) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetConferenceByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<List<ConferenceResponseDTO>?> GetConferenceByName(string name)
        {
            try
            {
                var conference = await _conferenceRepository.FindAsync(a => a.Name.Contains(name));
                return conference != null ? _mapper.Map<List<ConferenceResponseDTO>>(conference) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetConferenceByIdAsync: {ex.Message}");
                return null;
            }
        }

        // Tạo mới hội thảo
        public async Task<bool> AddConferenceAsync(AddConferenceRequestDTO addConferenceRequestDTO)
        {
            try
            {
                var conference = _mapper.Map<Conference>(addConferenceRequestDTO);
                conference.CreatedAt = DateTime.UtcNow;
                await _conferenceRepository.AddAsync(conference);
                await _conferenceRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddConferenceAsync: {ex.Message}");
                return false;
            }
        }

        // Cập nhật hội thảo
        public async Task<bool> UpdateConferenceAsync(int id, UpdateConferenceRequestDTO updateConferenceRequestDTO)
        {
            try
            {
                var existingConference = await _conferenceRepository.GetByIdAsync(id);
                if (existingConference == null) return false;

                _mapper.Map(updateConferenceRequestDTO, existingConference);
                await _conferenceRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateConferenceAsync: {ex.Message}");
                return false;
            }
        }

        // Xóa hội thảo
        public async Task<bool> DeleteConferenceAsync(int id)
        {
            try
            {
                var conference = await _conferenceRepository.GetByIdAsync(id);
                if (conference == null) return false;

                _conferenceRepository.Remove(conference);
                await _conferenceRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteConferenceAsync: {ex.Message}");
                return false;
            }
        }
    }
}
