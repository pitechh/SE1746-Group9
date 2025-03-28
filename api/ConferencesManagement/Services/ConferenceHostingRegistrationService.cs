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
using static ConferencesManagementAPI.Constants.RegistrationStatusConstants;

namespace ConferencesManagementService
{
    public class ConferenceHostingRegistrationService
    {
        private readonly ConferenceHostingRegistrationRepositories _registrationRepository;
        private readonly ConferenceRepositories _conferenceRepository;
        private readonly DelegatesRepositories _delegateRepositories;

        private readonly IMapper _mapper;

        public ConferenceHostingRegistrationService(
            ConferenceHostingRegistrationRepositories registrationRepository,
            IMapper mapper,
            DelegatesRepositories delegateRepositories,
            ConferenceRepositories conferenceRepository)
        {
            _registrationRepository = registrationRepository;
            _mapper = mapper;
            _delegateRepositories = delegateRepositories;
            _conferenceRepository = conferenceRepository;
        }

        //  Xem danh sách tất cả đơn đăng ký
        public async Task<GeneralResponseDTO> GetAllRegistrationsAsync()
        {
            try
            {
                var registrations = await _registrationRepository.GetAllRegistrationAsync();
                var result = _mapper.Map<List<ConferenceHostingRegistrationDTO>>(registrations);
                return new GeneralResponseDTO { isSuccess = true, data = result, Message = "Lấy danh sách thành công" };
            }
            catch (Exception ex)
            {
                return new GeneralResponseDTO { isSuccess = false, Message = $"Lỗi: {ex.Message}" };
            }
        }

        //  Xem chi tiết đơn đăng ký theo ID
        public async Task<GeneralResponseDTO> GetRegistrationByIdAsync(int id)
        {
            try
            {
                var registration = await _registrationRepository.GetRegistrationByIdAsync(id);
                if (registration == null)
                    return new GeneralResponseDTO { isSuccess = false, Message = "Không tìm thấy đơn đăng ký" };

                var result = _mapper.Map<ConferenceHostingRegistrationDTO>(registration);
                return new GeneralResponseDTO { isSuccess = true, data = result, Message = "Lấy thông tin thành công" };
            }
            catch (Exception ex)
            {
                return new GeneralResponseDTO { isSuccess = false, Message = $"Lỗi: {ex.Message}" };
            }
        }

        //  Xem danh sách đơn đăng ký theo ID người đăng ký
        public async Task<GeneralResponseDTO> GetRegistrationsByDelegateIdAsync(int delegateId)
        {
            try
            {
                var registrations = await _registrationRepository.GetByDelegateIdAsync(delegateId);
                var result = _mapper.Map<List<ConferenceHostingRegistrationDTO>>(registrations);
                return new GeneralResponseDTO { isSuccess = true, data = result, Message = "Lấy danh sách thành công" };
            }
            catch (Exception ex)
            {
                return new GeneralResponseDTO { isSuccess = false, Message = $"Lỗi: {ex.Message}" };
            }
        }

        //  Xem danh sách đơn đăng ký theo ID người đăng ký
        public async Task<GeneralResponseDTO> GetByDelegateIdAndRegistrationIdAsync(int delegateId, int registrationId)
        {
            try
            {
                var registrations = await _registrationRepository.FindAsync(a => a.RegisterId == delegateId && a.Id == registrationId);
                if (registrations.Count() <= 0)
                {
                    return new GeneralResponseDTO { isSuccess = false, Message = "Người dùng không đăng kí hội thảo này hoặc đơn đăng kí không tồn tại" };
                }
                var result = _mapper.Map<List<ConferenceHostingRegistrationDTO>>(registrations);
                return new GeneralResponseDTO { isSuccess = true, data = result, Message = "Lấy danh sách thành công" };
            }
            catch (Exception ex)
            {
                return new GeneralResponseDTO { isSuccess = false, Message = $"Lỗi: {ex.Message}" };
            }
        }

        //  Tạo đơn đăng ký mới
        public async Task<GeneralResponseDTO> CreateRegistrationAsync(CreateConferenceHostingRegistrationDTO request)
        {
            try
            {
                var delegateFindResult = await _delegateRepositories.FirstOrDefaultAsync(a => a.Id == request.RegisterId);
                if (delegateFindResult == null)
                {
                    return new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Không tìm thấy đại biểu có Id này"
                    };
                }
                var newRegistration = _mapper.Map<ConferenceHostingRegistration>(request);
                var isSuccess = await _registrationRepository.CreateAsync(newRegistration);
                return new GeneralResponseDTO
                {
                    isSuccess = isSuccess,
                    Message = isSuccess ? "Tạo đơn đăng ký thành công" : "Không thể tạo đơn đăng ký"
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponseDTO { isSuccess = false, Message = $"Lỗi: {ex.Message}" };
            }
        }

        //  Duyệt đơn đăng ký
        public async Task<GeneralResponseDTO> ApproveRegistrationAsync(int id)
        {
            try
            {
                var registration = await _registrationRepository.GetByIdAsync(id);
                if (registration == null)
                    return new GeneralResponseDTO { isSuccess = false, Message = "Không tìm thấy đơn đăng ký" };
                var conference_add = registration;
                registration.Status = ConfHostingRegistrationStatusConstants.STATUS_APPROVED;
               await _registrationRepository.SaveChangesAsync();

                return new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Đã duyệt đơn đăng ký"
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponseDTO { isSuccess = false, Message = $"Lỗi: {ex.Message}" };
            }
        }

        //  Từ chối đơn đăng ký
        public async Task<GeneralResponseDTO> DenyRegistrationAsync(int id)
        {
            try
            {
                var registration = await _registrationRepository.GetByIdAsync(id);
                if (registration == null)
                    return new GeneralResponseDTO { isSuccess = false, Message = "Không tìm thấy đơn đăng ký" };

                registration.Status = ConfHostingRegistrationStatusConstants.STATUS_DENIED;
                await _registrationRepository.SaveChangesAsync();
                return new GeneralResponseDTO
                {
                    isSuccess = true,
                    Message = "Đã từ chối đơn đăng ký"
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponseDTO { isSuccess = false, Message = $"Lỗi: {ex.Message}" };
            }
        }

        //  Xóa đơn đăng ký
        public async Task<GeneralResponseDTO> DeleteRegistrationAsync(int id)
        {
            try
            {
                var registration = _registrationRepository.GetByIdAsync(id);
                if (registration == null)
                {
                    return new GeneralResponseDTO
                    {
                        isSuccess = false,
                        Message = "Đơn đăng kí tổ chức hội thảo không tồn tại"
                    };
                }
                var isSuccess = await _registrationRepository.DeleteAsync(id);
                return new GeneralResponseDTO
                {
                    isSuccess = isSuccess,
                    Message = isSuccess ? "Xóa đơn đăng ký thành công" : "Không thể xóa đơn đăng ký"
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponseDTO { isSuccess = false, Message = $"Lỗi: {ex.Message}" };
            }
        }
    }
}
