using Microsoft.EntityFrameworkCore; 
using КурсоваяБД5.Models;  
using System.Threading.Tasks;

namespace КурсоваяБД5
{
    public class DefaultUser
    {
        private readonly FivesemestercswrkContext _context;

        public DefaultUser(FivesemestercswrkContext context)
        {
            _context = context;
        }

        public async Task<int?> GetDefaultUserIdByUserIdAsync(string userId)
        {
            return await _context.Users.Where(e => e.IdentityUserId == userId).Select(e => e.UserId).FirstOrDefaultAsync();
        }

        public async Task<bool> IsDefaultUserIdExistsAsync(string userId)
        {
            return await _context.Users.AnyAsync(e => e.IdentityUserId == userId); // Предполагается, что у вас есть свойство IdentityUserId в модели Customer
        }
    }
}