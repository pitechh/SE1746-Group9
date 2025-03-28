using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.EntityFrameworkCore;
using static ConferencesManagementAPI.Data.DTO.DelegatesDTO;

namespace ConferencesManagementAPI.DAO.Repositories
{
    public class DelegatesRepositories : GenericRepository<Delegates>
    {
        private readonly ConferenceManagementDbContext _context;
        public DelegatesRepositories(ConferenceManagementDbContext context, ILogger<GenericRepository<Delegates>> logger) : base(context, logger)
        {
            _context = context;
        }
        public Task<Delegates?> GetDelegatesByEmail(string email)
        {
            return _context.Delegates.Include(d => d.Roles).FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}
