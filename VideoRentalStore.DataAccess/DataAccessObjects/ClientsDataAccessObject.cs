using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VideoRentalStore.DataAccess.Common;
using VideoRentalStore.Domain.Entities;

namespace VideoRentalStore.DataAccess.DataAccessObjects
{
    public class ClientsDataAccessObject : IDataAccess<Client>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ClientsDataAccessObject(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _applicationDbContext.Clients
                .OrderBy(c => c.FullName)
                .ToListAsync();
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Clients
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task AddAsync(Client entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Added;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Modified;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Client entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Deleted;
            await _applicationDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _applicationDbContext?.Dispose();
        }
    }
}
