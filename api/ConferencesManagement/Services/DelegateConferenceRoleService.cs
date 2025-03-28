using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using ConferencesManagementAPI.Data.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConferencesManagementAPI.DAO.Repositories;

namespace ConferencesManagementService
{
    public class DelegateConferenceRoleService
    {
        private readonly DelegateConferenceRoleRepositories _roleRepository;
        private readonly ConferenceRoleRepositories _conferenceRoleRepository; // Thêm repository kiểm tra RoleId
        private readonly IMapper _mapper;

        public DelegateConferenceRoleService(DelegateConferenceRoleRepositories roleRepository, ConferenceRoleRepositories conferenceRoleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _conferenceRoleRepository = conferenceRoleRepository;
            _mapper = mapper;
        }

        // Lấy danh sách vai trò của đại biểu
        public async Task<List<DelegateConferenceRoleResponseDTO>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _roleRepository.GetAllAsync();
                return _mapper.Map<List<DelegateConferenceRoleResponseDTO>>(roles);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching delegate conference roles", ex);
            }
        }

        // Lấy vai trò của đại biểu theo ID
        public async Task<DelegateConferenceRoleResponseDTO?> GetRoleByIdAsync(int delegateId, int conferenceId)
        {
            try
            {
                var role = await _roleRepository.GetByConferenceIdAndDelegateIdAsync(delegateId, conferenceId);
                return role != null ? _mapper.Map<DelegateConferenceRoleResponseDTO>(role) : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching role for DelegateId {delegateId} and ConferenceId {conferenceId}", ex);
            }
        }

        // Thêm mới vai trò cho đại biểu
        public async Task AddRoleAsync(AddDelegateConferenceRoleDTO addRoleDTO)
        {
            try
            {
                // Kiểm tra RoleId có tồn tại trong bảng ConferenceRoles không
                var roleExists = await _conferenceRoleRepository.ExistsAsync(addRoleDTO.RoleId);
                if (!roleExists)
                {
                    throw new ArgumentException($"Role ID {addRoleDTO.RoleId} does not exist.");
                }

                var role = _mapper.Map<DelegateConferenceRole>(addRoleDTO);
                await _roleRepository.AddAsync(role);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding delegate conference role", ex);
            }
        }

        // Cập nhật vai trò của đại biểu
        public async Task<bool> UpdateRoleAsync(int delegateId, int conferenceId, UpdateDelegateConferenceRoleDTO updateRoleDTO)
        {
            try
            {
                var existingRole = await _roleRepository.GetByConferenceIdAndDelegateIdAsync(delegateId, conferenceId);
                if (existingRole == null) return false;

                // Kiểm tra RoleId có tồn tại trong bảng ConferenceRoles không
                var roleExists = await _conferenceRoleRepository.ExistsAsync(updateRoleDTO.RoleId);
                if (!roleExists)
                {
                    throw new ArgumentException($"Role ID {updateRoleDTO.RoleId} does not exist.");
                }

                existingRole.RoleId = updateRoleDTO.RoleId;
                await _roleRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating role for DelegateId {delegateId} and ConferenceId {conferenceId}", ex);
            }
        }

        // Xóa vai trò của đại biểu
        public async Task<bool> DeleteRoleAsync(int delegateId, int conferenceId)
        {
            try
            {
                var role = await _roleRepository.GetByConferenceIdAndDelegateIdAsync(delegateId, conferenceId);
                if (role == null) return false;

                _roleRepository.Remove(role);
                await _roleRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting role for DelegateId {delegateId} and ConferenceId {conferenceId}", ex);
            }
        }
    }
}
