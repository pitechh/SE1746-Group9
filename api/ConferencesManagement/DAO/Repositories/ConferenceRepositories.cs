using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConferencesManagementAPI.DAO.Repositories
{
    public class ConferenceRepositories : GenericRepository<Conference>
    {
        private readonly ConferenceManagementDbContext _context;
        public ConferenceRepositories(ConferenceManagementDbContext context, ILogger<GenericRepository<Conference>> logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<Conference?> GetConferenceByIdAsync(int id)
        {
            try
            {
                return await _context.Conferences.Include(a => a.HostByNavigation).FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
