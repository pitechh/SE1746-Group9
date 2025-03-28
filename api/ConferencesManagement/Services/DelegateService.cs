
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementAPI.DAO.Repositories;
using ConferencesManagement.Utils;
using ConferencesManagementAPI.Data.DTO;
using AutoMapper;
using static ConferencesManagementAPI.Data.DTO.DelegatesDTO;

namespace ConferencesManagementService
{
    public class DelegateService
    {
        private readonly DelegatesRepositories _delegateRepository;
        private readonly ILogger<DelegateService> _logger;
        private readonly IMapper _mapper;

        public DelegateService(DelegatesRepositories delegatesRepositories, ILogger<DelegateService> logger, IMapper mapper)
        {
            _delegateRepository = delegatesRepositories;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Delegates>> GetAllDelegatesAsync()
        {
            try
            {
                var result = await _delegateRepository.GetAllAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllDelegatesAsync: {ex.Message}");
                return Enumerable.Empty<Delegates>();
            }
        }

        public async Task<Delegates?> GetDelegateByIdAsync(int id)
        {
            try
            {

                return await _delegateRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetDelegateByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<Delegates?> GetDelegateByEmailAsync(string email)
        {
            try
            {
                return await _delegateRepository.FirstOrDefaultAsync(a => a.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetDelegateByEmailAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Delegates>?> GetDelegateByEmailStartWithAsync(string email)
        {
            try
            {
                return await _delegateRepository.FindAsync(a => a.Email.StartsWith(email));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetDelegateByEmailAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<GeneralResponseDTO> AddDelegateAsync(AddDelegatesRequestDTO addDelegatesRequestDTO)
        {
            try
            {            //var newDelegate = _mapper.Map<Delegates>(addDelegatesRequestDTO);
                var newDelegates = new Delegates
                {
                    FullName = addDelegatesRequestDTO.FullName,
                    Email = addDelegatesRequestDTO.Email,
                    Address = addDelegatesRequestDTO.Address,
                    Biography = addDelegatesRequestDTO.Biography,
                    CreatedAt = DateTime.UtcNow,
                    DateOfBirth = addDelegatesRequestDTO.DateOfBirth,
                    Phone = addDelegatesRequestDTO.Phone,
                    PasswordHash = PasswordHasher.HashPassword(addDelegatesRequestDTO.Password),
                    Gender = !string.IsNullOrEmpty(addDelegatesRequestDTO.Gender) ? addDelegatesRequestDTO.Gender : "Male",
                    Nationality = addDelegatesRequestDTO.Nationality,
                    Organization = addDelegatesRequestDTO.Organization,
                    IsConfirmed = true,
                    Position = addDelegatesRequestDTO.Position,
                    PassportNumber = addDelegatesRequestDTO.PassportNumber
                };
                await _delegateRepository.AddAsync(newDelegates);
                await _delegateRepository.SaveChangesAsync();
                return new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Add new delegates successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddDelegateAsync: {ex.Message}");
                return new GeneralResponseDTO
                {
                    isSuccess = false,
                    Message = "Add new delegates failed. Error in backend"
                };
            }
        }

        public async Task<bool> UpdateDelegateAsync(UpdateDelegatesRequestDTO updatedDelegate)
        {
            try
            {
                var existingDelegate = await _delegateRepository.GetByIdAsync(updatedDelegate.Id);
                if (existingDelegate == null) return false;
                else
                {
                    existingDelegate.FullName = updatedDelegate.FullName ?? existingDelegate.FullName;
                    existingDelegate.Address = updatedDelegate.Address ?? existingDelegate.Address;
                    existingDelegate.Biography = updatedDelegate.Biography ?? existingDelegate.Biography;
                    existingDelegate.DateOfBirth = updatedDelegate.DateOfBirth ?? existingDelegate.DateOfBirth;
                    existingDelegate.Phone = updatedDelegate.Phone ?? existingDelegate.Phone;
                    existingDelegate.Nationality = updatedDelegate.Nationality ?? existingDelegate.Nationality;
                    existingDelegate.Organization = updatedDelegate.Organization ?? existingDelegate.Organization;
                    existingDelegate.Position = updatedDelegate.Position ?? existingDelegate.Position;
                    existingDelegate.PassportNumber = updatedDelegate.PassportNumber ?? existingDelegate.PassportNumber;
                    existingDelegate.PasswordHash = string.IsNullOrEmpty(updatedDelegate.Password)
                                                        ? existingDelegate.PasswordHash : PasswordHasher.HashPassword(updatedDelegate.Password);
                }


                //_delegateRepository.Update(existingDelegate);
                await _delegateRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateDelegateAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteDelegateAsync(int id)
        {
            try
            {
                var delegateToDelete = await _delegateRepository.GetByIdAsync(id);
                if (delegateToDelete == null) return false;

                _delegateRepository.Remove(delegateToDelete);
                await _delegateRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteDelegateAsync: {ex.Message}");
                return false;
            }
        }
    }
}
