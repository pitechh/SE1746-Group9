using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConferencesManagementAPI.DAO.Repositories
{
    public class ConferenceHostingRegistrationRepositories : GenericRepository<ConferenceHostingRegistration>
    {
        private readonly ConferenceManagementDbContext _context;
        public ConferenceHostingRegistrationRepositories(ConferenceManagementDbContext context, ILogger<GenericRepository<ConferenceHostingRegistration>> logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<List<ConferenceHostingRegistration>> GetAllRegistrationAsync()
        {
            return await _context.ConferenceHostingRegistrations.Include(a => a.Register).ToListAsync();
        }

        public async Task<ConferenceHostingRegistration?> GetRegistrationByIdAsync(int id)
        {
            return await _context.ConferenceHostingRegistrations.Include(a => a.Register).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<ConferenceHostingRegistration>> GetByDelegateIdAsync(int delegateId)
        {
            return await _context.ConferenceHostingRegistrations.Include(a => a.Register)
                .Where(r => r.RegisterId == delegateId)
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(ConferenceHostingRegistration registration)
        {
            _context.ConferenceHostingRegistrations.Add(registration);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(ConferenceHostingRegistration registration)
        {
            _context.ConferenceHostingRegistrations.Update(registration);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var registration = await GetByIdAsync(id);
            if (registration == null) return false;

            _context.ConferenceHostingRegistrations.Remove(registration);
            return await _context.SaveChangesAsync() > 0;
        }


    }
}
