using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using КурсоваяБД5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace КурсоваяБД5.Pages.User
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly КурсоваяБД5.Models.FivesemestercswrkContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration Configuration;

        public IndexModel(КурсоваяБД5.Models.FivesemestercswrkContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            Configuration = configuration;
        }

        // фильтры
    }
}