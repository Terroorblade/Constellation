using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1
{
    public class UseresService
    {
        private readonly ConsttestContext _context;

         public UseresService( ConsttestContext context)
    {
        _context = context;
    }
         public async Task<int?> GetPerformerIdByUserIdAsync(string userId)
        {
            return await _context.Users
                .Where(c => c.IdentityUserId == userId)
                .Select(c => c.IdUser)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsPerformerIdExistsAsync(string userId)
        {
            return await _context.Users
                .AnyAsync(p => p.IdentityUserId == userId);
        }
    }
}