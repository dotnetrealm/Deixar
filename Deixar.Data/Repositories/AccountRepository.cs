using Deixar.Data.Contexts;
using Deixar.Domain.DTOs;
using Deixar.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Deixar.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        readonly ApplicationDBContext _db;

        public AccountRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<UserDetails?> GetUserByEmailPasswordAsync(string email, string password)
        {
            UserDetails? user = await _db.Users.Where(u => u.EmailAddress == email && u.Password == password)
                                .Join(_db.UserRoles, user => user.Id, userrole => userrole.UserId, (user, userroles) => new { user, userroles })
                                .Join(_db.Roles, userrole => userrole.userroles.RoleId, role => role.Id, (userrole, role) => new { userrole, role })
                                .Select(userrole => new UserDetails { User = userrole.userrole.user, Role = userrole.role.RoleName }).FirstOrDefaultAsync();
            return user;
        }
    }
}
