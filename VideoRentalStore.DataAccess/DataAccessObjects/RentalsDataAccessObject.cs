using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VideoRentalStore.DataAccess.Common;
using VideoRentalStore.Domain.Entities;

namespace VideoRentalStore.DataAccess.DataAccessObjects
{
    public class RentalsDataAccessObject : IDataAccess<Rental>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public RentalsDataAccessObject(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            return await _applicationDbContext.Rentals
                .Include(x => x.Client)
                .Include(x => x.Movie)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<Rental> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Rentals
                .Include(a => a.Client)
                .Include(a => a.Movie)
                .Where(a => a.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task AddAsync(Rental entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Added;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Rental entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Modified;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Rental entity)
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
