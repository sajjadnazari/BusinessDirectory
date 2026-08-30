using BusinessDirectory.Application.Interfaces;
using BusinessDirectory.Domain.Entities;
using BusinessDirectory.Infrastructure.Data;

namespace BusinessDirectory.Infrastructure.Repositories
{
    internal sealed class BusinessRepository(DirectoryDbContext dbContext) : IBusinessRepository
    {
        public async Task AddAsync(BusinessProfile business, CancellationToken cancellationToken = default)
        {
            await dbContext.BusinessProfiles.AddAsync(business, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
