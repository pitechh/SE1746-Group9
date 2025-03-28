using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;

namespace ConferencesManagementService
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> AuthenticateAsync(AuthRequestDTO request);
        Task<GeneralResponseDTO> RegisterDelegateAsync(RegisterDelegatesDTO newDelegates);
    }
}
