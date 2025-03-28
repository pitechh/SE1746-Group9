using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using ConferencesManagementAPI.Data.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferencesManagementAPI.DAO.Repositories;
using ConferencesManagementAPI.Constants;
using ConferencesManagement.Constants;

namespace ConferencesManagementService
{
    public class RegistrationService
    {
        private readonly RegistrationRepositories _registrationRepository;
        private readonly DelegateConferenceRoleRepositories _delegateConferenceRoleRepository;
        private readonly DelegatesRepositories _delegateRepository;
        private readonly ConferenceRepositories _conferenceRepository;
        private readonly ConferenceRoleRepositories _conferenceRoleRepositories;

        private readonly IMapper _mapper;

        public RegistrationService(RegistrationRepositories registrationRepository, IMapper mapper,
                                   DelegateConferenceRoleRepositories delegateConferenceRepository,
                                   ConferenceRepositories conferenceRepository,
                                   DelegatesRepositories delegateRepository,
                                   ConferenceRoleRepositories conferenceRoleRepositories)
        {
            _registrationRepository = registrationRepository;
            _mapper = mapper;
            _delegateConferenceRoleRepository = delegateConferenceRepository;
            _conferenceRepository = conferenceRepository;
            _delegateRepository = delegateRepository;
            _conferenceRoleRepositories = conferenceRoleRepositories;
        }

        // Lấy danh sách đăng ký
        public async Task<List<RegistrationResponseDTO>> GetAllRegistrationsAsync()
        {
            try
            {
                var registrations = await _registrationRepository.GetAllAsync();
                return _mapper.Map<List<RegistrationResponseDTO>>(registrations);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching registrations", ex);
            }
        }

        // Lấy đăng ký theo ID
        public async Task<List<RegistrationResponseDTO>?> GetRegistrationByConfenreceIdAsync(int id)
        {
            try
            {
                var registration = _registrationRepository.GetByConferenceId(id);
                return registration;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching registration with ID {id}", ex);
            }
        }

        // Lấy đăng ký theo ID
        public async Task<Registration?> GetRegistrationByIdAsync(int id)
        {
            try
            {
                var registration = await _registrationRepository.GetByIdAsync(id);
                return registration;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching registration with ID {id}", ex);
            }
        }

        public async Task<GeneralResponseDTO> DelegateRegisterConferenceAsync(AddRegistrationRequestDTO registrationDTO)
        {
            try
            {
                // Kiểm tra đại biểu có tồn tại không
                var existingDelegate = await _delegateRepository.GetByIdAsync(registrationDTO.DelegateId);
                if (existingDelegate == null)
                {
                    return new GeneralResponseDTO { isSuccess = false, Message = "Delegate not found." };
                }

                // Kiểm tra hội thảo có tồn tại không
                var existingConference = await _conferenceRepository.GetByIdAsync(registrationDTO.ConferenceId);
                if (existingConference == null)
                {
                    return new GeneralResponseDTO { isSuccess = false, Message = "Conference not found." };
                }

                // Kiểm tra vai trò có tồn tại không
                var roleExists = await _conferenceRoleRepositories.ExistsAsync(registrationDTO.ConferenceRoleId);
                if (!roleExists)
                {
                    return new GeneralResponseDTO { isSuccess = false, Message = "Conference role not found." };
                }

                // Kiểm tra xem đại biểu đã đăng ký chưa
                var existingRegistration = await _registrationRepository.GetByConferenceIdAndDelegateIdAsync(
                    registrationDTO.DelegateId, registrationDTO.ConferenceId);
                if (existingRegistration != null)
                {
                    return new GeneralResponseDTO { isSuccess = false, Message = "Delegate is already registered for this conference." };
                }

                if(registrationDTO.DelegateId == existingConference.HostBy)
                {
                    return new GeneralResponseDTO { isSuccess = false, Message = "Delegate is host of this conference." };
                }

                // Thêm vào bảng Registrations
                var registration = new Registration
                {
                    DelegateId = registrationDTO.DelegateId,
                    ConferenceId = registrationDTO.ConferenceId,
                    Status = RegistrationStatusConstants.STATUS_Pending, // Sử dụng constant
                    RegisteredAt = DateTime.UtcNow
                };
                await _registrationRepository.AddAsync(registration);
                await _registrationRepository.SaveChangesAsync();

                // Thêm vai trò mặc định vào bảng DelegateConferenceRoles
                var defaultRole = new DelegateConferenceRole
                {
                    DelegateId = registrationDTO.DelegateId,
                    ConferenceId = registrationDTO.ConferenceId,
                    RoleId = registrationDTO.ConferenceRoleId
                };
                await _delegateConferenceRoleRepository.AddAsync(defaultRole);
                await _delegateConferenceRoleRepository.SaveChangesAsync();

                return new GeneralResponseDTO { isSuccess = true, Message = "Delegate registered successfully." };
            }
            catch (Exception ex)
            {
                return new GeneralResponseDTO { isSuccess = false, Message = $"An error occurred: {ex.Message}" };
            }
        }


        // Tạo mới đăng ký
        public async Task AddRegistrationAsync(AddRegistrationRequestDTO addRegistrationRequestDTO)
        {
            try
            {
                if (!RegistrationStatusConstants.ValidStatuses.Contains(addRegistrationRequestDTO.Status))
                {
                    throw new ArgumentException("Invalid registration status.");
                }

                var registration = _mapper.Map<Registration>(addRegistrationRequestDTO);
                registration.RegisteredAt = DateTime.UtcNow;

                await _registrationRepository.AddAsync(registration);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding new registration", ex);
            }
        }

        // Cập nhật đăng ký
        public async Task<GeneralResponseDTO> UpdateRegistrationAsync(int id, string status)
        {
            try
            {
                var existingRegistration = await _registrationRepository.GetByIdAsync(id);
                if (existingRegistration == null) return new GeneralResponseDTO { isSuccess = false, Message = "Registation not found" };

                if (!RegistrationStatusConstants.ValidStatuses.Contains(status))
                {
                    throw new ArgumentException("Invalid registration status.");
                }

                existingRegistration.Status = status;
                await _registrationRepository.SaveChangesAsync();
                return new GeneralResponseDTO { isSuccess = true, Message = "Update registration successfully" };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating registration with ID {id}", ex);
            }
        }

        // Xóa đăng ký
        public async Task<bool> DeleteRegistrationAsync(int id)
        {
            try
            {
                var registration = await _registrationRepository.GetByIdAsync(id);
                if (registration == null) return false;

                _registrationRepository.Remove(registration);
                await _registrationRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting registration with ID {id}", ex);
            }
        }
    }
}
