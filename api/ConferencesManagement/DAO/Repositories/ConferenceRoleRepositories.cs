using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConferencesManagementAPI.DAO.Repositories
{
    public class ConferenceRoleRepositories : GenericRepository<ConferenceRole>
    {
        private readonly ConferenceManagementDbContext _context;
        public ConferenceRoleRepositories(ConferenceManagementDbContext context, ILogger<GenericRepository<ConferenceRole>> logger) : base(context, logger)
        {
            _context = context;
        }

        // Kiểm tra xem RoleId có tồn tại không
        public async Task<bool> ExistsAsync(int roleId)
        {
            return await _context.ConferenceRoles.AnyAsync(r => r.Id == roleId);
        }
    }
}
