using ConferencesManagementAPI.Data.DTO;
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConferencesManagementAPI.DAO.Repositories
{
    public class RegistrationRepositories : GenericRepository<Registration>
    {
        private readonly ConferenceManagementDbContext _context;
        public RegistrationRepositories(ConferenceManagementDbContext context, ILogger<GenericRepository<Registration>> logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<Registration?> GetByConferenceIdAndDelegateIdAsync(int delegateId, int conferenceId)
        {
            return await _context.Registrations.FirstOrDefaultAsync(a => a.ConferenceId == conferenceId && a.DelegateId == delegateId);
        }

        public List<RegistrationResponseDTO> GetByConferenceId(int conferenceId)
        {
            var registrations = _context.Registrations
                                    .Include(a => a.Conference)
                                    .Include(a => a.Delegate)
                                    .Where(a => a.ConferenceId == conferenceId)
                                    .Select(a => new RegistrationResponseDTO
                                    {
                                        Id = a.Id,
                                        ConferenceId = a.ConferenceId,
                                        DelegateId = a.DelegateId,
                                        ConferenceName = a.Conference != null ? a.Conference.Name : "",
                                        DelegateName = a.Delegate != null ? a.Delegate.FullName : "",
                                        DelegateEmail = a.Delegate != null ? a.Delegate.Email : "",
                                        RegisteredAt = a.RegisteredAt,
                                        Status = a.Status,
                                    })
                                    .ToList();
            return registrations;
        }
    }
}
