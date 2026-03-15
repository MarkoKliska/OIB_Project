using Autoservice.Domain.Entities;
using Autoservice.Domain.Repositories;
using Autoservice.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Autoservice.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AutoserviceDbContext _context;
    public UserRepository(AutoserviceDbContext context) => _context = context;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.Users.FindAsync([id], ct);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Username == username, ct);

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Users.ToListAsync(ct);

    public async Task AddAsync(User user, CancellationToken ct = default) =>
        await _context.Users.AddAsync(user, ct);
}
