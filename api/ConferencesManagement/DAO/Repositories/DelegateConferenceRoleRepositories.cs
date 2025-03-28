using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConferencesManagementAPI.DAO.Repositories
{
    public class DelegateConferenceRoleRepositories : GenericRepository<DelegateConferenceRole>
    {
        private readonly ConferenceManagementDbContext _context;
        public DelegateConferenceRoleRepositories(ConferenceManagementDbContext context, ILogger<GenericRepository<DelegateConferenceRole>> logger) : base(context, logger)
        {
            _context = context;
        }

        // Lấy vai trò của một đại biểu trong một hội thảo theo ID
        public async Task<DelegateConferenceRole?> GetByConferenceIdAndDelegateIdAsync(int delegateId, int conferenceId)
        {
            return await _context.DelegateConferenceRoles
                .FirstOrDefaultAsync(r => r.DelegateId == delegateId && r.ConferenceId == conferenceId);
        }
    }
}
